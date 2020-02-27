using Jong2D.Utility;

namespace Jong2D.Framework.Collision
{
    public class CircularBox : ICollisionBox
    {
        public double Radius { get; set; }
        public Vector2D Pos { get; set; }

        public bool Collide(ICollisionBox bb)
        {
            if (bb is CircularBox) return Collide((CircularBox)bb);
            return false;
        }

        public bool Collide(CircularBox bb)
        {
            Vector2D toVec = bb.Pos - Pos;
            return Radius * Radius < toVec.LengthSquare();
        }
    }
}
