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
using System.Xml;
using System.Xml.Linq;

namespace EdgeLibrary
{
    public class Particle : Sprite
    {
        protected double livedTime;

        public float LifeTime;
        public Vector2 Velocity;
        public float RotateSpeed;
        public float GrowSpeed;

        public ColorChangeIndex ColorIndex;

        public bool shouldRemove;

        public Particle(float lifeTime, float rotateSpeed, float growSpeed) : base("", Vector2.Zero)
        {
            LifeTime = lifeTime;
            RotateSpeed = rotateSpeed;
            GrowSpeed = growSpeed;

            shouldRemove = false;
            livedTime = 0;
        }

        protected override void UpdateObject(GameTime gameTime)
        {
            base.UpdateObject(gameTime);

            livedTime += gameTime.ElapsedGameTime.TotalMilliseconds * EdgeGame.GetFrameTimeMultiplier(gameTime);

            if (livedTime >= LifeTime)
            {
                shouldRemove = true;
            }
            else
            {
                Position += Velocity * (float)EdgeGame.GetFrameTimeMultiplier(gameTime);
                Rotation += RotateSpeed / 10f * (float)EdgeGame.GetFrameTimeMultiplier(gameTime);
                Scale += new Vector2(GrowSpeed / Width * (float)EdgeGame.GetFrameTimeMultiplier(gameTime),
                    GrowSpeed / Height * (float)EdgeGame.GetFrameTimeMultiplier(gameTime));

                Color = ColorIndex.Update(gameTime);
            }
        }
    }
}
