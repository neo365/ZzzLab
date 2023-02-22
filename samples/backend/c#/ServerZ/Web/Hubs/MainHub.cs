using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ZzzLab.Web.Hubs
{
    internal class MainHub : Hub
    {
        public static IHubContext<MainHub>? Current { get; set; }

        public MainHub() : base()
        {
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public static async Task SerialExDataReceived(string portName, string message)
        {
            if (Current != null)
            {
                await Current.Clients.All.SendAsync("SerialExDataReceived", portName, message);
            }
        }
    }
}