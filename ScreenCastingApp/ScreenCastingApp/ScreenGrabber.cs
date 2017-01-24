using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenCastingApp
{
    class ScreenGrabber
    {

        public byte[] CaptureScreenshotAsBytes()
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
