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
    public class Sprite : Element
    {
        public Sprite() : base()
        {
        }

        protected override void DrawObject(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        protected override void UpdateObject(GameTime gameTime)
        {
        }

        public virtual void DebugDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
    }
}
