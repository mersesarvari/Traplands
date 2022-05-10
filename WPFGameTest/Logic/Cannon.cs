using Game.Helpers;
using Game.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Game.Logic
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public class Cannon : SolidObject
    {
        public class Bullet : DynamicObject
        {
            public Vector2 Velocity { get; set; }
            public int MoveSpeed { get; set; }

            private float timeToDestroy;

            public Bullet(Vector2 position, Vector2 size, Vector2 dir, int moveSpeed) : base(position, size)
            {
                Tag = "Trap";
                MoveSpeed = moveSpeed;
                Velocity = dir * moveSpeed;
                Fill = new SolidColorBrush(Colors.Black);
                timeToDestroy = 2f;
            }

            public override void MoveX(float amount, Action onCollision)
            {
                xRemainder += amount;
                int move = (int)Math.Round(xRemainder, MidpointRounding.ToEven);

                if (move != 0)
                {
                    xRemainder -= move;
                    int sign = Math.Sign(move);

                    while (move != 0)
                    {
                        GameObject obj;
                        IntRect tempRect = new IntRect
                        {
                            X = Hitbox.X + sign,
                            Y = Hitbox.Y,
                            Width = Hitbox.Width,
                            Height = Hitbox.Height
                        };

                        if (Physics.IsColliding(players, tempRect, out obj))
                        {
                            (obj as Player).Die();
                        }

                        if (!Physics.IsColliding(solids, tempRect))
                        {
                            //  We don't collide with anyting solid
                            Transform.Position.X += sign;
                            Hitbox.X += sign;
                            move -= sign;
                        }
                        else
                        {
                            onCollision?.Invoke();
                            break;
                        }
                    }
                }
            }

            public override void MoveY(float amount, Action onCollision)
            {
                yRemainder += amount;
                int move = (int)Math.Round(yRemainder, MidpointRounding.ToEven);

                if (move != 0)
                {
                    yRemainder -= move;
                    int sign = Math.Sign(move);

                    while (move != 0)
                    {
                        GameObject obj;
                        IntRect tempRect = new IntRect
                        {
                            X = Hitbox.X,
                            Y = Hitbox.Y + sign,
                            Width = Hitbox.Width,
                            Height = Hitbox.Height
                        };

                        if (Physics.IsColliding(players, tempRect, out obj))
                        {
                            (obj as Player).Die();
                        }

                        if (!Physics.IsColliding(solids, tempRect))
                        {
                            //  We don't collide with anyting solid
                            Transform.Position.Y += sign;
                            Hitbox.Y += sign;
                            move -= sign;
                        }
                        else
                        {
                            onCollision?.Invoke();
                            break;
                        }
                    }
                }
            }

            public override void Update(float deltaTime)
            {
                MoveX(Velocity.X * deltaTime, () => { Fill = new SolidColorBrush(Colors.Transparent); Tag = "Default"; });
                MoveY(Velocity.Y * deltaTime, () => { Fill = new SolidColorBrush(Colors.Transparent); Tag = "Default"; });

                timeToDestroy -= deltaTime;
                if (timeToDestroy <= 0)
                {
                    NeedToRemove = true;
                }
            }
        }

        public Cannon(Vector2 position, Vector2 size, bool grabbable = false) : base(position, size, grabbable)
        {

        }

        public Direction ShootingDirection { get; set; }
        public Vector2 BulletDirection { get; set; }

        private Bullet[] bullets;
        private float shootingTime;
        private float timeToShoot;
        private int bulletSpeed;

        private bool needToShoot;

        public Cannon(Vector2 position, Vector2 size, Direction dir, bool grabbable = false) : base(position, size, grabbable)
        {
            Fill = new SolidColorBrush(Colors.Black);
            ShootingDirection = dir;

            shootingTime = 1;
            timeToShoot = shootingTime;
            bulletSpeed = 100;

            bullets = new Bullet[2];
            Bullet bullet = new Bullet(new Vector2(Transform.Position.X + 10 / 2, Transform.Position.Y - Transform.Size.Y / 2), new Vector2(10, 10), BulletDirection, bulletSpeed);
            Bullet bullet2 = new Bullet(new Vector2(Transform.Position.X + 10 / 2, Transform.Position.Y - Transform.Size.Y / 2), new Vector2(10, 10), BulletDirection, bulletSpeed);
            bullets[0] = bullet;
            bullets[1] = bullet2;

            interactables.Add(bullet);
            interactables.Add(bullet2);

            switch (ShootingDirection)
            {
                case Direction.Up:
                    BulletDirection = new Vector2(0, -1);
                    break;
                case Direction.Down:
                    BulletDirection = new Vector2(0, 1);
                    break;
                case Direction.Left:
                    BulletDirection = new Vector2(-1, 0);
                    break;
                case Direction.Right:
                    BulletDirection = new Vector2(1, 0);
                    break;
            }
        }

        public void Shoot()
        {
            
        }

        public override void Update(float deltaTime)
        {
            if (timeToShoot <= 0)
            {
                Shoot();
                timeToShoot = shootingTime;
            }
            else
            {
                timeToShoot -= deltaTime;
            }
        }

        public override void LateUpdate()
        {
            if (needToShoot)
            {
                Shoot();
                needToShoot = false;
            }
        }
    }
}
