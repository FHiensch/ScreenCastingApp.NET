using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenCastingApp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]  
    public class ScreenCastingService : IScreenCastingService
    {
        private readonly System.Timers.Timer timer;
        private readonly ScreenGrabber screenGrabber;

        private volatile byte[] currentImageData;

        public ScreenCastingService()
        {
            screenGrabber = new ScreenGrabber();
            timer = new System.Timers.Timer(150);
            timer.Elapsed += TimerElapsed;
        }

        public void Start()
        {
            timer.Start();
        }

        private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            currentImageData = screenGrabber.CaptureScreenshotAsBytes();
        }
        
        [WebGet(UriTemplate = "ping", BodyStyle = WebMessageBodyStyle.Bare)]
        public string Ping()
        {
            return string.Format("{0}",DateTime.Now);
        }

        [WebGet(UriTemplate = "cast", BodyStyle = WebMessageBodyStyle.Bare)]
        public Stream Cast()
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(@"index.html")));
        }

        [WebGet(UriTemplate = "image", BodyStyle = WebMessageBodyStyle.Bare)]
        public Stream GetImage()
        {

            byte[] imageData = WaitForNextAvailableImage();

            WebOperationContext.Current.OutgoingResponse.ContentType = "image/jpeg";
            WebOperationContext.Current.OutgoingResponse.ContentLength = imageData.Length;

            WebOperationContext.Current.OutgoingResponse.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Pragma", "no-cache");
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Expires", "0");

            return new MemoryStream(imageData);
        }

        private byte[] WaitForNextAvailableImage()
        {
            byte[] imageData = currentImageData;
            while (imageData == null)
            {
                imageData = currentImageData;
                System.Threading.Thread.Sleep(50);
            }
            return imageData;
        }
    }
}
