using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCastingApp
{
    [ServiceContract]
    public interface IScreenCastingService
    {
        [OperationContract]
        string SayHello();

        [OperationContract]
        Stream Cast();

        [OperationContract]
        Stream GetImage();

        [OperationContract]
        void PostImage(Stream img);//PostImageElement elem);
    }

    public class PostImageElement{

        private Stream _image;

        public Stream Image
        {
            get { return _image; }
            set { _image = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
