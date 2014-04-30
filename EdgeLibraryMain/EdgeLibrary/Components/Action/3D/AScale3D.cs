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
    //Scales a sprite over time
    public class AScale3D : Action3D
    {
        public Vector3 TargetScale;
        private Vector3 originalScale;
        public float Time;
        private double elapsedTime;

        public AScale3D(Vector3 targetScale, float time)
        {
            TargetScale = targetScale;
            Time = time;
            elapsedTime = 0;
        }

        protected override void UpdateAction(GameTime gameTime, Sprite3D sprite)
        {
            if (originalScale == null) { originalScale = sprite.Scale; }

            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds * EdgeGame.GameSpeed;

            if (elapsedTime >= Time)
            {
                Stop(gameTime, sprite);
            }

            sprite.Scale = new Vector3(MathHelper.Lerp(originalScale.X, TargetScale.X, (float)elapsedTime / Time),
                                        MathHelper.Lerp(originalScale.Y, TargetScale.Y, (float)elapsedTime / Time),
                                        MathHelper.Lerp(originalScale.Z, TargetScale.Z, (float)elapsedTime / Time));
        }

        public override Action3D Clone()
        {
            return new AScale3D(TargetScale, Time);
        }
    }
}
