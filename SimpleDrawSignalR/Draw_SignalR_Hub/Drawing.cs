﻿using DrawLib;
using System.Security.Cryptography.Xml;

namespace Draw_SignalR_Hub
{
    public static class Drawing
    {
        public static Dictionary<int, ShapeData> Shapes { get; set; } = new();
        static int LastId = 0;

        public static ShapeData Add(ShapeData shape)
        {
            shape.Id = ++LastId;
            Shapes.Add(shape.Id, shape);
            return shape;
        }

        public static ShapeData Remove(ShapeData shape)
        {
            Shapes.Remove(shape.Id, out shape);
            return shape;
        }

        public static bool Update(ShapeData shape)
        {
            if (shape.Id == 0)
                return false;
            try
            {
                Shapes[shape.Id] = shape;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
            return true;
        }
    }
}
