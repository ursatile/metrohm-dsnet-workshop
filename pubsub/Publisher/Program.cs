using System;
using System.Threading;
using EasyNetQ;
using Messages;

namespace Publisher {
    class Program {
        const string AMQP = "amqps://lmsunmkw:fiOkDVyorwTj0Yt5M44pBDCgNvWzJ1C1@squid.rmq.cloudamqp.com/lmsunmkw";
        static void Main(string[] args) {
            using var bus = RabbitHutch.CreateBus(AMQP);
            var count = 0;
            while (true) {
                // Console.WriteLine("Press a key to publish a message:");
                // Console.ReadKey();
                var body = $"Message {count} from {Environment.MachineName}";
                bus.PubSub.Publish(new Message {
                    Body = body
                });
                Console.WriteLine($"Published: {body}");
                count++;
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}
