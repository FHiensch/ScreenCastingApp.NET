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
        private readonly ScreenGrabber screenGrabber;

        public ScreenCastingService()
        {
            screenGrabber = new ScreenGrabber();
        }

        public void Start()
        {
            screenGrabber.Start();
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

            byte[] imageData = screenGrabber.WaitForNextAvailableImage();

            WebOperationContext.Current.OutgoingResponse.ContentType = "image/jpeg";
            WebOperationContext.Current.OutgoingResponse.ContentLength = imageData.Length;

            WebOperationContext.Current.OutgoingResponse.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Pragma", "no-cache");
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Expires", "0");

            return new MemoryStream(imageData);
        }
    }
}
