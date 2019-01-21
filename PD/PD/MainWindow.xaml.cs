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

using System.IO.Ports;
using System.Timers;
using System.Threading;

namespace PD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort port_PD;

        Thread thread1;
        System.Threading.Timer timer1;

        public MainWindow()
        {
            InitializeComponent();

            port_PD = new SerialPort("COM2", 115200, Parity.None, 8, StopBits.One);

            port_PD.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);

            thread1 = new Thread(_MainFunction); //Set thread function           

        }

        private void _MainFunction()
        {
            TimerCallback callback = new TimerCallback(callback_Tick);    //Get PD value
            timer1 = new System.Threading.Timer(callback, null, 0, 300);  //Timer setting and start
        }

        private void callback_Tick(object state)
        {
            port_PD.Write("70303f\r");

            Thread.Sleep(2);

            //讀入字串
            string data = port_PD.ReadExisting();

            Console.WriteLine("Receive: " + data);
        }

        private void btn_GO_Click(object sender, RoutedEventArgs e)
        {
            port_PD.Open();

            thread1.Start(); //Start thread1
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            port_PD.Close();
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //讀入字串
            string data = port_PD.ReadExisting();
            Console.WriteLine("Receive: " + data);

            /*
             //讀入位元組
            int bytes = port.BytesToRead;
            byte[] comBuffer = new byte[bytes];
            port.Read(comBuffer, 0, bytes);

            Console.WriteLine(comBuffer);
            */
        }
    }
}
