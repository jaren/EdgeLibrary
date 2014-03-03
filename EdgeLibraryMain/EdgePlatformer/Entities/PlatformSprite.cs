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
        private struct Force
        {
            public Vector2 Impulse;
            public float Deceleration;

            public Force(Vector2 impulse, float deceleration)
            {
                Impulse = impulse;
                Deceleration = deceleration;
            }

            //Decreases the impulse
            public bool ApplyForce()
            {
                Impulse = MathTools.DecreaseVector(Impulse, Deceleration);
                if (Impulse == Vector2.Zero)
                {
                    return false;
                }
                return true;
            }
        }

        public bool MarkedForPlatformRemoval {get; set;}
        public CollisionLayers CollisionLayers;
        public float Acceleration;
        public float Deceleration;

        private List<Force> forces;

        private float fallSpeed;

        private bool collidingUp;
        private bool collidingDown;
        private bool collidingLeft;
        private bool collidingRight;

        public new delegate void CollisionEvent(PlatformSprite sprite1, PlatformSprite sprite2, GameTime gameTime);
        public new virtual event CollisionEvent Collision;

        public PlatformSprite(string id, string eTextureName, Vector2 ePosition) : base(id, eTextureName, ePosition)
        { 
            MarkedForRemoval = true;
            CollisionLayers = CollisionLayers.All;
            Acceleration = 0.1f;
            Deceleration = 0.06f;
            forces = new List<Force>();
        }

        protected virtual void UpdateCollision(List<PlatformSprite> sprites, Vector2 Gravity, GameTime gameTime)
        {
            collidingUp = false;
            collidingDown = false;
            collidingLeft = false;
            collidingRight = false;

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
                            //When collided into something, acceleration is reset
                            fallSpeed = 0;


                            //The Y collisions are flipped because the screen coordinates are flipped
                            if (Position.Y > sprite.Position.Y)
                            {
                                Position = new Vector2(Position.X, Position.Y + collision.Height);
                                collidingDown = true;
                            }
                            else
                            {
                                Position = new Vector2(Position.X, Position.Y - collision.Height);
                                collidingUp = true;
                            }
                        }
                        else
                        {
                            if (Position.X > sprite.Position.X)
                            {
                                Position = new Vector2(Position.X + collision.Width, Position.Y);
                                collidingLeft = true;
                            }
                            else
                            {
                                Position = new Vector2(Position.X - collision.Width, Position.Y);
                                collidingRight = true;
                            }
                        }
                    }
                }
            }
        }

        public bool TryMove(Vector2 vector)
        {
            if ((vector.X < 0 && !collidingLeft) || (vector.X > 0 && !collidingRight))
            {
                Position = new Vector2(Position.X + vector.X, Position.Y);
                return true;
            }

            if ((vector.Y < 0 && !collidingDown) || (vector.Y > 0 && !collidingUp))
            {
                //It subtracts from the Y because the screen coordinates are flipped
                Position = new Vector2(Position.X, Position.Y - vector.Y);
                return true;
            }
            return false;
        }

        public void ApplyImpulse(Vector2 impulse, float deceleration)
        {
            forces.Add(new Force(impulse, deceleration));
        }

        public void ApplyImpulse(Vector2 impulse)
        {
            ApplyImpulse(impulse, Deceleration);
        }

        public virtual void UpdateForces(Vector2 Gravity)
        {
            for (int i = 0; i < forces.Count; i++)
            {
                if (forces[i].ApplyForce())
                {
                    TryMove(forces[i].Impulse);
                }
                else
                {
                    forces.Remove(forces[i]);
                    i--;
                }
                
            }

            if (!collidingDown)
            {
                fallSpeed += Acceleration / 10;
              //  Position -= Gravity * fallSpeed;
            }
        }

        public virtual void UpdatePlatform(GameTime gameTime, Vector2 Gravity, List<PlatformSprite> sprites)
        {
            base.updateElement(gameTime);
            UpdateForces(Gravity);
            UpdateCollision(sprites, Gravity, gameTime);
        }
    }
}
