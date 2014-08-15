using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using FstyleLottery.Common;
using FstyleLottery.DataModel;
#if WINDOWS_APP
using Windows.UI.ApplicationSettings;
#endif

// ユニバーサル ハブ アプリケーション プロジェクト テンプレートについては、http://go.microsoft.com/fwlink/?LinkID=391955 を参照してください

namespace FstyleLottery
{
    /// <summary>
    /// 既定の Application クラスに対してアプリケーション独自の動作を実装します。
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        private static LotteryModel _model;

        /// <summary>
        /// <see cref="App"/> クラスの単一インスタンスを初期化します。これは、実行される作成したコードの
        /// 最初の行であり、main() または WinMain() と論理的に等価です。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        /// 静的 Model です。
        /// </summary>
        /// <returns>Letters オブジェクトです。</returns>
        public static LotteryModel Model
        {
            get
            {
                // 必要になるまでモデルの作成を延期します
                if (_model == null)
                    _model = new LotteryModel();

                return _model;
            }
        }

        /// <summary>
        /// アプリケーションがエンド ユーザーによって正常に起動されたときに呼び出されます。他のエントリ ポイントは、
        /// アプリケーションが特定のファイルを開くために呼び出されたときに
        /// 検索結果やその他の情報を表示するために使用されます。
        /// </summary>
        /// <param name="e">起動要求とプロセスの詳細を表示します。</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // ウィンドウに既にコンテンツが表示されている場合は、アプリケーションの初期化を繰り返さずに、
            // ウィンドウがアクティブであることだけを確認してください
            if (rootFrame == null)
            {
                // ナビゲーション コンテキストとして動作するフレームを作成し、最初のページに移動します
                rootFrame = new Frame();

                //フレームを SuspensionManager キーに関連付けます                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                // TODO: この値をアプリケーションに適切なキャッシュ サイズに変更します
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // 必要な場合のみ、保存されたセッション状態を復元します
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        // 状態の復元に何か問題があります。
                        // 状態がないものとして続行します
                    }
                }

                // フレームを現在のウィンドウに配置します
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // スタートアップのターンスタイル ナビゲーションを削除します。
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                // ナビゲーションの履歴スタックが復元されていない場合、最初のページに移動します。
                // このとき、必要な情報をナビゲーション パラメーターとして渡して、新しいページを
                // 作成します
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // 現在のウィンドウがアクティブであることを確認します
            Window.Current.Activate();
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// アプリを起動した後のコンテンツの移行を復元します。
        /// </summary>
        /// <param name="sender">ハンドラーがアタッチされたオブジェクト。</param>
        /// <param name="e">ナビゲーション イベントの詳細。</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        /// <summary>
        /// アプリケーションの実行が中断されたときに呼び出されます。アプリケーションの状態は、
        /// アプリケーションが終了されるのか、メモリの内容がそのままで再開されるのか
        /// わからない状態で保存されます。
        /// </summary>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

#if WINDOWS_APP
        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
        }

        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var resourceLoader = new Windows.ApplicationModel.Resources.ResourceLoader();

            args.Request.ApplicationCommands.Add(new SettingsCommand(
                "AboutId", resourceLoader.GetString("AboutString"), (handler) => ShowAboutFlyout()));
        }

        public void ShowAboutFlyout()
        {
            var aboutFlyout = new AboutFlyout();
            aboutFlyout.Show();
        }
#endif
    }
}