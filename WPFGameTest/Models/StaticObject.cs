using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFGameTest.Helpers;

namespace WPFGameTest.Models
{
    public class StaticObject
    {
        public Rectangle Element { get; set; }
        public Transform Transform { get; set; }
        public IntRect Hitbox { get; set; }
        public bool IsGrabbable { get; private set; }

        public StaticObject(Vector2 position, Vector2 size, bool grabbable = false)
        {
            Transform = new Transform();
            Transform.Position = position;
            Transform.Size = size;
            IsGrabbable = grabbable;

            Element = new Rectangle();
            Element.Width = size.X;
            Element.Height = size.Y;
            Element.Fill = new SolidColorBrush(Colors.Thistle);

            Hitbox = new IntRect(position.X, position.Y, size.X, size.Y);

            Canvas.SetLeft(Element, position.X);
            Canvas.SetTop(Element, position.Y);
        }

        public StaticObject(Vector2 position, Vector2 size, ImageSource image, bool grabbable = false) : this(position, size, grabbable)
        {
            Element.Fill = new ImageBrush(image);
        }

        public StaticObject(Vector2 position, Vector2 size, Color color, bool grabbable = false) : this(position, size, grabbable)
        {
            Element.Fill = new SolidColorBrush(color);
        }
    }
}

