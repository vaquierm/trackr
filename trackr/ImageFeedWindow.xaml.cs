using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Navigation;
using System.Diagnostics;
using trackr.imgProcessing;
using static trackr.imgProcessing.CameraController;

namespace trackr
{
    /// <summary>
    /// Interaction logic for ImageFeedWindow.xaml
    /// </summary>
    public partial class ImageFeedWindow : Window
    {

        public ImageFeedWindow()
        {
            InitializeComponent();

            CameraController.NewFrameAcquired += NewFrameHandler;
        }

        private void NewFrameHandler(object sender, EventArgs e)
        {
            if (!(e is NewFrameAcquiredEventArgs frameArgs))
                return;


            // Get the bitmap image
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                frameHolder.Source = frameArgs.Img;
            }));
        }

        private void configCameraButton_Click(object sender, RoutedEventArgs e)
        {
            CameraController.SwitchCamera();
        }
    }
}
