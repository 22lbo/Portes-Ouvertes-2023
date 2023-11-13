using DrawLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDraw
{
    public abstract class Shape
    {
        public ShapeData Data = new();

        public Shape() { LineColor = Color.Black;  }

        public int Id { get => Data.Id; set => Data.Id = value; }
        public int X1 { get => Data.X1; set => Data.X1 = value; }
        public int Y1 { get => Data.Y1; set => Data.Y1 = value; }
        public int X2 { get => Data.X2; set => Data.X2 = value; }
        public int Y2 { get => Data.Y2; set => Data.Y2 = value; }
        public Color LineColor { get => Color.FromArgb(Data.LineColor); set => Data.LineColor = value.ToArgb(); }
        public Pen Pen => new(LineColor);

        public abstract void Draw(Graphics g);

    }
}
