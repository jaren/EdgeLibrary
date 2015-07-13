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
    //Changes the color of a sprite using multiple textures
    public class ATextureChange : Action
    {
        public TextureChangeIndex Index;

        public ATextureChange(TextureChangeIndex index)
            : base()
        {
            Index = index;
        }

        //Changes the sprite's texture based on the texture change index
        protected override void UpdateAction(GameTime gameTime, Sprite sprite)
        {
            sprite.Texture = Index.Update(gameTime);

            if (Index.HasFinished)
            {
                Stop(gameTime, sprite);
            }
        }

        public override void Reset()
        {
            Index.ResetTime();
            base.Reset();
        }

        public override Action SubClone()
        {
            return new ATextureChange(Index);
        }
    }
}
