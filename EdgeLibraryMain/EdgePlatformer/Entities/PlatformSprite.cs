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
    //The base for all platform sprites
    public class PlatformSprite : Sprite
    {
        protected class Force
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

        public Vector2 gravity;
        public float Acceleration;
        public float Deceleration;
        public float MaxVelocity;

        protected List<Force> forces;

        protected float fallSpeed;

        public bool collidingXGreater; //Colliding left
        public bool collidingXLower; //Colliding right
        public bool collidingYGreater; //Colliding on the bottom (appears as top)
        public bool collidingYLower; //Colliding on the top (appears as bottom)

        public new virtual event CollisionEvent Collision;

        public PlatformSprite(string eTextureName, Vector2 ePosition) : this(MathTools.RandomID(typeof(PlatformSprite)), eTextureName, ePosition) { }

        public PlatformSprite(string id, string eTextureName, Vector2 ePosition) : base(id, eTextureName, ePosition)
        {
            Acceleration = 0.1f;
            Deceleration = 0.06f;
            MaxVelocity = 15;
            forces = new List<Force>();
        }

        protected override void UpdateCollision(GameTime gameTime)
        {
            collidingXGreater = false;
            collidingXLower = false;
            collidingYGreater = false;
            collidingYLower = false;

            foreach (Element element in EdgeGame.SelectedScene.elements)
            {
                if (element is PlatformSprite)
                {
                    PlatformSprite sprite = (PlatformSprite)element;
                    if (CollisionBody != null && sprite.CollisionBody != null && sprite != this)
                    {
                        if (CollisionBody.CheckForCollide(sprite.CollisionBody))
                        {
                            if (Collision != null)
                            {
                                Collision(this, sprite, gameTime);
                            }

                            Rectangle collision = CollisionBody.Intersect(CollisionBody, sprite.CollisionBody);

                            //If it's collided in horizontally more than vertical
                            if (Math.Abs(collision.Width) > Math.Abs(collision.Height))
                            {
                                //When collided into something, acceleration is reset
                                fallSpeed = 0;

                                if (Position.Y > sprite.Position.Y)
                                {
                                    Position = new Vector2(Position.X, Position.Y + collision.Height);
                                    collidingYGreater = true;
                                }
                                else
                                {
                                    Position = new Vector2(Position.X, Position.Y - collision.Height);
                                    collidingYLower = true;
                                }
                            }
                            else
                            {
                                if (Position.X > sprite.Position.X)
                                {
                                    Position = new Vector2(Position.X + collision.Width, Position.Y);
                                    collidingXGreater = true;
                                }
                                else
                                {
                                    Position = new Vector2(Position.X - collision.Width, Position.Y);
                                    collidingXLower = true;
                                }
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

        public bool TryMove(Vector2 vector)
        {
            vector = StopMaxVector2(vector);

            if ((vector.X < 0 && !collidingXGreater) || (vector.X > 0 && !collidingXLower))
            {
                Position = new Vector2(Position.X + vector.X, Position.Y);
                return true;
            }

            if ((vector.Y < 0 && !collidingYGreater) || (vector.Y > 0 && !collidingYLower))
            {
                Position = new Vector2(Position.X, Position.Y + vector.Y);
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

        public virtual void UpdateForces()
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

            if (!collidingYGreater)
            {
                fallSpeed += Acceleration / 10;
                Vector2 vector = StopMaxVector2(gravity * fallSpeed);
                Position -= vector;
            }
        }

        protected override void updateElement(GameTime gameTime)
        {
            UpdateForces();
            UpdateCollision(gameTime);
        }
    }
}
