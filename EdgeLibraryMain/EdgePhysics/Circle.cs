using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EdgePhysics
{
    public class Circle
    {
        public Vector2 Position;
        public float Radius;

        bool CirclevsCircleUnoptimized(Circle c)
        {
            float r = Radius + c.Radius;

            return r < Vector2.Distance(Position, c.Position);
        }

        //Doesn't use square roots
        bool CirclevsCircleOptimized(Circle c)
        {
            float r = Radius + c.Radius;
            r *= r;
            return r < ((Position.X + c.Position.X) * (Position.X + c.Position.X)
                        + (Position.Y + c.Position.Y) * (Position.Y + c.Position.Y));
        }
    }
}
