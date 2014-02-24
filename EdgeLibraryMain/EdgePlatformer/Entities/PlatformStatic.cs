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
using EdgeLibrary;

namespace EdgeLibrary.Platform
{
    //A platform sprite which WILL NOT MOVE unless the position is manually changed
    class PlatformStatic : PlatformSprite
    {
        public new event CollisionEvent Collision;

        public PlatformStatic(string id, string eTextureName, Vector2 ePosition) : base(id, eTextureName, ePosition) { }

        //Doesn't move in collisions
        protected override void UpdateCollision(List<PlatformSprite> sprites, GameTime gameTime)
        {
            foreach (PlatformSprite sprite in sprites)
            {
                if ((sprite.CollisionLayers & CollisionLayers) != 0 && sprite != this && sprite.BoundingBox.Intersects(BoundingBox))
                {
                        if (Collision != null)
                        {
                            Collision(this, sprite, gameTime);
                        }
                }
            }
        }

        //Isn't affected by gravity
        public override void UpdateMotion() { }
    }
}
