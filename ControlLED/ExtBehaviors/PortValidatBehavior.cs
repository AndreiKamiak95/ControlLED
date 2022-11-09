using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ControlLED.ExtBehaviors
{
    public class PortValidatBehavior : Behavior<Entry>
    {
        public bool IsValid { get; set; }
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += Bindable_TextChanged;
            base.OnAttachedTo(bindable);
        }

        private void Bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((Entry)sender).Text))
            {
                int enterPort = Convert.ToInt32(((Entry)sender).Text);
                if ((enterPort <= ushort.MaxValue) && (enterPort >= ushort.MinValue))
                {
                    ((Entry)sender).TextColor = Color.Default;
                    IsValid = true;
                }
                else
                {
                    ((Entry)sender).TextColor = Color.Red;
                    IsValid = false;
                }
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= Bindable_TextChanged;
            base.OnDetachingFrom(bindable);
        }
    }
}
