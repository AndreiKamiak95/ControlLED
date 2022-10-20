using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ControlLED.Classes;

namespace ControlLED.Model
{
    class TcpChannel
    {
        public string IpAddress { get; set; } = "192.168.137.214";
        public int Port { get; set; } = 80;

        TcpClient tcpClient;
        NetworkStream networkStream;
        Task taskListenServer;
        public delegate void RecieveLedPwm(LedPwm ledPwm);
        public event RecieveLedPwm ReceiveLedPwm;



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
            if (!string.IsNullOrEmpty(IpAddress))
            {
                tcpClient = new TcpClient(IpAddress, Port);
                networkStream = tcpClient.GetStream();
                taskListenServer = new Task(ListenServer);
                taskListenServer.Start();
            }
        }

        public void ChangeIpAddresAndPort(string ipAddress, int port)
        {
            tcpClient.Connect(ipAddress, port);
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
                    LedPwm ledPwm = new LedPwm();
                    ledPwm.PWM = recvData[0];
                    ledPwm.StatusWork = Convert.ToBoolean(recvData[1]);
                    ReceiveLedPwm?.Invoke(ledPwm);
                }
            }
        }
    }
}
