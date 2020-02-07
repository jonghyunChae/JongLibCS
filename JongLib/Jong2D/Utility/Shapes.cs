using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jong2D.Utility
{
    public struct Line
    {
        Vector2D From { get; set; }
        Vector2D To { get; set; }
    }

    public struct Rectangle
    {
        public int left { get; set; }
        public int bottom { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public Rectangle(int left = 0, int bottom = 0, int width = 0, int height = 0)
        {
            this.left = left;
            this.bottom = bottom;
            this.width = width;
            this.height = height;
        }
    }
}
