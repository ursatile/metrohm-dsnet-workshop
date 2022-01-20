using System;
using System.IO;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Autobarn.Messages;

namespace Autobarn.AuditLog {
    class Program {

        const string SUBSCRIPTION_ID = "Autobarn.AuditLog";

        static void Main(string[] args) {
            using var bus = RabbitHutch.CreateBus(Configuration.GetConnectionString("RabbitMQ"));
            bus.PubSub.Subscribe<NewVehicleMessage>(SUBSCRIPTION_ID, HandleNewVehicleMessage);
            Console.WriteLine("Autobarn.AuditLog started - listening for NewVehicleMessages (press Enter to quit)");
            Console.ReadLine();
        }

        static void HandleNewVehicleMessage(NewVehicleMessage m) {
            var csvRow = $"{m.Registration},{m.ManufacturerName},{m.ModelName},{m.Color},{m.Year},{m.ListedAt:O}";
            Console.WriteLine(csvRow);
            File.AppendAllText("vehicles_log.csv", csvRow + Environment.NewLine);
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
