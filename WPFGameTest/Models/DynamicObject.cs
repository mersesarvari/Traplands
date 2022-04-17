using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using WPFGameTest.Helpers;

namespace WPFGameTest.Models
{
    public class DynamicObject : Entity
    {
        public float xRemainder { get; protected set; }
        public float yRemainder { get; protected set; }
        public bool FacingRight { get; private set; }
        public bool Grounded { get; protected set; }
        public int HitboxOffset { get; private set; }

        public DynamicObject(Vector2 position, Vector2 size, int hitboxOffset = 0) : base(position, size)
        {
            Transform = new Transform();
            Transform.Position = new Vector2(position.X, position.Y);
            Transform.Size = new Vector2(size.X, size.Y);

            Hitbox = new IntRect(position.X, position.Y, size.X, size.Y);
            Hitbox.X += hitboxOffset;
            Hitbox.Width -= hitboxOffset * 2;

            xRemainder = 0;
            yRemainder = 0;
            FacingRight = true;
            HitboxOffset = hitboxOffset;

            Element = new Rectangle();
            Element.Width = size.X;
            Element.Height = size.Y;
            Element.Fill = new SolidColorBrush(Colors.Red);

            Transform.ScaleTransform = new ScaleTransform();
            Element.RenderTransform = Transform.ScaleTransform;
            Element.RenderTransformOrigin = new Point(0.5, 0.5);

            Canvas.SetLeft(Element, position.X);
            Canvas.SetTop(Element, position.Y);
        }

        public static List<Entity> solids = new List<Entity>();
        public static List<Entity> interactables = new List<Entity>();

        public void SetSolids(List<Entity> solidBodies)
        {
            solids = solidBodies;
        }

        public void SetInteractables(List<Entity> interactableBodies)
        {
            interactables = interactableBodies;
        }

        public virtual void MoveX(float amount, Action onCollision)
        {
            xRemainder += amount;
            int move = (int)Math.Round(xRemainder, MidpointRounding.ToEven);

            if (move != 0)
            {
                xRemainder -= move;
                int sign = Math.Sign(move);

                while (move != 0)
                {
                    IntRect tempRect = new IntRect
                    {
                        X = Hitbox.X + sign,
                        Y = Hitbox.Y,
                        Width = Hitbox.Width,
                        Height = Hitbox.Height
                    };

                    if (!Physics.IsColliding(solids, tempRect))
                    {
                        //  We don't collide with anyting solid
                        Transform.Position.X += sign;
                        Hitbox.X += sign;
                        move -= sign;
                    }
                    else
                    {
                        // Colliding with solid
                        onCollision?.Invoke();
                        break;
                    }
                }
            }

            Canvas.SetLeft(Element, Transform.Position.X);
        }

        public virtual void MoveY(float amount, Action onCollision)
        {
            yRemainder += amount;
            int move = (int)Math.Round(yRemainder, MidpointRounding.ToEven);

            yRemainder -= move;
            int sign = Math.Sign(move);

            while (move != 0)
            {
                IntRect tempRect = new IntRect
                {
                    X = Hitbox.X,
                    Y = Hitbox.Y + sign,
                    Width = Hitbox.Width,
                    Height = Hitbox.Height
                };

                if (!Physics.IsColliding(solids, tempRect))
                {
                    //  We don't collide with anyting solid
                    Transform.Position.Y += sign;
                    Hitbox.Y += sign;
                    move -= sign;
                    Grounded = false;
                }
                else
                {
                    // Colliding with solid
                    if (sign == 1) // If we collided below us
                    {
                        Grounded = true;
                    }

                    onCollision?.Invoke();
                    break;
                }
            }

            Canvas.SetTop(Element, Transform.Position.Y);
        }

        public void Flip()
        {
            Transform.ScaleTransform.ScaleX *= -1;
            FacingRight = !FacingRight;
        }

        public void SetDefaultSprite(ImageSource imgSrc)
        {
            Element.Fill = new ImageBrush(imgSrc);
        }

        public virtual void Update(float deltaTime) { }
    }
}
