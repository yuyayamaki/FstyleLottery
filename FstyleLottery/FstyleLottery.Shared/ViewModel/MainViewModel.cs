using FstyleLottery.DataModel;
using FstyleLottery.Helper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace FstyleLottery.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private DispatcherTimer rouletteDispatcherTimer;
        private DispatcherTimer changeIntervalDispatcherTimer;
        
        private WavePlayer wavePlayer = new WavePlayer();
        private CanStopWavePlayer rouletteMusicPlayer = new CanStopWavePlayer("ms-appx:///SoundFiles/lo_040.wav");

        public MainViewModel()
        {
            rouletteDispatcherTimer = new DispatcherTimer();
            rouletteDispatcherTimer.Tick += rouletteDispatcherTimer_Tick;

            changeIntervalDispatcherTimer = new DispatcherTimer();
            changeIntervalDispatcherTimer.Tick += changeIntervalDispatcherTimer_Tick;
            changeIntervalDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            StartButtonVisibility = Visibility.Visible;

            lotteryModel = App.Model;
            lotteryModel.SetTextLotteryItems();
            lotteryModel.GenerateLotteryItems();

            this.lotteryViewItems.AddRange(GetStringListFromLotteryItems(lotteryModel.MainLotteryItems).ToArray());
            if(lotteryModel.MainLotteryItems.Count <= 6)
            {
                switch (lotteryModel.MainLotteryItems.Count)
	            {
                    case 2:
                        for (int i = 0; i < 2; i++)
                        {
                            lotteryViewItems.Add(lotteryModel.MainLotteryItems[0].Text);
                            lotteryViewItems.Add(lotteryModel.MainLotteryItems[1].Text);
                        }
                        lotteryViewItems.Add(lotteryModel.MainLotteryItems[0].Text);
                        break;
                    case 3:
                        lotteryViewItems.Add(lotteryModel.MainLotteryItems[0].Text);
                        lotteryViewItems.Add(lotteryModel.MainLotteryItems[1].Text);
                        lotteryViewItems.Add(lotteryModel.MainLotteryItems[2].Text);
                        lotteryViewItems.Add(lotteryModel.MainLotteryItems[0].Text);
                        break;
                    case 4:
                        lotteryViewItems.Add(lotteryModel.MainLotteryItems[0].Text);
                        lotteryViewItems.Add(lotteryModel.MainLotteryItems[1].Text);
                        lotteryViewItems.Add(lotteryModel.MainLotteryItems[2].Text);
                        break;
                    case 5:
                        lotteryViewItems.Add(lotteryModel.MainLotteryItems[0].Text);
                        lotteryViewItems.Add(lotteryModel.MainLotteryItems[1].Text);
                        break;
                    case 6:
                        lotteryViewItems.Add(lotteryModel.MainLotteryItems[0].Text);
                        break;
	            }

            }
            

            this.setInitialItems();
        }

        private List<string> GetStringListFromLotteryItems(ObservableCollection<LotteryItem> observableCollection)
        {
            var tempList = new List<string>();
            foreach (var item in observableCollection)
	        {
                tempList.Add(item.Text);
	        }
            return tempList;
        }
    
        private LotteryModel lotteryModel;

        private List<string> lotteryViewItems = new List<string>();

        private int currentFirstItemIndex = 6;

        private string _text1;
        private string _text2;
        private string _text3;
        private string _text4;
        private string _text5;
        private string _text6;
        private string _text7;
        public string Text1
        {
            get { return _text1; }
            set
            {
                Set(ref _text1, value);
            }
        }

        public string Text2
        {
            get { return _text2; }
            set
            {
                Set(ref _text2, value);
            }
        }

        public string Text3
        {
            get { return _text3; }
            set
            {
                Set(ref _text3, value);
            }
        }

        public string Text4
        {
            get { return _text4; }
            set
            {
                Set(ref _text4, value);
            }
        }

        public string Text5
        {
            get { return _text5; }
            set
            {
                Set(ref _text5, value);
            }
        }

        public string Text6
        {
            get { return _text6; }
            set
            {
                Set(ref _text6, value);
            }
        }

        public string Text7
        {
            get { return _text7; }
            set
            {
                Set(ref _text7, value);
            }
        }

        private Visibility _startButtonVisibility;
        public Visibility StartButtonVisibility
        {
            get { return _startButtonVisibility; }
            set
            {
                Set(ref _startButtonVisibility, value);
                RaisePropertyChanged(() => StopButtonVisibility);
            }
        }
        public Visibility StopButtonVisibility
        {
            get
            {
                return StartButtonVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private int currentIntervalSeconds;

        private RelayCommand _startCommand;
        private RelayCommand _stopCommand;
  
        private bool _canExcuteStartCommand = true;
        private bool _canExcuteStopCommand = false;

        public bool CanExcuteStartCommand
        {
            get
            {
                return _canExcuteStartCommand;
            }
            set
            {
                Set(ref _canExcuteStartCommand, value);
                StartCommand.RaiseCanExecuteChanged();
            }
        }
        public bool CanExcuteStopCommand
        {
            get
            {
                return _canExcuteStopCommand;
            }
            set
            {
                Set(ref _canExcuteStopCommand, value);
                StopCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets the StartCommand.
        /// </summary>
        public RelayCommand StartCommand
        {
            get
            {
                return _startCommand 
                    ?? (_startCommand = new RelayCommand(
                                          () =>
                                          {
                                              rouletteMusicPlayer.Play();
                                              
                                              StartButtonVisibility = Visibility.Collapsed;
                                              CanExcuteStopCommand = false;
                                              CanExcuteStartCommand = false;

                                              currentIntervalSeconds = 1000;
                                              rouletteDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, currentIntervalSeconds);
                                              changeIntervalDispatcherTimer.Start();
                                              rouletteDispatcherTimer.Start();
                                          },
                                          () => CanExcuteStartCommand));
            }
        }

        /// <summary>
        /// Gets the StopCommand.
        /// </summary>
        public RelayCommand StopCommand
        {
            get
            {
                return _stopCommand
                    ?? (_stopCommand = new RelayCommand(
                                          () =>
                                          {
                                              StartButtonVisibility = Visibility.Visible;
                                              isStopButtonClicked = true;
                                          },
                                          () => CanExcuteStopCommand));
            }
        }

        private bool isStopButtonClicked = false;

        private void rouletteDispatcherTimer_Tick(object sender, object e)
        {
            shiftText();

            this.Play("ms-appx:///SoundFiles/b_001.wav");
        }

        private void changeIntervalDispatcherTimer_Tick(object sender, object e)
        {
            if (!isStopButtonClicked)
            {
                if (currentIntervalSeconds > 20)
                {
                    currentIntervalSeconds -= 20;
                    rouletteDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, currentIntervalSeconds);
                }
                else
                {
                    CanExcuteStopCommand = true;
                }
            }
            else
            {
                if (currentIntervalSeconds < 1000)
                {
                    currentIntervalSeconds += 20;
                    rouletteDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, currentIntervalSeconds);
                }
                else
                {
                    rouletteDispatcherTimer.Stop();
                    ((DispatcherTimer)sender).Stop();
                    isStopButtonClicked = false;
                    CanExcuteStartCommand = true;

                    Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));

                    rouletteMusicPlayer.Stop();
                    this.Play("ms-appx:///SoundFiles/ji_017.wav");
                }
            }
        }

        private void shiftText()
        {
            Text1 = _text2;
            Text2 = _text3;
            Text3 = _text4;
            Text4 = _text5;
            Text5 = _text6;
            Text6 = _text7;
            
            if(lotteryModel.MainLotteryItems.Count >= 7)
                Text7 = lotteryViewItems[currentFirstItemIndex];
            else
            {
                switch (lotteryModel.MainLotteryItems.Count)
	            {
                    case 6:
                        if(currentFirstItemIndex + 1 > 6)
                            currentFirstItemIndex-=6;
                        Text7 = lotteryViewItems[currentFirstItemIndex + 1];
                        break;
                    case 5:
                        if (currentFirstItemIndex + 1 > 5)
                            currentFirstItemIndex -= 5;
                        Text7 = lotteryViewItems[currentFirstItemIndex + 2];
                        break;
                    case 4:
                        if (currentFirstItemIndex + 1 > 4)
                            currentFirstItemIndex -= 4;
                        Text7 = lotteryViewItems[currentFirstItemIndex];
                        break;
                    case 3:
                        while (currentFirstItemIndex + 1 > 3)
	                    {
                            currentFirstItemIndex -= 3;        
	                    }
                        Text7 = lotteryViewItems[currentFirstItemIndex];
                        break;
                    case 2:
                        while (currentFirstItemIndex + 1 > 2)
                        {
                            currentFirstItemIndex -= 2;
                        }
                        Text7 = lotteryViewItems[currentFirstItemIndex];
                        break;
                }
            }

            if (currentFirstItemIndex == lotteryModel.MainLotteryItems.Count - 1)
            {
                currentFirstItemIndex = 0;
            }
            else
            {
                currentFirstItemIndex++;
            }
        }

        private void setInitialItems()
        {
            Text1 = lotteryViewItems[0];
            Text2 = lotteryViewItems[1];
            Text3 = lotteryViewItems[2];
            Text4 = lotteryViewItems[3];
            Text5 = lotteryViewItems[4];
            Text6 = lotteryViewItems[5];
            Text7 = lotteryViewItems[6];
        }

        private async void Play(string soundFileUrl)
        {
            var file = await Windows.Storage.StorageFile
                              .GetFileFromApplicationUriAsync(new Uri(soundFileUrl));
            wavePlayer.StartPlay(file);
        }
    }
}
