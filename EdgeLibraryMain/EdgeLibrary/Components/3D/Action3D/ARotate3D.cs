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

namespace EdgeLibrary
{
    //Adds Vector3 to a sprite's rotation
    public class ARotate3D : Action3D
    {
        public Vector3 Speed;

        public ARotate3D(Vector3 radians)
        {
            Speed = radians;
        }

        protected override void UpdateAction(GameTime gameTime, Sprite3D sprite)
        {
            sprite.Rotation += Speed;
        }

        public override Action3D Clone()
        {
            return new ARotate3D(Speed);
        }
    }
}
