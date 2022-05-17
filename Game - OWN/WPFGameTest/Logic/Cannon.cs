using Game.Helpers;
using Game.Models;
using Game.Renderer;
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
        Right,
        Left
    }

    public class Bullet : DynamicObject
    {
        public Vector2 Velocity { get; set; }
        public Vector2 Dir { get; set; }
        public int MoveSpeed { get; set; }

        public Vector2 SpawnPosition { get; set; }

        private float timeToDestroy;
        private float timeLeft;

        public Bullet(Vector2 position, Vector2 size, Vector2 dir, int moveSpeed) : base(position, size)
        {
            Tag = "Trap";
            MoveSpeed = moveSpeed;
            Dir = dir;
            Velocity = dir * moveSpeed;
            Fill = new ImageBrush(Resource.GetImage("Bullet"));
            SpawnPosition = new Vector2(position.X, position.Y);
            timeToDestroy = 2f;
            timeLeft = timeToDestroy;
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
                        onCollision?.Invoke();
                        break;
                    }
                }
            }
        }

        public void Shoot()
        {
            Tag = "Trap";
            Fill.Opacity = 1;
            Velocity = Dir * MoveSpeed;
            Transform.Position = new Vector2(SpawnPosition.X, SpawnPosition.Y);
            Hitbox.X = Transform.Position.X;
            Hitbox.Y = Transform.Position.Y;
            timeLeft = timeToDestroy;
        }

        public void Stop()
        {
            Tag = "Default";
            Fill.Opacity = 0;
            Velocity = new Vector2(0, 0);
            Transform.Position = new Vector2(SpawnPosition.X, SpawnPosition.Y);
            Hitbox.X = Transform.Position.X;
            Hitbox.Y = Transform.Position.Y;
        }

        public override void Update(float deltaTime)
        {
            MoveX(Velocity.X * deltaTime, Stop);

            timeLeft -= deltaTime;
            if (timeLeft <= 0)
            {
                Stop();
            }
        }
    }

    public class Cannon : SolidObject
    {
        public Direction ShootingDirection { get; set; }
        public Vector2 BulletDirection { get; set; }

        private Bullet[] bullets;
        private float shootingTime;
        private float timeToShoot;
        private int bulletSpeed;
        private int nextBullet;

        public Cannon(Vector2 position, Vector2 size, Direction dir, bool grabbable = false) : base(position, size, grabbable)
        {
            ShootingDirection = dir;

            shootingTime = 1;
            timeToShoot = shootingTime;
            bulletSpeed = 500;
            nextBullet = 0;

            int bulletSize = 16;
            int diff = 0;

            switch (ShootingDirection)
            {
                case Direction.Right:
                    BulletDirection = new Vector2(1, 0);
                    diff = Transform.Size.X;
                    break;
                case Direction.Left:
                    BulletDirection = new Vector2(-1, 0);
                    diff = -bulletSize;
                    break;
            }

            bullets = new Bullet[2];
            Bullet bullet = new Bullet(
                new Vector2(Transform.Position.X + diff, 
                            Transform.Position.Y - Transform.Size.Y / 2 + 24), 
                new Vector2(bulletSize, bulletSize), 
                BulletDirection, 
                bulletSpeed);
            Bullet bullet2 = new Bullet(
                new Vector2(Transform.Position.X + diff, 
                            Transform.Position.Y - Transform.Size.Y / 2 + 24),
                new Vector2(bulletSize, bulletSize),
                BulletDirection,
                bulletSpeed);
            bullets[0] = bullet;
            bullets[1] = bullet2;
        }

        public void Shoot()
        {
            bullets[nextBullet].Shoot();
            nextBullet = nextBullet == 0 ? 1 : 0;
            timeToShoot = shootingTime;
        }

        public override void Start()
        {
            interactables.Add(bullets[0]);
            interactables.Add(bullets[1]);
            timeToShoot = shootingTime;
        }

        public override void Update(float deltaTime)
        {
            if (timeToShoot <= 0)
            {
                Shoot();
            }
            else
            {
                timeToShoot -= deltaTime;
            }
        }
    }
}
