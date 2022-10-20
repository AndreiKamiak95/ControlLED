using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ControlLED.Model;
using System.Timers;
using System.Threading.Tasks;
using ControlLED.Classes;

namespace ControlLED.View
{
    class TcpChannelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        TcpChannel tcpChannel;
        LedPwm ledPwm;

        public TcpChannelViewModel()
        {
            tcpChannel = TcpChannel.getInstance();
            tcpChannel.ReceiveLedPwm += TcpChannel_ReceiveLedPwm;

            ledPwm = new LedPwm();
        }

        private void TcpChannel_ReceiveLedPwm(LedPwm recvLedPwm)
        {
            ledPwm = (LedPwm)recvLedPwm.Clone();
        }

        public byte PWM
        {
            get { return ledPwm.PWM; }
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

        public bool StatusWork
        {
            get { return ledPwm.StatusWork; }
            set
            {
                if (ledPwm.StatusWork != value)
                {
                    ledPwm.StatusWork = value;
                    OnPropertyChanged("ChangeStatusWork");
                }
            }
        }
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
