using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Jong2D.Utility
{
    public static class VectorHelper
    {
        public static double Cross(Vector2D src, Vector2D dest) =>
            src.x * dest.y - src.y * dest.x;

        public static double Dot(Vector2D src, Vector2D dest) =>
            src.x * dest.x + src.y * dest.y;

        public static Vector3D Cross(Vector3D src, Vector3D dest) =>
            new Vector3D(
                x: src.y * dest.z - src.z * dest.y,
                y: src.z * dest.x - src.x * dest.z,
                z: src.x * dest.y - src.y * dest.x);

        public static double Dot(Vector3D src, Vector3D dest) =>
            src.x * dest.x + src.y * dest.y + src.z + dest.z;

        public static bool IsCounterClockwise(Vector2D src, Vector2D dest) => 
            Cross(src, dest) > 0;

        public static bool IsClockwise(Vector2D src, Vector2D dest) => 
            Cross(src, dest) < 0;
    }

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

        public static Vector2D operator -(Vector2D src, double scalar)
        {
            return new Vector2D(src.x - scalar, src.y - scalar);
        }

        public static Vector2D operator -(Vector2D src, int scalar)
        {
            return new Vector2D(src.x - scalar, src.y - scalar);
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

        public Vector3D ToVector3D() =>
            new Vector3D(x, y, 0);

        public Rotator2D ToRotator()
        {
            var unitVector = Normalize();
            return new Rotator2D(
                pitch: unitVector.ToVector3D().Normalize().Cross(Rotator2D.Roll).ToVector2D(),
                yaw: unitVector);
        }

        public double Dot(Vector2D dest) => VectorHelper.Dot(this, dest);

        public override string ToString()
        {
            return $"[X:{x.ToString("F2")}, Y:{y.ToString("F2")}]";
        }
    }

    public struct Vector3D
    {
        public double x { get; set; }
        public double y { get; set; }

        public double z { get; set; }

        public Vector3D(double x = 0, double y = 0, double z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3D operator *(Vector3D src, double n)
        {
            return new Vector3D(src.x * n, src.y * n, src.z * n);
        }

        public static Vector3D operator /(Vector3D src, double n)
        {
            return new Vector3D(src.x / n, src.y / n, src.z / n);
        }

        public static Vector3D operator +(Vector3D src, Vector3D dest)
        {
            return new Vector3D(src.x + dest.x, src.y + dest.y, src.z + dest.z);
        }

        public static Vector3D operator -(Vector3D src, Vector3D dest)
        {
            return new Vector3D(src.x - dest.x, src.y - dest.y, src.z - dest.z);
        }

        public static Vector3D operator -(Vector3D src, double scalar)
        {
            return new Vector3D(src.x - scalar, src.y - scalar, src.z - scalar);
        }

        public static Vector3D operator -(Vector3D src, int scalar)
        {
            return new Vector3D(src.x - scalar, src.y - scalar, src.z - scalar);
        }

        public double LengthSquare()
        {
            return x * x + y * y + z * z;
        }

        public double Length()
        {
            return Math.Sqrt(LengthSquare());
        }

        public Vector3D Normalize()
        {
            var lengthSqr = LengthSquare();
            if (lengthSqr == 0)
            {
                return this;
            }
            return this / Length();
        }

        public Vector2D ToVector2D() =>
            new Vector2D(x, y);

        public Vector3D Cross(Vector3D target) =>
            VectorHelper.Cross(this, target);

        public double Dot(Vector3D dest) => VectorHelper.Dot(this, dest);

        public override string ToString()
        {
            return $"[X:{x.ToString("F2")}, Y:{y.ToString("F2")}, Z:{z.ToString("F2")}]";
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
