using System;
using System.Collections.Generic;
using System.Windows.Media;
using Game.Helpers;

namespace Game.Models
{
    public class DynamicObject : GameObject
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

            Fill = new SolidColorBrush(Colors.Red);

            Transform.ScaleTransform = new ScaleTransform();
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
                    Transform.Position.X += sign;
                    Hitbox.X += sign;
                    move -= sign;
                }
            }
        }

        public virtual void MoveY(float amount, Action onCollision)
        {
            yRemainder += amount;
            int move = (int)Math.Round(yRemainder, MidpointRounding.ToEven);

            yRemainder -= move;
            int sign = Math.Sign(move);

            while (move != 0)
            {
                Transform.Position.Y += sign;
                Hitbox.Y += sign;
                move -= sign;
            }
        }

        public void Flip()
        {
            Transform.ScaleTransform.ScaleX *= -1;
            FacingRight = !FacingRight;
        }
    }
}
