using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDraw
{
    public abstract class Shape
    {
        private Pen pen = new(Color.Black);

        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public Color LineColor { get => pen.Color; set => pen.Color = value; }
        public Pen Pen => pen;

        public abstract void Draw(Graphics g);

    }
}
