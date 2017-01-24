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

        static void Main(string[] args)
        {

            Uri baseAddress = new Uri("http://localhost:9988/");

            ScreenCastingService svc = new ScreenCastingService();

            using (ServiceHost host = new ServiceHost(svc, baseAddress))
            {
                // Enable metadata publishing.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);
                
                while(true)
                {
                    try
                    {
                        // Open the ServiceHost to start listening for messages. Since
                        // no endpoints are explicitly configured, the runtime will create
                        // one endpoint per base address for each service contract implemented
                        // by the service.
                        host.Open();

                        svc.Start();

                        Console.WriteLine("The service is ready at {0}", baseAddress);
                        Console.WriteLine("Press <Enter> to stop the service.");
                        Console.ReadLine();
                        System.Environment.Exit(1);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("The screencaster service could not be started.\n{0}\nDo you want to try again? y/n", ex.Message);
                        var result = Console.ReadKey();
                        if (result.Key != ConsoleKey.Y)
                        {
                            System.Environment.Exit(1);
                        }
                    }
                }
            }
        }
    }
}
