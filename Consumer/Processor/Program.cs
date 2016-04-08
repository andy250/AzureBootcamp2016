using System;
using System.Configuration;
using Microsoft.ServiceBus.Messaging;

namespace AzureBootcamp.Processor
{
    class Program
    {
        public static readonly string StorageConn = ConfigurationManager.AppSettings["storageconn"];
        public static readonly string IotHubConn = ConfigurationManager.AppSettings["iothubconn"];

        static void Main(string[] args)
        {
            var host = new EventProcessorHost("iothub_" + Guid.NewGuid(), "messages/events", "cold", IotHubConn, StorageConn, "sensor-events");
            host.RegisterEventProcessorAsync<Processor>().Wait();

            Console.WriteLine("Receiving. Press enter key to stop worker.");
            Console.ReadLine();

            host.UnregisterEventProcessorAsync().Wait();
        }
    }
}
