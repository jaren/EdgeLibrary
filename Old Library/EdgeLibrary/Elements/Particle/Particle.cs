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
        protected TimeSpan livedTime;

        public float lifeTime;
        public Vector2 velocity;
        public float rotateSpeed;
        public float growSpeed;

        public ColorChangeIndex ColorIndex;

        public bool shouldRemove;

        public Particle(string id, string eTextureName, float eLifeTime, float eRotateSpeed, float eGrowSpeed) : base(id, eTextureName, Vector2.Zero, false)
        {
            lifeTime = eLifeTime;
            rotateSpeed = eRotateSpeed;
            growSpeed = eGrowSpeed;

            shouldRemove = false;
            livedTime = TimeSpan.Zero;
        }

        protected override void updateElement(GameTime gameTime)
        {
            base.updateElement(gameTime);

            livedTime += gameTime.ElapsedGameTime;

            if (livedTime.TotalMilliseconds >= lifeTime)
            {
                shouldRemove = true;
            }
            else
            {
                Position += velocity;
                Style.Rotation += rotateSpeed/10;
                Width += growSpeed;
                Height += growSpeed;

                Style.Color = ColorIndex.Update(gameTime);
            }
        }

        protected override void drawElement(GameTime gameTime)
        {
            base.drawElement(gameTime);
        }
    }
}
