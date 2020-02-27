using Jong2D.Utility;

namespace Jong2D.Framework.Collision
{
    public class BoundingBox : ICollisionBox
    {
        public Vector2D MinPoint { get; set; }
        public Vector2D MaxPoint { get; set; }

        public bool Collide(ICollisionBox bb)
        {
            if (bb is BoundingBox) return Collide((BoundingBox)bb);
            return false;
        }

        public bool Collide(BoundingBox bb)
        {
            if (this.MinPoint.x > bb.MaxPoint.x) return false;
            if (this.MaxPoint.x < bb.MinPoint.x) return false;
            if (this.MinPoint.y > bb.MaxPoint.y) return false;
            if (this.MaxPoint.y < bb.MinPoint.y) return false;

            return true;
        }

        public static BoundingBox Create(Vector2D Pos, Size2D size)
        {
            int w = size.width / 2;
            int h = size.height / 2;
            return new BoundingBox()
            {
                MinPoint = new Vector2D(Pos.x - w, Pos.y - h),
                MaxPoint = new Vector2D(Pos.x + w, Pos.y + h),
            };
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(MinPoint, new Size2D((int)(MaxPoint.x - MinPoint.x), (int)(MaxPoint.y - MinPoint.y)));
        }
    }
}
