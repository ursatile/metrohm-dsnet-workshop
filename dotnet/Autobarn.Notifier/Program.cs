using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Autobarn.Notifier {
    class Program {
        private static IBus bus;
        private static HubConnection hub;

        static async Task Main(string[] args) {
            hub = new HubConnectionBuilder().WithUrl(Configuration["AutobarnSignalRHubUrl"]).Build();
            await hub.StartAsync();
            Console.WriteLine("Connected to SignalR hub!");
            bus = RabbitHutch.CreateBus(Configuration.GetConnectionString("RabbitMQ"));
            await bus.PubSub.SubscribeAsync<NewVehiclePriceMessage>("Autobarn.Notifier", HandleNewVehiclePriceMessage);
            Console.WriteLine("Autobarn.Notifier started; listening for NewVehiclePriceMessages...");
            Console.ReadLine();
        }

        private static async Task HandleNewVehiclePriceMessage(NewVehiclePriceMessage m) {
            var json = JsonConvert.SerializeObject(m);
            Console.WriteLine($"Sending JSON to SignalR: {json}");
            await hub.SendAsync("NotifyWebUsers", "Autobarn.Notifier", json);
            Console.WriteLine("Sent!");
        }

        static IConfigurationRoot config;

        static IConfigurationRoot Configuration {
            get {
                if (config != default) return (config);
                var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
                config = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();
                return config;
            }
        }

    }

}
