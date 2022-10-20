using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ControlLED.View;

namespace ControlLED.Classes
{
    class LedPwmViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private TcpChannelViewModel tcpChannelViewModel;

        public LedPwmViewModel()
        {
            tcpChannelViewModel = new TcpChannelViewModel();
        }

        public byte ChangePwm
        {
            get { return tcpChannelViewModel.PWM; }
            set
            {
                if (tcpChannelViewModel.PWM != value)
                {
                    tcpChannelViewModel.PWM = value;
                    OnPropertyChanged("ChangePwm");
                }
            }
        }
        
        public bool ChangeWork
        {
            get { return tcpChannelViewModel.StatusWork; }
            set
            {
                if (tcpChannelViewModel.StatusWork != value)
                {
                    tcpChannelViewModel.StatusWork = value;
                    OnPropertyChanged("ChangeWork");
                }
            }
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
