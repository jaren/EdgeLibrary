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
    public class AScale : Action
    {
        public Vector2 TargetScale;
        private Vector2 originalScale;
        public float Time;
        private double elapsedTime;

        public AScale(Vector2 targetScale, float time) : base()
        {
            TargetScale = targetScale;
            Time = time;
            elapsedTime = 0;
        }

        public AScale(string id, Vector2 targetScale, float time) : base(id)
        {
            TargetScale = targetScale;
            Time = time;
            elapsedTime = 0;
        }

        protected override void UpdateAction(GameTime gameTime, Sprite sprite)
        {
            if (originalScale == null) { originalScale = sprite.Scale; }

            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds * EdgeGame.GetFrameTimeMultiplier(gameTime);

            if (elapsedTime >= Time)
            {
                Stop(gameTime, sprite);
            }

            sprite.Scale = new Vector2(MathHelper.Lerp(originalScale.X, TargetScale.X, (float)elapsedTime / Time), MathHelper.Lerp(originalScale.Y, TargetScale.Y, (float)elapsedTime / Time));
        }

        public override Action Clone()
        {
            return new AScale(ID, TargetScale, Time);
        }
    }
}
