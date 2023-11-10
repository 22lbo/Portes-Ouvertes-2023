using Microsoft.AspNetCore.SignalR;
using SimpleDraw;

namespace Draw_SignalR_Hub
{


    public class DrawingHub : Hub
    {
        private static HashMap<ShapeData> Shapes = new();

    }
}
