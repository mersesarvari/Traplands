using System.Windows.Media;
using Game.Helpers;

namespace Game.Models
{
    public abstract class GameObject
    {
        public string Tag { get; set; }
        public Brush Fill { get; set; }
        public Transform Transform { get; set; }
        public IntRect Hitbox { get; set; }

        public GameObject(Vector2 position, Vector2 size)
        {
            Tag = "Default";

            Fill = new SolidColorBrush(Colors.Thistle);

            Transform = new Transform();
            Transform.Position = new Vector2(position.X, position.Y);
            Transform.Size = new Vector2(size.X, size.Y); ;

            Hitbox = new IntRect(position.X, position.Y, size.X, size.Y);
        }

        public virtual void Update(float deltaTime)
        {
            Transform.ScaleTransform.CenterX = Transform.Position.X + Transform.Size.X * 0.5f;
            Transform.ScaleTransform.CenterY = Transform.Position.Y + Transform.Size.Y * 0.5f;
        }
    }
}