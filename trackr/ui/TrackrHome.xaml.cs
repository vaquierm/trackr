using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using trackr.core;
using trackr.imgProcessing;
using trackr.ui;
using Timer = System.Timers.Timer;

namespace trackr
{
    /// <summary>
    /// Interaction logic for TrackrHome.xaml
    /// </summary>
    public partial class TrackrHome : Page
    {
        private readonly TrackrHomeViewModel _viewModel;
        private readonly Timer _welcomeTimer;

        private List<IdContainerButton> patientButtons;

        public TrackrHome()
        {
            _viewModel = new TrackrHomeViewModel();
            this.DataContext = _viewModel;

            InitializeComponent();

            _welcomeTimer = new Timer(5000);
            _welcomeTimer.Elapsed += this.OnTimedEvent;
            _welcomeTimer.Start();

            // Initialize the camera controller
            CameraController.InitializeToDefaultCamera();

            RefreshPatientButtons();



            // ----------------------------- UNCOMMENT TO CREATE PATIENTS -----------------------------------
            //var ws = Workspace.Instance;

            //ws.AddNewPatient(new TherapyPatient("Tamara", "Hendrix", Gender.Female, new DateTime(1988, 05, 12)));
            //ws.AddNewPatient(new TherapyPatient("George", "Weirsma", Gender.Male, new DateTime(1992, 05, 12)));
            //ws.AddNewPatient(new TherapyPatient("Jared", "Peelee", Gender.Other, new DateTime(1980, 05, 12)));
            //ws.AddNewPatient(new TherapyPatient("Shawn", "Wheeler", Gender.Male, new DateTime(1995, 05, 12)));
            //int i = 1;
            //foreach (TherapyPatient patient in ws.GetPatients())
            //{
            //    patient.GenerateTherapySessionData(2018, i);
            //}

        }

        public void RefreshPatientButtons()
        {
            //Create the list of buttons to keep show on the panel
            patientButtons = new List<IdContainerButton>();

            // Create a button for each patient
            bool first = true;
            foreach (TherapyPatient patient in Workspace.Instance.GetPatients())
            {
                var patientButton = new IdContainerButton();
                patientButton.ID = patient.PatientStringId;
                patientButton.Click += patientButton_Click;
                patientButton.Height = 80;
                patientButton.Width = System.Windows.SystemParameters.PrimaryScreenWidth - 50;
                patientButton.HorizontalAlignment = HorizontalAlignment.Left;
                patientButton.VerticalAlignment = VerticalAlignment.Top;
                if (first)
                {
                    patientButton.Margin = new Thickness(20, 60, 20, 10);
                    first = false;
                }
                else
                {
                    patientButton.Margin = new Thickness(20, 10, 20, 10);
                }

                string lastSession;
                if (patient.GetSessions().Count == 0)
                    lastSession = "No sessions yet.";
                else
                    lastSession = patient.GetSessions().Last().StartDateTime.ToString("dd/MM/yyyy");
                var patientText = new TextBlock
                {
                    Height = 70,
                    Width = System.Windows.SystemParameters.PrimaryScreenWidth - 70,
                    FontSize = 24,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                };

                patientText.Inlines.Add(new Run
                {
                    Text = patient.Info.Name + " " + patient.Info.LastName + "\t\t\tLast Session: " + "\t\t\tNext Session:"
                });
                patientText.Inlines.Add(new LineBreak());
                patientText.Inlines.Add(new Run
                {
                    Text = "Age: " + patient.Info.Age() + "\t\t\t" + lastSession + "\t\t\t" + "Next"
                });

                var grid = new Grid();

                


                patientButton.Content = patientText;

                patientButtons.Add(patientButton);
            }

            patientsControl.ItemsSource = patientButtons;
        }

        private void FadeWelcomeScreen()
        {
            //for (int i = 1; i < 100; i++)
            //{
            //    welcomeImage.Opacity = i;
            //    welcomeText.Opacity = i;
            //    welcomeText2.Opacity = i;
            //}

            welcomeImage.Visibility = Visibility.Collapsed;
            welcomeText.Visibility = Visibility.Collapsed;
            trackr.Visibility = Visibility.Collapsed;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, (Action)FadeWelcomeScreen);

            _welcomeTimer.Stop();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PatientView("Test"));
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            ImageFeedWindow imageFeedWindow = new ImageFeedWindow();
            imageFeedWindow.Show();
        }

        private void patientButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is IdContainerButton button))
                return;

            this.NavigationService.Navigate(new PatientView(button.ID));
        }

        private void newPatientButton_Click(object sender, RoutedEventArgs e)
        {
            var newPatientForm = new NewPatientForm(this);
            newPatientForm.Show();
        }
    }
}
