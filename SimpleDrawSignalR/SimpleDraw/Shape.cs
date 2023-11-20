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

        public Shape() { FillColor = Color.White; LineColor = Color.Black; }

        public const int minSelectDist = 2;

        public int Id { get => Data.Id; set => Data.Id = value; }
        public int X1 { get => Data.X1; set => Data.X1 = value; }
        public int Y1 { get => Data.Y1; set => Data.Y1 = value; }
        public int X2 { get => Data.X2; set => Data.X2 = value; }
        public int Y2 { get => Data.Y2; set => Data.Y2 = value; }
        public int SZ { get => Data.SZ; set => Data.SZ = value; }
        public Color LineColor { get => Color.FromArgb(Data.LineColor); set => Data.LineColor = value.ToArgb(); }
        public Color FillColor { get => Color.FromArgb(Data.FillColor); set => Data.FillColor = value.ToArgb(); }
        public Pen Pen => new(LineColor);
        public Brush Brush => new SolidBrush(FillColor);

        public int X { get => Math.Min(X1, X2); }
        public int Y { get => Math.Min(Y1, Y2); }
        public int Width { get => Math.Abs(X2 - X1); }
        public int Height { get => Math.Abs(Y2 - Y1); }

        public int SelectionDistance => Math.Max((int)(Pen.Width + 1) / 2, minSelectDist);

        public virtual bool IsHit(int x, int y)
        {
            int rx = X - SelectionDistance;
            int ry = Y - SelectionDistance;
            int rwith = Width + 2 * SelectionDistance;
            int rheight = Height + 2 * SelectionDistance;
            var rect = new System.Drawing.Rectangle(rx, ry, rwith, rheight);
            return rect.Contains(x, y);
        }

        public abstract void Draw(Graphics g);

    }
}
