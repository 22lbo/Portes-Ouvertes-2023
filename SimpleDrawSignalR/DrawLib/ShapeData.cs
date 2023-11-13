﻿namespace DrawLib
{
    public  class ShapeData
    {
        public enum Types { None, Line, Rectangle, Ellipse};
        public int Id { get; set; }
        public Types Type { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public int LineColor { get; set; }


    }
}
