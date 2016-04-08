using Microsoft.AspNet.SignalR;

namespace AzureBootcamp.UI.Controllers
{
    public class SensorHub : Hub
    {
        public void Send(string data)
        {
            Clients.All.broadcastMessage(data);
        }
    }
}