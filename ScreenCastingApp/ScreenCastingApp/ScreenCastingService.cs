using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenCastingApp
{
    public class ScreenCastingService : IScreenCastingService
    {
        static System.Timers.Timer timer = new System.Timers.Timer(3000);

        public static void RegisterScreenCastTimer()
        {
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        private static void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                               Screen.PrimaryScreen.Bounds.Height,
                                               PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);
            using (MemoryStream converter = new MemoryStream())
            {
                bmpScreenshot.Save(converter, ImageFormat.Jpeg);
                CurrentScreen = converter.ToArray();
            }


        }

        //MemoryStream _resultStream = new MemoryStream();
        //MemoryStream ResultStream
        //{
        //    get
        //    {
        //        var @lock = new object();
        //        lock (@lock)
        //        {
        //            //Create a new stream
        //            var newStream = new MemoryStream();

        //            //If there is an old stream, flush and dispose it
        //            if (_resultStream != null)
        //            {
        //                _resultStream.Flush();
        //                _resultStream.Dispose();
        //            }

        //            //Create a reference to the stream
        //            _resultStream = newStream;

        //            return newStream;
        //        }
        //    }
        //}

        static byte[] _currentScreen;
        static byte[] CurrentScreen
        {
            get
            {
                //var @lock = new object();
                //lock (@lock)
                //{
                return _currentScreen;
                //}
            }

            set
            {
                //var @lock = new object();
                //lock (@lock)
                //{
                _currentScreen = value;
                //}
            }
        }

        static string currentMachine = "";

        //[WebGet(UriTemplate = "SayHello", BodyStyle = WebMessageBodyStyle.Bare)]
        public string SayHello()
        {
            return string.Format("Hello - " + DateTime.Now);
        }

        public void PostImage(Stream img)//PostImageElement elem)
        {
            //currentScreen = elem.Image;
            //currentMachine = elem.Name;
            //currentScreen = img;
        }

        [WebGet(UriTemplate = "cast", BodyStyle = WebMessageBodyStyle.Bare)]
        public Stream Cast()
        {
            string result = File.ReadAllText(@"index.html");
            byte[] resultBytes = Encoding.UTF8.GetBytes(result);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return new MemoryStream(resultBytes);
        }

        [WebGet(UriTemplate = "image", BodyStyle = WebMessageBodyStyle.Bare)]
        public Stream GetImage()
        {
            //MemoryStream resultStream = currentScreen;
            MemoryStream resultStream = new MemoryStream();

            if (CurrentScreen != null)
            {
                try
                {
                    resultStream.Write(CurrentScreen, 0, CurrentScreen.Length);                    
                    //CurrentScreen.Save(resultStream, ImageFormat.Jpeg); //@"A:\temp\screen"+ DateTime.Now.Millisecond + ".jpg"
                                                                        //FileStream fs = new FileStream(@"A:\temp\screen950.jpg", FileMode.Open);
                    resultStream.Seek(0, SeekOrigin.Begin);
                    resultStream.Position = 0L;
                }
                catch (Exception)
                {
                    Console.WriteLine("CurrentScreen was in use");
                }
            }
            WebOperationContext.Current.OutgoingResponse.ContentType = "image/jpeg";
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Pragma", "no-cache");
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Expires", "0");            
            return resultStream;

        }
    }
}
