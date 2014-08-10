using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FstyleLottery.Controls
{
    [TemplatePart(Name = NumericUpDown.UpButtonPartName, Type = typeof(ButtonBase))]
    [TemplatePart(Name = NumericUpDown.DownButtonPartName, Type = typeof(ButtonBase))]
    public sealed class NumericUpDown : Control
    {
        public delegate void ValueChangedHandler(object sender);
        public event ValueChangedHandler ValueChanged;

        private const string UpButtonPartName = "UpButtonPart";
        private const string DownButtonPartName = "DownButtonPart";

        private ButtonBase UpButton { get; set; }
        private ButtonBase DownButton { get; set; }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static DependencyProperty ValueProperty { get; private set; }

        static NumericUpDown()
        {
            ValueProperty = DependencyProperty.Register(
                "Value",
                typeof(int),
                typeof(NumericUpDown),
                new PropertyMetadata(0));
        }

        public NumericUpDown()
        {
            this.DefaultStyleKey = typeof(NumericUpDown);
        }

        protected override void OnApplyTemplate()
        {
            if (this.UpButton != null)
            {
                this.UpButton.Click -= UpButton_Click;
            }

            if (this.DownButton != null)
            {
                this.DownButton.Click -= DownButton_Click;
            }

            this.UpButton = this.GetTemplateChild(UpButtonPartName) as ButtonBase;
            this.DownButton = this.GetTemplateChild(DownButtonPartName) as ButtonBase;

            if (this.UpButton != null)
            {
                this.UpButton.Click += UpButton_Click;
            }

            if (this.DownButton != null)
            {
                this.DownButton.Click += DownButton_Click;
            }

            base.OnApplyTemplate();
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Value > 0)
            {
                this.Value--;
                ValueChanged(this);
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Value < 9)
            {
                this.Value++;
                ValueChanged(this);
            }
        }
    }
}
