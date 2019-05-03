using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamChat.Core;
using XamChat.View;
using Xamarin.Essentials;
using XamChat.Helpers;
using Microsoft.AppCenter.Distribute;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamChat
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<ChatService>();

            MainPage = new HomePage();
        }

        protected override void OnStart()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android && Settings.AppCenterAndroid != "AC_ANDROID")
            {
                AppCenter.Start($"android={Settings.AppCenterAndroid};" +
                    "uwp={Your UWP App secret here};" +
                    "ios={Your iOS App secret here}",
                    typeof(Analytics), typeof(Crashes), typeof(Distribute));
            }
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
