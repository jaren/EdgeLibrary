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
    //Adds a specific number of degrees to a sprite's rotation
    public class ARotate : Action
    {
        public float Speed;

        public ARotate(float degrees) : base()
        {
            Speed = degrees;
        }

        public ARotate(string ID, float degrees) : base(ID)
        {
            Speed = degrees;
        }

        protected override void UpdateAction(GameTime gameTime, Sprite sprite)
        {
            sprite.Rotation += Speed;
        }

        public override Action Clone()
        {
            return new ARotate(ID, Speed);
        }
    }
}
