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

        protected virtual void UpdateCollision(List<PlatformSprite> sprites, GameTime gameTime)
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

                        float VerticalCollision = Math.Min(Math.Abs(BoundingBox.Top - sprite.BoundingBox.Bottom), Math.Abs(sprite.BoundingBox.Top - BoundingBox.Bottom));
                        float HorizontalCollision = Math.Min(Math.Abs(BoundingBox.Left - sprite.BoundingBox.Right), Math.Abs(sprite.BoundingBox.Left - BoundingBox.Right));
                        if (VerticalCollision < HorizontalCollision)
                        {
                            //This collision box is on the bottom of the other one
                            if (BoundingBox.Top - sprite.BoundingBox.Bottom < sprite.BoundingBox.Top - BoundingBox.Bottom)
                            {
                                Position = new Vector2(Position.X - VerticalCollision, Position.Y);
                            }
                            else
                            {
                                Position = new Vector2(Position.X + VerticalCollision, Position.Y);
                            }
                        }
                        else
                        {
                            //This collision box is on the left of the other one
                            if (BoundingBox.Right - sprite.BoundingBox.Left < sprite.BoundingBox.Right - BoundingBox.Left)
                            {
                                Position = new Vector2(Position.X, Position.Y - HorizontalCollision);
                            }
                            else
                            {
                                Position = new Vector2(Position.X, Position.Y + HorizontalCollision);
                            }
                        }
                    }
                }
            }
        }

        public virtual void UpdateMotion(Vector2 Gravity)
        {
            Position -= Gravity;
        }

        public void UpdateSprite(GameTime gameTime, Vector2 Gravity, List<PlatformSprite> sprites)
        {
            UpdateMotion(Gravity);
            UpdateCollision(sprites, gameTime);
        }
    }
}
