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
    //Changes the color of a sprite using a ColorChangeIndex
    public class AColorChange : AAction
    {
        public ColorChangeIndex Index;

        public AColorChange(Color start, Color finish, float time) : this(new ColorChangeIndex(time, start, finish)) {}
        public AColorChange(ColorChangeIndex index)
        {
            Index = index;
        }

        //Changes the sprite's color based on the color change index
        protected override void UpdateAction(GameTime gameTime, Sprite sprite)
        {
            sprite.Color = Index.Update(gameTime);

            if (Index.HasFinished)
            {
                Stop(gameTime, sprite);
            }
        }

        public override AAction Copy()
        {
            return new AColorChange(Index);
        }
    }
}
