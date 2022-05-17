using System;
using System.Collections.Generic;
using System.Windows;

namespace Server.Models
{
    public class Transform
    {
        public ScaleTransform ScaleTransform { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public Transform()
        {
            ScaleTransform = new ScaleTransform();
            Position = new Vector2();
            Size = new Vector2();
        }
    }
    public class ScaleTransform
    {
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double ScaleX { get; set; }
    }
}
