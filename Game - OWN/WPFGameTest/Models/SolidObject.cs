using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Game.Helpers;

namespace Game.Models
{
    public class SolidObject : GameObject
    {
        public bool IsGrabbable { get; private set; }

        public SolidObject(Vector2 position, Vector2 size, bool grabbable = true) : base(position, size)
        {
            IsGrabbable = grabbable;
        }

        public SolidObject(Vector2 position, Vector2 size, ImageSource image, bool grabbable = false) : this(position, size, grabbable)
        {
            Fill = new ImageBrush(image);
        }

        public SolidObject(Vector2 position, Vector2 size, Color color, bool grabbable = false) : this(position, size, grabbable)
        {
            Fill = new SolidColorBrush(color);
        }
    }
}

