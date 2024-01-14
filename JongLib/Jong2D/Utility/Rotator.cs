using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jong2D.Utility
{
    public class Rotator2D
    {
        public static Vector3D Roll = new Vector3D(0, 0, 1);

        public Vector2D Pitch { get; private set; }

        public Vector2D Yaw { get; private set; }

        public Rotator2D(Vector2D pitch, Vector2D yaw)
        {
            Pitch = pitch;
            Yaw = yaw;
        }
    }
}
