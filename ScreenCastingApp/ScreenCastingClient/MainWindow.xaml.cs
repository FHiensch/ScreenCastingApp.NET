using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Windows.Threading;
using System.IO;

namespace ScreenCastingClient
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ScreenCastingService.ScreenCastingServiceClient scclient = new ScreenCastingService.ScreenCastingServiceClient();

        DispatcherTimer timer = new DispatcherTimer()
        {
            Interval = new TimeSpan(TimeSpan.TicksPerSecond * 3)
        };

        public MainWindow()
        {
            InitializeComponent();

            scclient.Endpoint.Address = new EndpointAddress("http://localhost:9988");

            MessageBox.Show(scclient.SayHello());

            timer.Tick += TimerTick;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            FileStream fs = File.OpenRead(@"A:\temp\image.jpg");
            scclient.PostImage(fs);//new ScreenCastingService.PostImageElement() { Image = fs, Name = Environment.MachineName });
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
    }
}
