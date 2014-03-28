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

namespace EdgeLibrary
{
    /// <summary>
    /// An emmiter of particles.
    /// </summary>
    public class ParticleEmitter : Sprite
    {
        //The color for the particles
        public ColorChangeIndex MinColorIndex;
        public ColorChangeIndex MaxColorIndex;
        //The velocity that the particles will move at
        public Vector2 MinVelocity;
        public Vector2 MaxVelocity;
        //The scale of the particles
        public Vector2 MinScale;
        public Vector2 MaxScale;
        //How quickly the particles will grow
        public float GrowSpeed;
        //What the particles starting rotation will be
        public float MaxStartRotation;
        public float MinStartRotation;
        //How quickly the particles will rotate
        public float MaxRotationSpeed;
        public float MinRotationSpeed;
        //How long the particles will remain updating/drawing
        public float MaxLife;
        public float MinLife;
        //How long the emitter waits before emitting a particle
        public float EmitWait;
        //The maximum number of particles that can exist at once
        public int MaxParticles;
        //Generates particles with the same width and height
        public bool SquareParticles;
        //The variance in emit position
        public float EmitWidth;
        public float EmitHeight;

        protected List<Particle> particles;
        protected TimeSpan timeSinceLastEmit;

        //To stop garbage collection every single time
        protected List<Particle> particlesToRemove;
        protected const int maxParticlesToRemove = 30;

        //Particle emitter event called when a particle is emitted
        public delegate void ParticleEventHandler(ParticleEmitter sender, Particle particle, GameTime gameTime);
        public event ParticleEventHandler OnEmit = delegate { };

        public ParticleEmitter(string textureName, Vector2 position) : base(textureName, position)
        {
            particles = new List<Particle>();

            SquareParticles = true;

            MinColorIndex = new ColorChangeIndex(Color.White);
            MaxColorIndex = MinColorIndex;
            MinVelocity = -Vector2.One;
            MaxVelocity = Vector2.One * 2;
            MinScale = Vector2.One;
            MaxScale = MinScale;
            GrowSpeed = 0;
            MinStartRotation = 0;
            MaxStartRotation = MinStartRotation;
            MinRotationSpeed = 0;
            MaxRotationSpeed = MinRotationSpeed;
            MinLife = 1000;
            MaxLife = MinLife;
            EmitWait = 0;
            MaxParticles = 10000;

            particlesToRemove = new List<Particle>();
            timeSinceLastEmit = TimeSpan.Zero;
        }

        //Sets the minimum and maximum color
        public void SetColor(Color color)
        {
            MinColorIndex = new ColorChangeIndex(color);
            MaxColorIndex = MinColorIndex;
        }
        public void SetColor(ColorChangeIndex color)
        {
            MinColorIndex = color;
            MaxColorIndex = MinColorIndex;
        }
        public void SetColor(Color color, Color color2)
        {
            MinColorIndex = new ColorChangeIndex(color);
            MaxColorIndex = new ColorChangeIndex(color2);
        }
        public void SetColor(ColorChangeIndex color, ColorChangeIndex color2)
        {
            MinColorIndex = color;
            MaxColorIndex = color2;
        }
        //Sets the minimum and maximum velocity
        public void SetVelocity(Vector2 v)
        {
            MinVelocity = v;
            MaxVelocity = v;
        }
        public void SetVelocity(Vector2 v, Vector2 v2)
        {
            MinVelocity = v;
            MaxVelocity = v2;
        }
        //Sets the minimum and maximum scale
        public void SetScale(Vector2 s)
        {
            MinScale = s;
            MaxScale = s;
        }
        public void SetScale(Vector2 s, Vector2 s2)
        {
            MinScale = s;
            MaxScale = s2;
        }
        //Sets the minimum and maximum start rotation
        public void SetRotation(float r)
        {
            MinStartRotation = r;
            MaxStartRotation = r;
        }
        public void SetRotation(float r, float r2)
        {
            MinStartRotation = r;
            MaxStartRotation = r2;
        }
        //Sets the minimum and maximum rotation speed
        public void SetRotationSpeed(float r)
        {
            MinRotationSpeed = r;
            MaxRotationSpeed = r;
        }
        public void SetRotationSpeed(float r, float r2)
        {
            MinRotationSpeed = r;
            MaxRotationSpeed = r2;
        }
        //Sets the minimum and maximum life time
        public void SetLife(float l)
        {
            MinLife = l;
            MaxLife = l;
        }
        public void SetLife(float l, float l2)
        {
            MinLife = l;
            MaxLife = l2;
        }
        //Sets the emit area
        public void SetEmitArea(float width, float height)
        {
            EmitWidth = width;
            EmitHeight = height;
        }
        public void SetEmitArea(Vector2 value)
        {
            EmitWidth = value.X;
            EmitHeight = value.Y;
        }

