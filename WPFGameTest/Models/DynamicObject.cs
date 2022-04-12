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
    public class DynamicObject
    {
        public Transform Transform { get; set; }
        public Rectangle Element { get; set; }
        public float xRemainder { get; protected set; }
        public float yRemainder { get; protected set; }
        public bool FacingRight { get; private set; }
        public bool Grounded { get; protected set; }
        public int HitboxOffset { get; private set; }

        public DynamicObject(Vector2 position, Vector2 size, int hitboxOffset = 0)
        {
            Transform = new Transform();
            Transform.Position = new Vector2(position.X, position.Y);
            Transform.Size = new Vector2(size.X, size.Y); ;
            Transform.Position.X -= hitboxOffset;
            Transform.Size.X -= hitboxOffset * 2;

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

        public static List<StaticObject> solids = new List<StaticObject>();

        public void SetSolids(List<StaticObject> solidBodies)
        {
            solids = solidBodies;
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
                        X = Transform.Position.X + sign,
                        Y = Transform.Position.Y,
                        Width = Transform.Size.X,
                        Height = Transform.Size.Y
                    };

                    if (!Physics.IsColliding(solids, tempRect))
                    {
                        //  We don't collide with anyting solid
                        Transform.Position.X += sign;
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

            Canvas.SetLeft(Element, Transform.Position.X - HitboxOffset);
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
                    X = Transform.Position.X,
                    Y = Transform.Position.Y + sign,
                    Width = Transform.Size.X,
                    Height = Transform.Size.Y
                };

                if (!Physics.IsColliding(solids, tempRect))
                {
                    //  We don't collide with anyting solid
                    Transform.Position.Y += sign;
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
