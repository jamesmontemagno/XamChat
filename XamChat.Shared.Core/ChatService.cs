using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using XamChat.Core.EventHandlers;

namespace XamChat.Core
{
    public class ChatService
    {
        public event EventHandler<MessageEventArgs> OnReceivedMessage;
        public event EventHandler<MessageEventArgs> OnEnteredOrExited;
        public event EventHandler<MessageEventArgs> OnConnectionClosed;

        HubConnection hubConnection;
        Random random;

        bool IsConnected { get; set; }
        Dictionary<string, string> ActiveChannels { get; } = new Dictionary<string, string>();


        public void Init(string urlRoot, bool useHttps)
        {
            random = new Random();
            var url = $"http{(useHttps ? "s" : string.Empty)}://{urlRoot}/hubs/chat";
            hubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .Build();

           hubConnection.Closed += async (error) =>
            {
                OnConnectionClosed?.Invoke(this, new MessageEventArgs("Connection closed...", string.Empty));
                IsConnected = false;
                await Task.Delay(random.Next(0, 5) * 1000);
                try
                {
                    await ConnectAsync();
                }
                catch (Exception ex)
                {
                    // Exception!
                    Debug.WriteLine(ex);
                }
            };

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                OnReceivedMessage?.Invoke(this, new MessageEventArgs(message, user));
            });

            hubConnection.On<string>("Entered", (user) =>
            {
                OnEnteredOrExited?.Invoke(this, new MessageEventArgs($"{user} entered.", user));
            });


            hubConnection.On<string>("Left", (user) =>
            {
                OnEnteredOrExited?.Invoke(this, new MessageEventArgs($"{user} left.", user));
            });


            hubConnection.On<string>("EnteredOrLeft", (message) =>
            {
                OnEnteredOrExited?.Invoke(this, new MessageEventArgs(message, message));                
            });
        }

        public async Task ConnectAsync()
        {
            if (IsConnected)
                return;

            await hubConnection.StartAsync();
            IsConnected = true;
        }

        public async Task DisconnectAsync()
        {
            if (!IsConnected)
                return;

            try
            {
                //await hubConnection.StopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            ActiveChannels.Clear();
            IsConnected = false;
        }

        public async Task LeaveChannelAsync(string group, string userName)
        {
            if (!IsConnected || !ActiveChannels.ContainsKey(group))
                return;
          
            await hubConnection.SendAsync("RemoveFromGroup", group, userName);

            ActiveChannels.Remove(group);
        }

        public async Task JoinChannelAsync(string group, string userName)
        {
            if (!IsConnected || ActiveChannels.ContainsKey(group))
                return;
        
            await hubConnection.SendAsync("AddToGroup", group, userName);
            ActiveChannels.Add(group, userName);

        }

        public async Task SendMessageAsync(string group, string userName, string message)
        {
            if (!IsConnected)
                throw new InvalidOperationException("Not connected");

            await hubConnection.InvokeAsync("SendMessageGroup",
                    group,
                    userName,
                    message);
        }

        public List<string> GetRooms()
        {
            return new List<string>
                        {
                                ".NET",
                                "ASP.NET",
                                "Xamarin"
                        };
        }
    }
}
