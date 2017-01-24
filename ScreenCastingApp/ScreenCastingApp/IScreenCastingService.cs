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
        string Ping();

        [OperationContract]
        Stream Cast();

        [OperationContract]
        Stream GetImage();
    }
}
