using Xamarin.Forms;
using XamChat.ViewModel;

namespace XamChat.View
{
    public partial class MainPage : ContentPage
    {
        LobbyViewModel vm;
        LobbyViewModel VM
        {
            get => vm ?? (vm = (LobbyViewModel)BindingContext);
        }
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ToolbarProfile.Clicked += ToolbarProfile_Clicked;
        }

        private async void ToolbarProfile_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new XamChatNavigationPage(new ProfilePage()));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ToolbarProfile.Clicked -= ToolbarProfile_Clicked;
        }

        async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await VM.GoToGroupChat(Navigation, e.SelectedItem as string);
            ((ListView)sender).SelectedItem = null;
        }
    }
}
