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
    [Flags]
    public enum CollisionLayers : byte
    {
        A = 1,
        B = 2,
        C = 4,
        D = 8,
        E = 16,
        F = 32,
        G = 64,
        All = 127
    }

    //The base for all platform sprites
    public class PlatformSprite : Sprite
    {
        public bool MarkedForPlatformRemoval {get; set;}
        public CollisionLayers CollisionLayers;

        public new delegate void CollisionEvent(PlatformSprite sprite1, PlatformSprite sprite2, GameTime gameTime);
        public new virtual event CollisionEvent Collision;

        public PlatformSprite(string id, string eTextureName, Vector2 ePosition) : base(id, eTextureName, ePosition)
        { 
            MarkedForRemoval = true;
            CollisionLayers = CollisionLayers.All;
        }

        protected virtual void UpdateCollision(List<PlatformSprite> sprites, Vector2 Gravity, GameTime gameTime)
        {
            foreach (PlatformSprite sprite in sprites)
            {
                if ((sprite.CollisionLayers & CollisionLayers) != 0 && sprite != this)
                {
                    if (sprite.BoundingBox.Intersects(BoundingBox))
                    {
                        if (Collision != null)
                        {
                            Collision(this, sprite, gameTime);
                        }

                        Rectangle collision = Rectangle.Intersect(BoundingBox, sprite.BoundingBox);

                        //If it's collided in horizontally more than vertical
                        if (Math.Abs(collision.Width) > Math.Abs(collision.Height))
                        {
                            if (Position.Y > sprite.Position.Y)
                            {
                                Position = new Vector2(Position.X, Position.Y + collision.Height);
                            }
                            else
                            {
                                Position = new Vector2(Position.X, Position.Y - collision.Height);
                            }
                        }
                        else
                        {
                            if (Position.X > sprite.Position.X)
                            {
                                Position = new Vector2(Position.X + collision.Width, Position.Y);
                            }
                            else
                            {
                                Position = new Vector2(Position.X - collision.Width, Position.Y);
                            }
                        }
                    }
                }
            }
        }

        public virtual void UpdateForces(Vector2 Gravity)
        {
            Position -= Gravity;
        }

        public void UpdateSprite(GameTime gameTime, Vector2 Gravity, List<PlatformSprite> sprites)
        {
            UpdateForces(Gravity);
            UpdateCollision(sprites, Gravity, gameTime);
        }
    }
}
