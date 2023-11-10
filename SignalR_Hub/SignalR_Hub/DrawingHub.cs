using Microsoft.AspNetCore.SignalR;

namespace SignalR_Hub
{
    public class DrawingHub : Hub
    {
        public async Task SendMessage(string user, string message) => await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
