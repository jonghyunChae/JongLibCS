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

        public Line(Vector2D from, Vector2D to)
        {
            this.From = from;
            this.To = to;
        }
    }

    public struct Rectangle
    {
        // 좌측하단 지점
        public Vector2D pos;
        public int x { get => pos.x; set { pos.x = value; } }
        public int y { get => pos.y; set { pos.y = value; } }

        public Size2D size;
        public int width { get => size.width; set { size.width = value; } }
        public int height { get => size.height; set { size.height = value; } }

        public Rectangle(int x = 0, int y = 0, int width = 0, int height = 0)
        {
            this.pos = new Vector2D(x, y);
            this.size = new Size2D(width, height);
        }

        public Rectangle(Vector2D pos, Size2D size)
        {
            this.pos = pos;
            this.size = size;
        }
    }
}
