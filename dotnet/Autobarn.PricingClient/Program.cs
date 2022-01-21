using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using Autobarn.PricingEngine;
using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Autobarn.PricingClient {
    class Program {
        private static IBus bus;
        private static Pricer.PricerClient grpcClient;

        static async Task Main(string[] args) {
            bus = RabbitHutch.CreateBus(Configuration.GetConnectionString("RabbitMQ"));
            var channel = GrpcChannel.ForAddress(Configuration["AutobarnPricingServerUrl"]);
            grpcClient = new Pricer.PricerClient(channel);
            const string SUBSCRIPTION_ID = "Autobarn.PricingClient";
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>(SUBSCRIPTION_ID, HandleNewVehicleMessage);
            Console.WriteLine("Autobarn.PricingClient has started - waiting for messages...");
            Console.ReadLine();
        }

        private static async Task HandleNewVehicleMessage(NewVehicleMessage m) {
            Console.WriteLine($"Received NewVehicleMessage: {m.ModelName}, {m.ManufacturerName} ({m.Color}, {m.Year})");
            var priceRequest = new PriceRequest {
                Color = m.Color,
                ManufacturerName = m.ManufacturerName,
                ModelName = m.ModelName,
                Year = m.Year
            };
            var priceReply = await grpcClient.GetPriceAsync(priceRequest);
            Console.WriteLine($"Calculated price: {priceReply.Price} {priceReply.CurrencyCode}");
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
