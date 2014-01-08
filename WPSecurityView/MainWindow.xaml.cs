using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace WPSecurityView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            new Thread(() =>
                {
                    var u = new UdpClient(new IPEndPoint(IPAddress.Any, 11000));

                    while (true)
                    {
                        IPEndPoint ip = null;
                        var bytes = u.Receive(ref ip);
                        Console.WriteLine(bytes.Length);

                        Dispatcher.Invoke(() =>
                            {
                                JpegBitmapDecoder decoder = null;
                                BitmapSource bitmapSource = null;
                                using (var stream = new MemoryStream(bytes))
                                {
                                    decoder = new JpegBitmapDecoder(stream,
                                                                    BitmapCreateOptions.PreservePixelFormat,
                                                                    BitmapCacheOption.OnLoad);
                                }
                                bitmapSource = decoder.Frames[0];
                                bitmapSource.Freeze();
                                img.Source = bitmapSource;
                            });
                    }
                }).Start();
        }
    }
}
