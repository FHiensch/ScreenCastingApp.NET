using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenCastingApp
{
    class ScreenGrabber
    {
        private readonly System.Timers.Timer timer;

        private byte[] currentImageData;

        public ScreenGrabber()
        {
            timer = new System.Timers.Timer(150);
            timer.Elapsed += TimerElapsed;
        }

        public void Start()
        {
            timer.Start();
        }

        public byte[] WaitForNextAvailableImage()
        {
            byte[] imageData = currentImageData;
            while (imageData == null)
            {
                imageData = currentImageData;
                System.Threading.Thread.Sleep(50);
            }
            return imageData;
        }

        private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Interlocked.Exchange(ref currentImageData, CaptureScreenshotAsBytes());
        }

        private byte[] CaptureScreenshotAsBytes()
        {
            var offScreenImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                            Screen.PrimaryScreen.Bounds.Height,
                                            PixelFormat.Format32bppArgb);

            Graphics.FromImage(offScreenImage)
                    .CopyFromScreen(0, 0, 0, 0, offScreenImage.Size, CopyPixelOperation.SourceCopy);

            using (MemoryStream imageData = new MemoryStream())
            {
                offScreenImage.Save(imageData, ImageFormat.Jpeg);
                return imageData.ToArray();
            }
        }
    }
}
