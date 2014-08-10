using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace FstyleLottery.DataModel
{
    public class LotteryItem : ObservableObject
    {
        public LotteryItem()
        {
            this.Text = "";
        }
        public LotteryItem(string text)
        {
            this.Text = text;
        }

        public bool IsNotYet = true;
        private string _text;
        [JsonProperty("text")]
        public string Text
        {
            get {return _text; }
            set {
                _text = value;
                RaisePropertyChanged();
            }
        }
    }
}
