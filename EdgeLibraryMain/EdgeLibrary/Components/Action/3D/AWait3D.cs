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
    public class AWait3D : Action3D
    {
        public float WaitTime;
        private float elapsedTime;

        public AWait3D(float waitTime)
        {
            WaitTime = waitTime;
            elapsedTime = 0;
        }

        protected override void UpdateAction(GameTime gameTime, Sprite3D sprite)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds * EdgeGame.GameSpeed;

            if (elapsedTime >= WaitTime)
            {
                Stop(gameTime, sprite);
            }
        }

        public override Action3D Clone()
        {
            return new AWait3D(WaitTime);
        }
    }
}
