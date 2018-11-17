using AForge.Video;
using AForge.Video.DirectShow;
using AForge;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using trackr.ImageProcessing;

namespace trackr.imgProcessing
{
    class CameraController
    {

        private static VideoCaptureDevice Camera;
        private static FilterInfoCollection LoaclWebCamsCollection;

        public static bool EmotionCalculationEnabled { set; get; }

        public static void InitializeToDefaultCamera()
        {
            LoaclWebCamsCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            Camera = new VideoCaptureDevice(LoaclWebCamsCollection[0].MonikerString);
            Camera.NewFrame += new NewFrameEventHandler(Cam_NewFrame);
        }


        /// <summary>
        /// Pops us the window to change the selected camera
        /// </summary>
        /// <param name="start">Parameter to start the camera capture. Defults to true</param>
        public static void SwitchCamera(bool start = true)
        {

            VideoCaptureDeviceForm form = new VideoCaptureDeviceForm();


            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Camera != null)
                {
                    // Remove handler and stop camera
                    Camera.NewFrame -= new NewFrameEventHandler(Cam_NewFrame);
                    StopCapture();
                }

                Camera = form.VideoDevice;

                //Restart the new camera
                Camera.NewFrame += new NewFrameEventHandler(Cam_NewFrame);
                if (start)
                    StartCapture();
            }

        }

        /// <summary>
        /// Starts the capture of the selected camera
        /// </summary>
        public static void StartCapture()
        {
            if (!Camera.IsRunning)
                Camera.Start();
            LastEmotionDataTime = DateTime.Now;
        }

        /// <summary>
        /// Stops the capture of the selected camera
        /// </summary>
        public static void StopCapture()
        {
            if (Camera.IsRunning)
                Camera.Stop();
        }


        private static DateTime LastEmotionDataTime;
        
        private static void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                // Get the image
                System.Drawing.Image img = (Bitmap)eventArgs.Frame.Clone();

                // Convert the image to bitmap
                var ms = new MemoryStream();
                img.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();

                bi.Freeze();

                var imgArgs = new NewFrameAcquiredEventArgs();
                imgArgs.Img = bi;

                //NewFrameAcquired.Invoke(null, imgArgs);
                NewFrameAcquired(null, imgArgs);

                // If emotion calculations are required calculate them
                if (EmotionCalculationEnabled)
                {
                    var currTime = DateTime.Now;
                    if (currTime - LastEmotionDataTime > new TimeSpan(0, 0, 0, 3, 0))
                    {
                        LastEmotionDataTime = currTime;

                        var resultEmotionData = FaceApiController.CalculateEmotions(ImageToByteArray(img));

                        var emotionsEventArgs = new NewEmotionDataEventArgs();

                        emotionsEventArgs.Data = resultEmotionData;

                        Console.Write(resultEmotionData);

                        NewEmotionDataAvailible.Invoke(null, emotionsEventArgs);
                    }
                }

            }
            catch (Exception ex)
            {
            }

        }

        private static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        /// <summary>
        /// Class used to create hold the image that is currently being captured
        /// </summary>
        public class NewFrameAcquiredEventArgs : EventArgs
        {
            public BitmapImage Img { get; set; }
        }

        /// <summary>
        /// Hook onto this event to get the last frame captured
        /// </summary>
        public static event EventHandler NewFrameAcquired;

        /// <summary>
        /// Class used to pass the emotiondata through events
        /// </summary>
        public class NewEmotionDataEventArgs : EventArgs
        {
            public EmotionData Data;
        }

        /// <summary>
        /// Hook onto this event to get emotion data
        /// </summary>
        public static event EventHandler NewEmotionDataAvailible;

    }
}
