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
        protected struct Force
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
        public float MaxVelocity;

        protected List<Force> forces;

        protected float fallSpeed;

        public bool collidingUp;
        public bool collidingDown;
        public bool collidingLeft;
        public bool collidingRight;

        public new delegate void CollisionEvent(PlatformSprite sender, PlatformSprite sprite2, GameTime gameTime);
        public new virtual event CollisionEvent Collision;

        public PlatformSprite(string eTextureName, Vector2 ePosition) : this(MathTools.RandomID(), eTextureName, ePosition) { }

        public PlatformSprite(string id, string eTextureName, Vector2 ePosition) : base(id, eTextureName, ePosition)
        { 
            MarkedForRemoval = true;
            CollisionLayers = CollisionLayers.All;
            Acceleration = 0.1f;
            Deceleration = 0.06f;
            MaxVelocity = 15;
            forces = new List<Force>();

            if (EdgeGame.SelectedScene is PlatformLevel)
            {
                ((PlatformLevel)EdgeGame.SelectedScene).AddSprite(this);
            }
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
                    if (GetBoundingBox().Intersects(sprite.GetBoundingBox()))
                    {
                        if (Collision != null)
                        {
                            Collision(this, sprite, gameTime);
                        }

                        Rectangle collision = Rectangle.Intersect(GetBoundingBox(), sprite.GetBoundingBox());

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

        public Vector2 StopMaxVector2(Vector2 vector)
        {
            if (vector.X > 0 && vector.X > MaxVelocity)
            {
                vector = new Vector2(MaxVelocity, vector.Y);
            }
            else if (vector.X < 0 && Math.Abs(vector.X) > MaxVelocity)
            {
                vector = new Vector2(-MaxVelocity, vector.Y);
            }

            if (vector.Y > 0 && vector.Y > MaxVelocity)
            {
                vector = new Vector2(vector.X, MaxVelocity);
            }
            else if (vector.Y < 0 && Math.Abs(vector.Y) > MaxVelocity)
            {
                vector = new Vector2(vector.X, -MaxVelocity);
            }

            return vector;
        }

        public void REMOVEplatform()
        {
            MarkedForPlatformRemoval = true;
        }

        public bool TryMove(Vector2 vector)
        {
            vector = StopMaxVector2(vector);

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
            Vector2 forceTotal = Vector2.Zero;
            for (int i = 0; i < forces.Count; i++)
            {
                if (forces[i].ApplyForce())
                {
                    forceTotal += forces[i].Impulse;
                }
                else
                {
                    forces.Remove(forces[i]);
                    i--;
                }
            }
            TryMove(forceTotal);

            if (!collidingDown)
            {
                fallSpeed += Acceleration / 10;
                Vector2 vector = StopMaxVector2(Gravity * fallSpeed);
                Position -= vector;
            }
        }

        public virtual void UpdatePlatform(GameTime gameTime, Vector2 Gravity, List<PlatformSprite> sprites)
        {
            UpdateForces(Gravity);
            UpdateCollision(sprites, Gravity, gameTime);
        }
    }
}
