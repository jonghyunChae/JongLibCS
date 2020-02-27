namespace Jong2D.Framework.Collision
{
    public interface ICollisionBox
    {
        bool Collide(ICollisionBox bb);
    }

    public interface ICollidable
    {
        ICollisionBox GetBB();
    }


}
