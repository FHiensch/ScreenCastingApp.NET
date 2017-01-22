using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ServiceModel.Web;

namespace ScreenCastingApp
{
    class Program
    {
        public static Uri baseAddress;

        static void Main(string[] args)
        {
            baseAddress = new Uri("http://localhost:9988/");

            // Create the ServiceHost.
            using (ServiceHost host = new ServiceHost(typeof(ScreenCastingService), baseAddress))
            {
                // Enable metadata publishing.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);

                bool retry = false;
                bool error = false;
                do
                {
                    try
                    {
                        retry = false;
                        error = false;

                        // Open the ServiceHost to start listening for messages. Since
                        // no endpoints are explicitly configured, the runtime will create
                        // one endpoint per base address for each service contract implemented
                        // by the service.
                        host.Open();
                    }
                    catch (Exception ex)
                    {
                        error = true;
                        Console.WriteLine("Der Dienst konnte nicht gestartet werden. \n" + ex.Message + "\nMöchten Sie es erneut versuchen? j/n");
                        var result = Console.ReadKey();
                        if (result.Key == ConsoleKey.J)
                        {
                            retry = true;
                        }
                    }
                } while (retry);

                if (error)
                {
                    return;
                }

                ScreenCastingService.RegisterScreenCastTimer();

                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                // Close the ServiceHost.
                host.Close();
            }

        }
    }




}
