using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDraw
{
    public class DrawingHub : Hub
    {
        private static List<string> users = new List<string>();
        public override Task OnConnected()
        {
            users.Add(Context.ConnectionId);
            return base.OnConnected();
        }
    }
}
