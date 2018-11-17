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

            // Disable the emotion data aquisitation for when calibrating and chosing the camera
            CameraController.EmotionCalculationEnabled = false;
            CameraController.NewFrameAcquired += NewFrameHandler;
            CameraController.StartCapture();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //do my stuff before closing
            CameraController.StopCapture();

            //Re enable the emotion data acquisition for when the session starts
            CameraController.EmotionCalculationEnabled = true;
            base.OnClosing(e);
        }

        private void NewFrameHandler(object sender, EventArgs e)
        {
            if (!(e is NewFrameAcquiredEventArgs frameArgs))
                return;

            // Display the image to the window
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                frameHolder.Source = frameArgs.Img;
            }));
        }

        private void configCameraButton_Click(object sender, RoutedEventArgs e)
        {
            CameraController.SwitchCamera();
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
