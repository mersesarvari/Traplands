using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;
using WPFGameTest.Models;

namespace WPFGameTest.Helpers
{
    public class IntRect
    {
        public float X, Y;
        public int Width, Height;

        public IntRect()
        {
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
        }

        public IntRect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }

    public class Vector2f
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Vector2f(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2f()
        {
            X = 0;
            Y = 0;
        }

        public static Vector2f operator +(Vector2f a, Vector2f b)
        {
            return new Vector2f(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2f operator -(Vector2f a, Vector2f b)
        {
            return new Vector2f(a.X - b.X, a.Y - b.Y);
        }

        public static float Distance(Vector2f a, Vector2f b)
        {
            return (float)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
    }

    public class Vector2
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2()
        {
            X = 0;
            Y = 0;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            return MathF.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
    }

    public static class Physics
    {
        public const float Gravity = 1850;
        public const float FallClamp = 1500;
        public const float WallSlideSpeed = 150;

        public static bool IsColliding(List<StaticObject> solids, IntRect b)
        {
            foreach (var solid in solids)
            {
                IntRect a = solid.Hitbox;

                if (a.X < b.X + b.Width &&
                    a.X + a.Width > b.X &&
                    a.Y < b.Y + b.Height &&
                    a.Y + a.Height > b.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsColliding(List<StaticObject> solids, IntRect b, out StaticObject collidedWith)
        {
            foreach (var solid in solids)
            {
                IntRect a = solid.Hitbox;

                if (a.X < b.X + b.Width &&
                    a.X + a.Width > b.X &&
                    a.Y < b.Y + b.Height &&
                    a.Y + a.Height > b.Y)
                {
                    collidedWith = solid;
                    return true;
                }
            }

            collidedWith = null;
            return false;
        }

        public static bool IsColliding(IntRect a, IntRect b)
        {
            if (a.X < b.X + b.Width &&
                a.X + a.Width > b.X &&
                a.Y < b.Y + b.Height &&
                a.Y + a.Height > b.Y)
            {
                return true;
            }

            return false;
        }

        public static bool IsColliding(Rectangle a, Rectangle b)
        {
            if (Canvas.GetLeft(a) < Canvas.GetLeft(b) + b.Width &&
                Canvas.GetLeft(a) + a.Width > Canvas.GetLeft(b) &&
                Canvas.GetTop(a) < Canvas.GetTop(b) + b.Height &&
                Canvas.GetTop(a) + a.Height > Canvas.GetTop(b))
            {
                return true;
            }

            return false;
        }

        public static bool PointInRect(int x, int y, Rectangle rect)
        {
            if (x > Canvas.GetLeft(rect) &&
                x < Canvas.GetLeft(rect) + rect.Width &&
                y > Canvas.GetTop(rect) &&
                y < Canvas.GetTop(rect) + rect.Height)
            {
                return true;
            }

            return false;
        }
    }
}
