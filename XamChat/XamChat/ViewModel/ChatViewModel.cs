using MvvmHelpers;
using Xamarin.Forms;
using XamChat.Model;
using System.Threading.Tasks;
using System;
using XamChat.Helpers;
using System.Linq;

namespace XamChat.ViewModel
{
    public class ChatViewModel : ViewModelBase
    {
        public ChatMessage ChatMessage { get; }

        public ObservableRangeCollection<ChatMessage> Messages { get; }
        public ObservableRangeCollection<User> Users { get; }

        bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    SetProperty(ref isConnected, value);
                });
            }
        }
        

        public Command SendMessageCommand { get; }
        public Command ConnectCommand { get; }
        public Command DisconnectCommand { get; }

        Random random;
        public ChatViewModel()
        {
            if (DesignMode.IsDesignModeEnabled)
                return;

            Title = Settings.Group;

            ChatMessage = new ChatMessage();
            Messages = new ObservableRangeCollection<ChatMessage>();
            Users = new ObservableRangeCollection<User>();
            SendMessageCommand = new Command(async () => await SendMessage());
            ConnectCommand = new Command(async () => await Connect());
            DisconnectCommand = new Command(async () => await Disconnect());
            random = new Random();

            ChatService.Init(Settings.ServerIP, Settings.UseHttps);

            ChatService.OnReceivedMessage += (sender, args) =>
            {
                SendLocalMessage(args.Message, args.User);
                AddRemoveUser(args.User, true);
            };

            ChatService.OnEnteredOrExited += (sender, args) =>
            {
                SendLocalMessage(args.Message, args.User);

                AddRemoveUser(args.User, args.User.Contains("joined"));
            };

            ChatService.OnConnectionClosed += (sender, args) =>
            {
                SendLocalMessage(args.Message, args.User);  
            };
        }


        async Task Connect()
        {
            if (IsConnected)
                return;
            try
            {                
                await ChatService.ConnectAsync();
                await ChatService.JoinChannelAsync(Settings.Group, Settings.UserName);
                IsConnected = true;
                SendLocalMessage("Connected...", Settings.UserName);
                AddRemoveUser(Settings.UserName, true);
            }
            catch (Exception ex)
            {
                SendLocalMessage($"Connection error: {ex.Message}", Settings.UserName);
            }
        }

        async Task Disconnect()
        {
            if (!IsConnected)
                return;
            await ChatService.LeaveChannelAsync(Settings.Group, Settings.UserName);
            await ChatService.DisconnectAsync();
            IsConnected = false;
            SendLocalMessage("Disconnected...", Settings.UserName);
        }

        async Task SendMessage()
        {
            if(!IsConnected)
            {
                await DialogService.DisplayAlert("Not connected", "Please connect to the server and try again.", "OK");
                return;
            }
            try
            {
                IsBusy = true;
                await ChatService.SendMessageAsync(Settings.Group,
                    Settings.UserName,
                    ChatMessage.Message);

                ChatMessage.Message = string.Empty;
            }
            catch (Exception ex)
            {
                SendLocalMessage($"Send failed: {ex.Message}", Settings.UserName);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SendLocalMessage(string message, string user)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Messages.Insert(0, new ChatMessage
                {
                    Message = message,
                    User = user
                });
            });
        }

        void AddRemoveUser(string name, bool add)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;
            Device.BeginInvokeOnMainThread(() =>
            {
                if (add)
                {
                    if (!Users.Any(u => u.Name == name))
                        Users.Add(new User { Name = name });
                }
                else
                {
                    var user = Users.FirstOrDefault(u => u.Name == name);
                    if (user != null)
                        Users.Remove(user);
                }
            });
        }

    }
}
