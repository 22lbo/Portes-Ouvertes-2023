using Microsoft.AspNetCore.SignalR;
using DrawLib;

namespace Draw_SignalR_Hub
{


    public class DrawingHub : Hub
    {
        public DrawingHub() { }

        public int AddShape(ShapeData shape)
        {
            var s = Drawing.Add(shape);
            Clients.Others.SendAsync("ShapeAdded", s);
            return s.Id;
        }

        public void UpdateShape(ShapeData shape)
        {
            if (Drawing.Update(shape))
                Clients.Others.SendAsync("ShapeUpdated", shape);
        }

    }
}
