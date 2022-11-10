﻿using ControlLED.Classes;
using ControlLED.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ControlLED
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            //BindingContext = new TcpChannelViewModel();
            BindingContext = LedPwmViewModel.TcpChannelViewModel;
        }
    }
}