        //Clears all the particles from the emitter
        public void ClearParticles()
        {
            particles.Clear();
        }

        //Emits a single particle - can be called from outside the emitter
        public void EmitSingleParticle(GameTime gameTime)
        {
            Particle particle = new Particle(RandomTools.RandomFloat(MinLife, MaxLife), RandomTools.RandomFloat(MinRotationSpeed, MaxRotationSpeed), GrowSpeed);

            particle.Velocity = new Vector2(RandomTools.RandomFloat(MinVelocity.X, MaxVelocity.X), RandomTools.RandomFloat(MinVelocity.Y, MaxVelocity.Y));

            particle.Texture = Texture;
            particle.CollisionBody = null;
            particle.Position = new Vector2(RandomTools.RandomFloat(Position.X - EmitWidth / 2f, Position.X + EmitWidth / 2f), RandomTools.RandomFloat(Position.Y - EmitHeight / 2f, Position.Y + EmitHeight / 2f));
            particle.Rotation = RandomTools.RandomFloat(MinStartRotation, MaxStartRotation);
            particle.Scale = new Vector2(RandomTools.RandomFloat(MinScale.X, MaxScale.X), RandomTools.RandomFloat(MinScale.Y, MaxScale.Y));
            particle.ColorIndex = ColorChangeIndex.Lerp(MinColorIndex, MaxColorIndex, RandomTools.RandomFloat());

            if (SquareParticles)
            {
                particle.Scale = new Vector2(particle.Scale.X, particle.Scale.X);
            }
            particle.ColorIndex = ColorChangeIndex.Lerp(MinColorIndex, MaxColorIndex, RandomTools.RandomFloat());

            particles.Add(particle);

            OnEmit(this, particle, gameTime);
        }

        protected override void DrawObject(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Particle particle in particles)
            {
                if (!particlesToRemove.Contains(particle))
                {
                    particle.Draw(gameTime, spriteBatch);
                }
            }
        }

        //Updates all the particles in the emitter
        protected override void UpdateObject(GameTime gameTime)
        {
            timeSinceLastEmit += gameTime.ElapsedGameTime;

            //If the elapsed time is greater than the EmitWait, emit a particle
            if (timeSinceLastEmit.TotalMilliseconds >= EmitWait)
            {
                timeSinceLastEmit = new TimeSpan(0);
                EmitSingleParticle(gameTime);
            }

            //Checks if each particle should be removed
            foreach (Particle particle in particles)
            {
                if (!particlesToRemove.Contains(particle))
                {
                    particle.Update(gameTime);
                    if (particle.shouldRemove)
                    {
                        particlesToRemove.Add(particle);
                    }
                }
            }

            //If enough particles should be removed, delete them
            if (particlesToRemove.Count >= maxParticlesToRemove)
            {
                foreach (Particle particle in particlesToRemove)
                {
                    particles.Remove(particle);
                }
                particlesToRemove.Clear();
            }

            //Deletes particles until the number is less than the Maximum number of particles
            while (particles.Count > MaxParticles)
            {
                particles.RemoveAt(0);
            }
        }

        public override void  DrawDebug(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            Rectangle rectangle = new Rectangle((int)(Position.X - EmitWidth / 2), (int)(Position.Y - EmitHeight / 2), (int)EmitWidth, (int)EmitHeight);
            //Draws a box around where the rectangle is
            spriteBatch.Draw(Resources.GetTexture("Pixel"), new Rectangle(rectangle.Top, rectangle.Left, 1, rectangle.Height), color);
            spriteBatch.Draw(Resources.GetTexture("Pixel"), new Rectangle(rectangle.Top, rectangle.Right, 1, rectangle.Height), color);
            spriteBatch.Draw(Resources.GetTexture("Pixel"), new Rectangle(rectangle.Top, rectangle.Left, rectangle.Width, 1), color);
            spriteBatch.Draw(Resources.GetTexture("Pixel"), new Rectangle(rectangle.Bottom, rectangle.Left, rectangle.Width, 1), color);
        }
    }
}
