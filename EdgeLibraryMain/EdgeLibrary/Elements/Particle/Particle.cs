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

        protected float lifeTime;
        protected Vector2 velocity;
        protected float rotateSpeed;
        protected float growSpeed;

        public Color StartColor;
        public Color FinishColor;

        public bool shouldRemove;

        public Particle(string id, string eTextureName, float eLifeTime, Vector2 eVelocity, float eRotateSpeed, float eGrowSpeed) : base(id, eTextureName, Vector2.Zero)
        {
            lifeTime = eLifeTime;
            velocity = eVelocity;
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
                Rotation += rotateSpeed;
                Width += growSpeed;
                Height += growSpeed;

                Color = Color.Lerp(StartColor, FinishColor, (float)livedTime.TotalMilliseconds/lifeTime);
            }
        }
    }
}
