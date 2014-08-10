using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace FstyleLottery.Helper
{
    class InvertedBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? false : true;
        }
    }
}
