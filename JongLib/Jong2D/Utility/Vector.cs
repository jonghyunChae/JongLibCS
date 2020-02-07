using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jong2D.Utility
{
    public struct Vector2D
    {
        public int x { get; set; }
        public int y { get; set; }

        public Vector2D(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
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
    }

}
