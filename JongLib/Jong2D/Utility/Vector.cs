using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jong2D.Utility
{

    public struct Vector2D
    {
        public double x { get; set; }
        public double y { get; set; }

        public Vector2D(double x = 0, double y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2D operator*(Vector2D src, double n)
        {
            return new Vector2D(src.x * n, src.y * n);
        }

        public static Vector2D operator /(Vector2D src, double n)
        {
            return new Vector2D(src.x / n, src.y / n);
        }

        public static Vector2D operator +(Vector2D src, Vector2D dest)
        {
            return new Vector2D(src.x + dest.x, src.y + dest.y);
        }

        public static Vector2D operator -(Vector2D src, Vector2D dest)
        {
            return new Vector2D(src.x - dest.x, src.y - dest.y);
        }

        public double LengthSquare()
        {
            return x * x + y * y;
        }

        public double Length()
        {
            return Math.Sqrt(LengthSquare());
        }

        public Vector2D Normalize()
        {
            var lengthSqr = LengthSquare();
            if (lengthSqr == 0)
            {
                return this;
            }
            return this / Length();
        }

        public override string ToString()
        {
            return $"[X:{x}, Y:{y}]";
        }
    }

    public struct Size2D
    {
        public int width { get; set; }
        public int height { get; set; }

        public Size2D(int width = 0, int height= 0)
        {
            this.width = width;
            this.height = height;
        }

        public override string ToString()
        {
            return $"[W:{width}, H:{height}]";
        }

    }

}
