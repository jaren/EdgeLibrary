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
    //Adds a specific number of radians to a sprite's rotation
    public class ARotate : Action
    {
        public float Speed;

        public ARotate(float radians) : base()
        {
            Speed = radians;
        }

        protected override void UpdateAction(GameTime gameTime, Sprite sprite)
        {
            sprite.Rotation += Speed * EdgeGame.GameSpeed;
        }

        public override Action SubClone()
        {
            return new ARotate(Speed);
        }
    }
}
