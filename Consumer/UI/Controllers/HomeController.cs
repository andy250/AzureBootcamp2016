using System.IO;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;

namespace AzureBootcamp.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void ReceiveData()
        {
            using (var stream = new StreamReader(Request.InputStream))
            {
                string data = stream.ReadToEnd();
                GlobalHost.ConnectionManager.GetHubContext<SensorHub>().Clients.All.broadcastMessage(data);
            }
        }
    }
}