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

namespace trackr.imgProcessing
{
    class CameraController
    {

        public static VideoCaptureDevice Camera;
        public static FilterInfoCollection LoaclWebCamsCollection;

        public static void InitializeToDefaultCamera()
        {
            LoaclWebCamsCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            Camera = new VideoCaptureDevice(LoaclWebCamsCollection[0].MonikerString);
        }

        public static void SwitchCamera()
        {

            VideoCaptureDeviceForm form = new VideoCaptureDeviceForm();


            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Camera != null)
                {
                    // Remove handler and stop camera
                    Camera.NewFrame -= new NewFrameEventHandler(Cam_NewFrame);
                    Camera.Stop();
                }

                Camera = form.VideoDevice;

                //Restart the new camera
                Camera.NewFrame += new NewFrameEventHandler(Cam_NewFrame);
                Camera.Start();
            }

        }

        private static void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                System.Drawing.Image img = (Bitmap)eventArgs.Frame.Clone();

                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();

                bi.Freeze();

                var imgArgs = new NewFrameAcquiredEventArgs();
                imgArgs.Img = bi;

                NewFrameAcquired.Invoke(null, imgArgs);
            }
            catch (Exception ex)
            {
            }

        }

        public class NewFrameAcquiredEventArgs : EventArgs
        {
            public BitmapImage Img { get; set; }
        }

        public static event EventHandler NewFrameAcquired;

        protected virtual void OnNewFrame(EventArgs e)
        {
            var handler = NewFrameAcquired;
            handler(this, e);
        }
    }
}
