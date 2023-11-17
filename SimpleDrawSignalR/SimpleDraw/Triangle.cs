using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDraw
{
    internal class Triangle : Shape
    {
        public Triangle() { Data.Type = DrawLib.ShapeData.Types.Line; }

        public override void Draw(Graphics g)
        {

            //g.DrawRectangle(Pen, Math.Min(X1, X2), Math.Min(Y1, Y2), Math.Abs(X2 - X1), Math.Abs(Y2 - Y1));

            Point point1 = new Point(X1, Y1);
            Point point2 = new Point(Y1, X2);
            Point point3 = new Point(X2, Y2);
            Point[] points = { point1, point2, point3 };

            g.DrawPolygon(Pen, points);
        }
    }

}
