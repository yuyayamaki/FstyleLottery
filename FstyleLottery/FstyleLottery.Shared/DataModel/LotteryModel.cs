using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace FstyleLottery.DataModel
{
    public class LotteryModel : ObservableObject
    {
        public LotteryModel()
        {
#if DEBUG
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) return;
#endif

            defaultTextLotteryItems.CollectionChanged += TextLotteryItems_CollectionChanged;
            TextLotteryItems.CollectionChanged += TextLotteryItems_CollectionChanged;

            //// Add DataChanged event handler of RoamingSettings
            //Windows.Storage.ApplicationData.Current.DataChanged += RoamingSettings_DataChanged;
        }

        void TextLotteryItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
        }

        // ApplicationDataContainer for roaming
        private Windows.Storage.ApplicationDataContainer _settings = Windows.Storage.ApplicationData.Current.RoamingSettings;

        //// CoreDispatcher for runing on UI thread
        //public Windows.UI.Core.CoreDispatcher CurrentDispatcher { get; set; }

        //async void RoamingSettings_DataChanged(Windows.Storage.ApplicationData sender, object args)
        //{
        //    if (CurrentDispatcher == null)
        //        throw new InvalidOperationException("RoamingOptionSettingsのCurrentDispatcherプロパティを事前に設定してください。");

        //    await CurrentDispatcher.RunAsync(
        //            Windows.UI.Core.CoreDispatcherPriority.Normal,
        //            () =>
        //            {
        //                // Usually run RasiePropertyChanged
        //            });
        //}

        public ObservableCollection<LotteryItem> MainLotteryItems;
        private ObservableCollection<LotteryItem> tempNumberLotteryItems;

        private ObservableCollection<LotteryItem> defaultTextLotteryItems = new ObservableCollection<LotteryItem>{
    new LotteryItem("ぽぽぽぽーんな話"),
    new LotteryItem("震災前後で自分自身が変わったこと"),
    new LotteryItem("東日本大震災の時どこにいた？どう行動した？"),
    new LotteryItem("ドキドキした話"),
    new LotteryItem("涙した話"),
    new LotteryItem("東北、福島のために何が出来る？何か行動した？"),
    new LotteryItem("計画停電、節電政策がITに及ぼす影響")};

        private ObservableCollection<LotteryItem> _textLotteryItems;
        public ObservableCollection<LotteryItem> TextLotteryItems
        {
            get
            {
                if(_textLotteryItems == null)
                    GetRestoredTextLotteryItems();
                return _textLotteryItems;
            }
        }

        public bool IsNumberMode
        {
            get { return GetValue<bool>(false); }
            set { SetValue<bool>(value); }
        }

        public int NumberLottteryItemsCount
        {
            get { return GetValue<int>(50); }
            set { SetValue<int>(value); }
        }


        public bool IsShuffled
        {
            get { return GetValue<bool>(true); }
            set { SetValue<bool>(value); }
        }

        public void GenerateLotteryItems()
        {
            if (IsNumberMode)
            {
                tempNumberLotteryItems = new ObservableCollection<LotteryItem>();

                for (int i = 0; i < NumberLottteryItemsCount; i++)
                {
                    tempNumberLotteryItems.Add(new LotteryItem((i + 1).ToString()));
                }

                if (IsShuffled)
                    MainLotteryItems = new ObservableCollection<LotteryItem>(tempNumberLotteryItems.OrderBy(i => Guid.NewGuid()));
                else
                    MainLotteryItems = tempNumberLotteryItems;
            }
            else
                MainLotteryItems = TextLotteryItems;
        }
        
        private T GetValue<T>(T defaultValue, [System.Runtime.CompilerServices.CallerMemberName] string key = "")
        {
            if (!_settings.Values.ContainsKey(key))
                return defaultValue;

            object value = _settings.Values[key];
            return (value == null) ? default(T) : (T)value;
        }

        private void SetValue<T>(T newValue, [System.Runtime.CompilerServices.CallerMemberName] string key = "")
        {
            _settings.Values[key] = newValue;

            RaisePropertyChanged(key);            
        }

        public void SetTextLotteryItems()
        {
            string[] storedTextData = new string[_textLotteryItems.Count];
            var index = 0;
            foreach (LotteryItem item in _textLotteryItems)
            {
                storedTextData[index] = item.Text;
                index++;
            }

            var json = JsonConvert.SerializeObject(storedTextData);
            _settings.Values["storedTextData"] = json;
        }

        private void GetRestoredTextLotteryItems()
        {
            var json = (string)_settings.Values["storedTextData"];
            if (json == null)
            {
                _textLotteryItems = defaultTextLotteryItems;
            }
            else
            {
                _textLotteryItems = new ObservableCollection<LotteryItem>();
                var storedTextData = JsonConvert.DeserializeObject<string[]>(json);
                foreach (var text in storedTextData)
                {
                    _textLotteryItems.Add(new LotteryItem(text));
                }
            }
        }

    }
}
