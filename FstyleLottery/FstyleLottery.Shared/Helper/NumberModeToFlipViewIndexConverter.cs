using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace FstyleLottery.Helper
{
    class NumberModeToFlipViewIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value == true ? 1 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (int)value == 1 ? true : false;
        }
    }
}
