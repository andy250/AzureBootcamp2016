using System;
using System.Configuration;
using System.Text;
using System.Timers;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using TransportType = Microsoft.Azure.Devices.Client.TransportType;

namespace DeviceSim
{
    public class Program
    {
        private static DeviceClient client;
        private static double temp;
        private static double pressure;
        private static readonly Random Rnd = new Random();

        static void Main(string[] args)
        {
            client = DeviceClient.Create(ConfigurationManager.AppSettings["Hub"], 
                new DeviceAuthenticationWithRegistrySymmetricKey(ConfigurationManager.AppSettings["DeviceId"], ConfigurationManager.AppSettings["DeviceKey"]), TransportType.Amqp);

            temp = 15 + 10 * Rnd.NextDouble();
            pressure = 995 + 10 * Rnd.NextDouble();

            var t = new Timer(2000);
            t.Elapsed += Elapsed;
            t.Start();

            Console.WriteLine("Press ENTER to stop");
            Console.ReadLine();

            t.Stop();
            client.CloseAsync();
        }

        private static void Elapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            temp += (Rnd.NextDouble() - 0.5) * 0.5;
            pressure += (Rnd.NextDouble() - 0.5);

            Console.WriteLine($"Sending {temp} / {pressure}");

            var data = JsonConvert.SerializeObject(new { message = "Hello from Windows Sim!", temp, pressure, epoch = Epoch() });
            client.SendEventAsync(new Message(Encoding.UTF8.GetBytes(data))).Wait();
        }

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static long Epoch()
        {
            return Convert.ToInt64((DateTime.UtcNow - epoch).TotalSeconds);
        }
    }
}
