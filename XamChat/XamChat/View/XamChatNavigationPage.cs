using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamChat.View
{
    public class XamChatNavigationPage : NavigationPage
    {
        public XamChatNavigationPage(Page page) : base(page)
        {

        }

        public XamChatNavigationPage() : base()
        {

        }

        void SetColor()
        {
            BarBackgroundColor = (Color)Application.Current.Resources["PrimaryColor"];
            BarTextColor = (Color)Application.Current.Resources["PrimaryTextColor"];
        }
    }
}
