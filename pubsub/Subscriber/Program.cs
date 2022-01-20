using System;
using EasyNetQ;
using Messages;

namespace Subscriber {
    class Program {
        const string AMQP = "amqps://lmsunmkw:fiOkDVyorwTj0Yt5M44pBDCgNvWzJ1C1@squid.rmq.cloudamqp.com/lmsunmkw";
        static void Main(string[] args) {
            using var bus = RabbitHutch.CreateBus(AMQP);
            const string SUBSCRIPTION_ID = "EXAMPLE_SUBSCRIPTION";
            bus.PubSub.Subscribe<Message>(SUBSCRIPTION_ID, HandleMessage);
            Console.WriteLine("Connected to bus. Listening for messages. Press Enter to quit.");
            Console.ReadLine();
        }

        static void HandleMessage(Message m) {
            // if (m.Body.Contains("5 from")) throw new Exception("Simulated exception");
            Console.WriteLine(m.Body);
        }
    }
}
