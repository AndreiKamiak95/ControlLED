using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ControlLED.Classes;
using Xamarin.Essentials;

namespace ControlLED.Model
{
    class TcpChannel
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }

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
                    LedPwm ledPwm = new LedPwm
                    {
                        PWM = recvData[0],
                        StatusWork = Convert.ToBoolean(recvData[1])
                    };
                    ReceiveLedPwm?.Invoke(ledPwm);
                }
            }
        }

        public void Connect()
        {
            if (string.IsNullOrEmpty(IpAddress))
            {
                return;
            }
            tcpClient = new TcpClient(IpAddress, Port);
            networkStream = tcpClient.GetStream();
            taskListenServer = new Task(ListenServer);
            taskListenServer.Start();
        }
    }
}
