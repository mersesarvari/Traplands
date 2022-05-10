using System;
using System.Collections.Generic;

namespace Server.Models
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

        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X * b.X, a.Y * b.Y);
        }

        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X / b.X, a.Y / b.Y);
        }

        public static Vector2 operator *(Vector2 a, int b)
        {
            return new Vector2(a.X * b, a.Y * b);
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            return MathF.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
    }
}
