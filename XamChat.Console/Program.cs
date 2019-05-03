using System;
using System.Linq;
using System.Threading.Tasks;
using XamChat.Core;
using XamChat.Core.EventHandlers;

namespace XamChat.ConsoleApp
{
    public class Program
    {
        static ChatService service;
        public static async Task Main(string[] args)
        {
            service = new ChatService();
            service.OnReceivedMessage += Service_OnReceivedMessage;
            Console.WriteLine("Enter IP:");
            var ip = Console.ReadLine();
            service.Init(ip, ip == "localhost");

            await service.ConnectAsync();
            Console.WriteLine("You are connected...");
            Console.WriteLine($"Enter room ({string.Join(",", service.GetRooms())}):");
            var room = Console.ReadLine();

            await service.JoinChannelAsync(room, "console app");

            var keepGoing = true;
            do
            {
                var text = Console.ReadLine();
                if (text == "exit")
                {
                    keepGoing = false;
                }
                else
                {
                    await service.SendMessageAsync(room, "console app", text);
                }
            }
            while (keepGoing);
        }



        private static void Service_OnReceivedMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
