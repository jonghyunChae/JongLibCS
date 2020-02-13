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
