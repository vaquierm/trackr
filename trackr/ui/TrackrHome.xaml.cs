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

        public TrackrHome()
        {
            _viewModel = new TrackrHomeViewModel();
            this.DataContext = _viewModel;

            InitializeComponent();

            _welcomeTimer = new Timer(5000);
            _welcomeTimer.Elapsed += this.OnTimedEvent;
            _welcomeTimer.Start();

            CameraController.InitializeToDefaultCamera();
            
            // --------------------- BACKEND TESTS, IGNORE PLS ---------------------------
            //var wrk = new Workspace();
            //wrk.AddNewPatient(new TherapyPatient("Dummy", "McDummyFace", Gender.Other, DateTime.Today));
            /*wrk.ActivePatient.NewSession();
            Thread.Sleep(5000);
            wrk.ActivePatient.EndSession();
            wrk.SaveActivePatient(false);*/
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
    }
}
