using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 設定フライアウトの項目テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=273769 を参照してください

namespace FstyleLottery
{
    public sealed partial class AboutFlyout : SettingsFlyout
    {
        public AboutFlyout()
        {
            this.InitializeComponent();
        }

        private void SettingsFlyout_Loaded(object sender, RoutedEventArgs e)
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            textBlockVersion.Text = " " + string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }
    }
}
