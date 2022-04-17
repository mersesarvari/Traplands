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
    public abstract class Entity
    {
        public string Tag { get; set; }
        public Rectangle Element { get; set; }
        public Transform Transform { get; set; }
        public IntRect Hitbox { get; set; }

        public Entity(Vector2 position, Vector2 size)
        {
            Tag = "Default";

            Element = new Rectangle();
            Element.Width = size.X;
            Element.Height = size.Y;
            Element.Fill = new SolidColorBrush(Colors.Thistle);

            Transform = new Transform();
            Transform.Position = new Vector2(position.X, position.Y);
            Transform.Size = new Vector2(size.X, size.Y); ;

            Hitbox = new IntRect(position.X, position.Y, size.X, size.Y);

            Canvas.SetLeft(Element, position.X);
            Canvas.SetTop(Element, position.Y);
        }
    }
}
