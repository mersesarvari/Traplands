using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Game.Helpers;

namespace Game.Models
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
}
