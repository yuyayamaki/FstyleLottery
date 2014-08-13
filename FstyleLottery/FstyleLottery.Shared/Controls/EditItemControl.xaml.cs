using FstyleLottery.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// ユーザー コントロールのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace FstyleLottery.Controls
{
    public sealed partial class EditItemControl : UserControl
    {
        public EditItemControl()
        {
            this.InitializeComponent();
            SetNumericUpDownValues(NumberLottteryItemsCount);
        }

        public int NumberLottteryItemsCount
        {
            get { return (int)GetValue(LotteryNumberItemsCountProperty); }
            set { SetValue(LotteryNumberItemsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NumberLottteryItemsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LotteryNumberItemsCountProperty =
            DependencyProperty.Register("NumberLottteryItemsCount", typeof(int), typeof(EditItemControl), new PropertyMetadata(50,LotteryNumberItemsCountPropertyChanged));

        private static void LotteryNumberItemsCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var thisControl = (EditItemControl)d;
            thisControl.SetNumericUpDownValues((int)e.NewValue);
        }
        
        private void SetNumericUpDownValues(int numberItemsCount)
        {
            string stringNewValue = numberItemsCount.ToString("0000");

            thousandsPlaceNumber.Value = int.Parse(stringNewValue[0].ToString());
            hundredsPlaceNumber.Value = int.Parse(stringNewValue[1].ToString());
            tensPlaceNumber.Value = int.Parse(stringNewValue[2].ToString());
            onesPlaceNumber.Value = int.Parse(stringNewValue[3].ToString());
        }

        private void NumericUpDown_ValueChanged(object sender)
        {
            int tempNumberCount = int.Parse(thousandsPlaceNumber.Value.ToString() + hundredsPlaceNumber.Value.ToString() + tensPlaceNumber.Value.ToString() + onesPlaceNumber.Value.ToString());
            if (tempNumberCount >= 2)
                NumberLottteryItemsCount = int.Parse(thousandsPlaceNumber.Value.ToString() + hundredsPlaceNumber.Value.ToString() + tensPlaceNumber.Value.ToString() + onesPlaceNumber.Value.ToString());
            else
            {
                onesPlaceNumber.Value = 2;
                NumberLottteryItemsCount = 2;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (((LotteryModel)this.DataContext).TextLotteryItems.Count > 2)
            {
                var button = (Button)sender;
                if (button.DataContext is LotteryItem)
                {
                    var deleteme = (LotteryItem)button.DataContext;
                    ((LotteryModel)this.DataContext).TextLotteryItems.Remove(deleteme);
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ((LotteryModel)this.DataContext).TextLotteryItems.Add(new LotteryItem());
        }
     
    }
}
