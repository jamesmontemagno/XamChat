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

        
        async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await VM.GoToGroupChat(Navigation, e.SelectedItem as string);
            ((ListView)sender).SelectedItem = null;
        }
    }
}
