namespace DrawLib
{
    public class ShapeData
    {
        public enum Types { None, Line, Rectangle, Ellipse, Triangle, BrushStroke };
        public int Id { get; set; }
        public Types Type { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public int SZ { get; set; }
        public int LineColor { get; set; }
        public int FillColor { get; set; }
    }
}
