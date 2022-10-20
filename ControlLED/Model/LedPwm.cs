using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ControlLED.Classes
{
    public class LedPwm : ICloneable
    {
        public byte PWM { get; set; }
        public bool StatusWork { get; set; }

        public object Clone()
        {
            return new LedPwm(PWM, StatusWork);
        }

        public LedPwm()
        {
            PWM = 0;
            StatusWork = true;
        }
        public LedPwm(byte pwm, bool statusWork)
        {
            PWM = pwm;
            StatusWork = statusWork;
        }

        public byte[] ToByteArray()
        {
            byte[] array = new byte[2];
            array[0] = PWM;
            array[1] = Convert.ToByte(StatusWork);

            return array;
        }
    }
}
