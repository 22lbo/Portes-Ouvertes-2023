﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDraw
{
    public class BrushStroke : Shape
    {
        public BrushStroke() { Data.Type = DrawLib.ShapeData.Types.Ellipse; }

        public override void Draw(Graphics g)
        {
            g.FillEllipse(Pen.Brush, X1, Y1, 10, 10);
        }
    }
}
