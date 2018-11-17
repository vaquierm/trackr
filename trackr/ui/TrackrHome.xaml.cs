using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using trackr.ui;

namespace trackr
{
    /// <summary>
    /// Interaction logic for TrackrHome.xaml
    /// </summary>
    public partial class TrackrHome : Window
    {
        private readonly TrackrHomeViewModel viewModel;
        private Timer welcomeTimer;

        public TrackrHome()
        {
            viewModel = new TrackrHomeViewModel();
            this.DataContext = viewModel;

            InitializeComponent();

            welcomeTimer = new Timer(5000);
            welcomeTimer.Elapsed += this.OnTimedEvent;
            welcomeTimer.Start();
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
            welcomeText2.Visibility = Visibility.Collapsed;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, (Action)delegate ()
            {
                FadeWelcomeScreen();
            });

            welcomeTimer.Stop();
        }
    }
}
