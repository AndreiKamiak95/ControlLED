using System.ComponentModel;
using ControlLED.Model;
using ControlLED.Classes;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace ControlLED.View
{
    class TcpChannelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        TcpChannel tcpChannel;
        LedPwm ledPwm;

        public ICommand NewConnectionCommand { get; }

        public TcpChannelViewModel()
        {
            tcpChannel = TcpChannel.getInstance();
            tcpChannel.ReceiveLedPwm += TcpChannel_ReceiveLedPwm;

            ledPwm = new LedPwm();
            NewConnectionCommand = new Command(NewConnection);

            IpAddress = Preferences.Get("ip", "192.168.1.1");
            Port = Preferences.Get("port", 60);
        }

        private void TcpChannel_ReceiveLedPwm(LedPwm recvLedPwm)
        {
            ledPwm = (LedPwm)recvLedPwm.Clone();
        }

        public byte PWM
        {
            get => ledPwm.PWM;
            set
            {
                if (ledPwm.PWM != value)
                {
                    ledPwm.PWM = value;
                    OnPropertyChanged("ChangePwm");
                    tcpChannel.SendData(ledPwm);
                }
            }
        }
        private string ipAddress;
        public string IpAddress
        {
            get => ipAddress;
            set
            {
                if (ipAddress != value)
                {
                    ipAddress = value;
                    OnPropertyChanged("IpAddress");
                }
            }
        }

        private int port;
        public int Port
        {
            get => port;
            set
            {
                if (port != value)
                {
                    port = value;
                    OnPropertyChanged("Port");
                }
            }
        }
        public bool StatusWork
        {
            get => ledPwm.StatusWork;
            set
            {
                if (ledPwm.StatusWork != value)
                {
                    ledPwm.StatusWork = value;
                    OnPropertyChanged("ChangeStatusWork");
                }
            }
        }

        private bool isValidIp;
        public bool IsValidIp
        {
            get => isValidIp;
            set
            {
                if (isValidIp != value)
                {
                    isValidIp = value;
                    OnPropertyChanged("IsValidIp");
                }
            }
        }
        private bool isValidPort;
        public bool IsValidPort
        {
            get => isValidPort;
            set
            {
                if (isValidPort != value)
                {
                    isValidPort = value;
                    OnPropertyChanged("IsValidPort");
                }
            }
        }

        public void NewConnection()
        {
            //Application.Current.Properties.Add("ip", IpAddress);
            //Application.Current.Properties.Add("port", Port);

            Preferences.Set("ip", IpAddress);
            Preferences.Set("port", Port);

            tcpChannel.Connect();
        }

        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
