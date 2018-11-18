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
using System.Windows.Media.Animation;
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

            if (MainWindow.firstOpen)
            {
                _welcomeTimer = new Timer(2000);
                _welcomeTimer.Elapsed += this.OnTimedEvent;
                _welcomeTimer.Start();
            }
            else
            {
                welcomeImage.Visibility = Visibility.Collapsed;
                welcomeText.Visibility = Visibility.Collapsed;
                trackr.Visibility = Visibility.Collapsed;

                toolbar.Visibility = Visibility.Visible;
            }

            // Initialize the camera controller
            CameraController.InitializeToDefaultCamera();

            InitializePatientButtons();
        }

        private void InitializePatientButtons()
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
                patientButton.Background = Brushes.CadetBlue;
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
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.DemiBold,
                //Text = patient.Info.Name + " " + patient.Info.LastName + "\t\tLast Session: " + "\t\tMNext Session:"  + "\nAge:" + patient.Info.Age() + "\t\t" + lastSession
                //+ "\t\t" + "Next"
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
                patientButton.Content = patientText;

                patientButtons.Add(patientButton);
            }

            patientsControl.ItemsSource = patientButtons;
        }

        private void FadeWelcomeScreen()
        {
            DoubleAnimation da = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(1.5)),
                AutoReverse = false
            };
            da.Completed += anim1_Complete;

            welcomeImage.BeginAnimation(OpacityProperty, da);
            welcomeText.BeginAnimation(OpacityProperty, da);
            trackr.BeginAnimation(OpacityProperty, da);

            toolbar.Visibility = Visibility.Visible;

            MainWindow.firstOpen = false;
        }

        void anim1_Complete(object sender, EventArgs e)
        {
            welcomeImage.Dispatcher.BeginInvoke(new Action(() =>
            {
                welcomeImage.Visibility = Visibility.Collapsed;
            }));
            welcomeText.Dispatcher.BeginInvoke(new Action(() =>
            {
                welcomeText.Visibility = Visibility.Collapsed;
            }));
            trackr.Dispatcher.BeginInvoke(new Action(() =>
            {
                trackr.Visibility = Visibility.Collapsed;
            }));

            toolbar.Dispatcher.BeginInvoke(new Action(() =>
            {
                toolbar.Visibility = Visibility.Visible;
            }));
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
    }
}
