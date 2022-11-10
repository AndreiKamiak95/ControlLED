using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ControlLED.Classes;
using System.Timers;

namespace ControlLED.Model
{
    internal class TcpChannel
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }

        TcpClient tcpClient;
        NetworkStream networkStream;
        Task taskListenServer;
        public delegate void RecieveLedPwm(LedPwm ledPwm);
        public event RecieveLedPwm ReceiveLedPwm;
        Timer timer;
        short countSeconds = 0;
        short timeOutConnected = 10;
        IAsyncResult rezult;
        public delegate void TcpChannelHandler();
        public event TcpChannelHandler ErrorConnection;
        public event TcpChannelHandler SuccessfulConnection;

        private static object syncRoot = new object(); //lock
        private static TcpChannel instance;

        public static TcpChannel getInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new TcpChannel();
                    }
                }
            }
            return instance;
        }

        private TcpChannel()
        {
            tcpClient = new TcpClient();
            timer = new Timer
            {
                Enabled = false,
                AutoReset = true,
                Interval = 1000
            };
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (rezult.IsCompleted == false)
            {
                countSeconds++;
                if (countSeconds >= timeOutConnected)
                {
                    timer.Stop();
                    countSeconds = 0;
                    tcpClient.Close();
                    ErrorConnection?.Invoke();
                }
            }
        }

        public void SendData(LedPwm ledPwm)
        {
            try
            {
                networkStream.Write(ledPwm.ToByteArray(), 0, ledPwm.ToByteArray().Length);
            }
            catch (System.IO.IOException)
            {
                tcpClient.Close();
                tcpClient = new TcpClient(IpAddress, Port);
                networkStream = tcpClient.GetStream();
            }
        }

        public void ListenServer()
        {
            while (true)
            {
                if (networkStream.DataAvailable)
                {
                    byte[] recvData = new byte[networkStream.Length];
                    networkStream.Read(recvData, 0, recvData.Length);
                    LedPwm ledPwm = new LedPwm
                    {
                        PWM = recvData[0],
                        StatusWork = Convert.ToBoolean(recvData[1])
                    };
                    ReceiveLedPwm?.Invoke(ledPwm);
                }
            }
        }

        public void ConnectCallback(IAsyncResult result)
        {
            try
            {
                tcpClient.EndConnect(result);
                networkStream = tcpClient.GetStream();
                taskListenServer = new Task(ListenServer);
                taskListenServer.Start();
                timer.Stop();
                countSeconds = 0;
                SuccessfulConnection?.Invoke();
            }
            catch (Exception)
            {
                
            }
        }

        public void Connect(string ipAddress, int port)
        {
            //tcpClient.Connect(IpAddress, Port);
            //networkStream = tcpClient.GetStream();
            //taskListenServer = new Task(ListenServer);
            //taskListenServer.Start();
            timer.Start();
            tcpClient = new TcpClient();
            rezult = tcpClient.BeginConnect(ipAddress, port, new AsyncCallback(ConnectCallback), tcpClient);
        }
    }
}
