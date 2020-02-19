using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
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

namespace RunningTest_Arduino
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort mainport = new SerialPort();
        DateTime dt1, dt2 = new DateTime();
        string startTime;
        string endTime;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!mainport.IsOpen)
            {
                try
                {
                    mainport.PortName = COMPortList.SelectedItem.ToString();
                    mainport.BaudRate = 9600;
                    mainport.DataBits = 8;
                    mainport.StopBits = StopBits.One;
                    mainport.Parity = Parity.None;
                }
                catch (Exception owo) { MessageBox.Show(owo.Message); }
                try
                {
                    //Connect
                    mainport.Open();
                    ((Button)e.Source).Content = "Disconnect";
                    
                }
                catch (Exception owo) { MessageBox.Show(owo.Message); }
            }
            else
            {
                try
                {
                    Task.WhenAll(tasks);
                    mainport.Close();
                    ((Button)e.Source).Content = "Connet";
                }
                catch (Exception owo) { MessageBox.Show(owo.Message); }
            }
        }
        private List<Task> tasks = new List<Task>();

        private void StartReceivingData()
        {
            tasks.Add(StartListening());
        }

        
        private async Task StartListening()
        {
            int a = 30;
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    while (mainport.IsOpen)
                    {
                        try
                        {
                            string message = mainport.ReadLine();
                            Application.Current.Dispatcher
                            .BeginInvoke(new Action(() => { time1.Content = startTime; }));

                            if (message != null)
                            {
                                if (a >= Convert.ToInt32(message))
                                {
                                    dt2 = DateTime.Now;
                                    Application.Current.Dispatcher
                                    .BeginInvoke(new Action(() => showValue()));

                                    endTime = dt2.ToString("mm분 ss.fff");
                                    Application.Current.Dispatcher
                                    .BeginInvoke(new Action(() => { time2.Content = endTime; }));

                                }
                            }
                            

                            

                        }
                        catch (TimeoutException) { }
                        catch (IOException) { }
                    }
                });
            }
            catch { }
        }

        int count = 1;
        private void showValue()
        {
            TimeSpan dt3 = dt2 - dt1;
            int minute = dt3.Minutes;
            int second = dt3.Seconds;
            int milli = dt3.Milliseconds;

            string items = count + " 번째 도착자 : " + minute + "분" + second + "." + milli + "초"  ;
            ListBox1.Items.Add(items);
            count++;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (string comPort in SerialPort.GetPortNames())
            {
                COMPortList.Items.Add(comPort);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            dt1 = DateTime.Now;
            startTime = dt1.ToString("mm분 ss.fff");
            StartReceivingData();

        }
    }
}
