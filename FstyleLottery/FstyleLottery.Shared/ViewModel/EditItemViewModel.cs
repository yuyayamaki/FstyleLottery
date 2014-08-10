using FstyleLottery.DataModel;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FstyleLottery.ViewModel
{
    public class EditItemViewModel : ViewModelBase
    {
        public EditItemViewModel()
        {
            TextLotteryItems = App.Model.TextLotteryItems;
            IsNumberMode = App.Model.IsNumberMode;
            NumberLottteryItemsCount = App.Model.NumberLottteryItemsCount;
            IsShuffled = App.Model.IsShuffled;
        }

        public ObservableCollection<LotteryItem> TextLotteryItems { get; set; }

        private bool _isNumberMode;
        public bool IsNumberMode
        {
            get { return _isNumberMode; }
            set { Set(ref _isNumberMode, value);
            App.Model.IsNumberMode = value;
            }
        }

        private int _numberLottteryItemsCount;
        public int NumberLottteryItemsCount
        {
            get { return _numberLottteryItemsCount; }
            set { Set(ref _numberLottteryItemsCount, value);
            App.Model.NumberLottteryItemsCount = value;
            }
        }

        private bool _isShuffled;
        public bool IsShuffled
        {
            get { return _isShuffled; }
            set { Set(ref _isShuffled, value);
            App.Model.IsShuffled = value;
            }
        }

    }
}
