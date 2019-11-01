using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace XamChat.Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }


        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
           .ConfigureLogging(logging =>
           {
               logging.ClearProviders();
               logging.AddConsole();
           })
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.ConfigureKestrel((context, options) =>
               {
#if DEBUG
                   options.Listen(IPAddress.Loopback, 5000);
#endif
               })
               .UseStartup<Startup>();
           });
    }
}
