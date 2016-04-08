using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureBootcamp.Processor
{
    public class Processor : IEventProcessor
    {
        protected readonly Stopwatch Stopwatch;
        private static readonly TimeSpan MaxCheckpointTime = TimeSpan.FromSeconds(30);

        public Processor()
        {
            Stopwatch = new Stopwatch();
        }

        public virtual Task OpenAsync(PartitionContext context)
        {
            Stopwatch.Restart();
            return Task.FromResult<object>(null);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (EventData eventData in messages)
            {
                string data = Encoding.UTF8.GetString(eventData.GetBytes());
                Console.WriteLine("Message received.  Partition: '{0}', Data: '{1}'", context.Lease.PartitionId, data);

                using (var wc = new WebClient())
                {
                    wc.UploadString(new Uri(ConfigurationManager.AppSettings["PushURL"], UriKind.Absolute), data);
                }
            }

            await TryCheckpoint(context);
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }

        protected async Task TryCheckpoint(PartitionContext context)
        {
            if (Stopwatch.Elapsed > MaxCheckpointTime)
            {
                await context.CheckpointAsync();
                Stopwatch.Restart();
            }
        }
    }
}
