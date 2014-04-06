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
    public class Box
    {
        public Vector2 Min;
        public Vector2 Max;
        private Vector2 _min;
        private Vector2 _max;

        public bool CheckColliding(Box box)
        {
            if(Max.X < box.Min.X || Min.X > box.Max.X) { return false; }
            if(Max.Y < box.Min.Y || Min.Y > box.Max.Y) { return false; }
            return true;
        }
    }
}
