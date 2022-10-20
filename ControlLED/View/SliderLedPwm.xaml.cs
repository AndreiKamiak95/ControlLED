using ControlLED.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ControlLED.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SliderLedPwm : ContentView
    {
        public SliderLedPwm()
        {
            InitializeComponent();
            BindingContext = new LedPwmViewModel();
        }
    }
}