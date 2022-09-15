using Microsoft.AspNetCore.SignalR;

namespace ZzzLab.AspCore.Hubs
{
    internal class NotifyHub : Hub
    {
        public static IHubContext<NotifyHub>? Current { get; set; }

        public NotifyHub() : base()
        {
        }

        public async Task SendMessageAsync(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public void SendMessage(string user, string message)
            => Task.Run(async () => await SendMessageAsync(user, message));

        public static async Task DebugMessageAsync(string message)
        {
            if (Current != null)
            {
                await Current.Clients.All.SendAsync("DebugMessage", message);
            }
        }

        public static void DebugMessage(string message)
            => Task.Run(async () => await DebugMessageAsync(message));
    }
}