using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDraw
{
    internal class Triangle : Shape
    {
        public Triangle() { Data.Type = DrawLib.ShapeData.Types.Triangle; }

        public override void Draw(Graphics g)
        {
            Point point1 = new Point(X1, Y1);
            Point point2 = new Point(X2, Y2);
            Point point3 = new Point(X1, Y2);
            Point[] points = { point1, point2, point3 };

            g.FillPolygon(Brush, points);
            g.DrawPolygon(Pen, points);
        }
    }

}
