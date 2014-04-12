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
        //Sets the min and max value for both variables at the same time
        public ColorChangeIndex ColorIndex { set { MinColorIndex = value; MaxColorIndex = value; } }
        public Vector2 Velocity { set { MaxVelocity = value; MinVelocity = value; } }
        public override Vector2 Scale { set { MinScale = value; MaxScale = value; } }
        public float StartRotation { set { MaxStartRotation = value; MinStartRotation = value; } }
        public float RotationSpeed { set { MinRotationSpeed = value; MaxRotationSpeed = value; } }
        public float Life { set { MinLife = value; MaxLife = value; } }
        public double EmitWait { set { MinEmitWait = value; MaxEmitWait = value; } }
        public Vector2 EmitArea { get { return new Vector2(EmitWidth, EmitHeight); } set { EmitWidth = value.X; EmitHeight = value.Y; } }
        public int ParticlesToEmit { set { MinParticlesToEmit = value; MaxParticlesToEmit = value; } }

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
        public double MinEmitWait;
        public double MaxEmitWait;
        //The maximum number of particles that can exist at once
        public int MaxParticles;
        //Generates particles with the same width and height
        public bool SquareParticles;
        //The variance in emit position
        public float EmitWidth;
        public float EmitHeight;
        //The number of particles to emit each count
        public int MinParticlesToEmit;
        public int MaxParticlesToEmit;

        //This can't be SubElements because the particles' removal must be handled by the ParticleEmitter
        protected List<Particle> Particles;
        protected double timeSinceLastEmit;
        protected double currentEmitWait;

        //To stop garbage collection every single time
        protected List<Particle> particlesToRemove;
        protected const int maxParticlesToRemove = 30;

        //Particle emitter event called when a particle is emitted
        public delegate void ParticleEventHandler(ParticleEmitter sender, Particle particle, GameTime gameTime);
        public event ParticleEventHandler OnEmit = delegate { };

        public ParticleEmitter(string textureName, Vector2 position) : base(textureName, position)
        {
            Particles = new List<Particle>();

            SquareParticles = true;

            ColorIndex = new ColorChangeIndex(Color.White);
            MinVelocity = -Vector2.One;
            MaxVelocity = Vector2.One * 2;
            Scale = Vector2.One;
            GrowSpeed = 0;
            StartRotation = 0;
            RotationSpeed = 0;
            Life = 1000;
            ParticlesToEmit = 1;
            EmitWait = 10;
            MaxParticles = 10000;

            currentEmitWait = RandomTools.RandomDouble(MinEmitWait, MaxEmitWait);

            particlesToRemove = new List<Particle>();
            timeSinceLastEmit = 0;
        }

        //Clears all the particles from the emitter
        public void ClearParticles()
        {
            Particles.Clear();
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
            particle.ColorIndex = ColorChangeIndex.Lerp(MinColorIndex, MaxColorIndex, RandomTools.RandomFloat(0, 1));

            if (SquareParticles)
            {
                particle.Scale = new Vector2(particle.Scale.X, particle.Scale.X);
            }
            particle.ColorIndex = ColorChangeIndex.Lerp(MinColorIndex, MaxColorIndex, RandomTools.RandomFloat(0, 1));

            Particles.Add(particle);

            OnEmit(this, particle, gameTime);
        }

        protected override void DrawObject(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Particle particle in Particles)
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
            timeSinceLastEmit += gameTime.ElapsedGameTime.TotalMilliseconds;

            //If the elapsed time is greater than the EmitWait, emit a particle
            if (timeSinceLastEmit >= currentEmitWait && Particles.Count < MaxParticles)
            {
                timeSinceLastEmit = 0;
                currentEmitWait = RandomTools.RandomDouble(MinEmitWait, MaxEmitWait);
                int p = RandomTools.RandomInt(MinParticlesToEmit, MaxParticlesToEmit);
                for (int i = 0; i < p; i++)
                {
                    EmitSingleParticle(gameTime);
                }
            }

            //Checks if each particle should be removed
            foreach (Particle particle in Particles)
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
                    Particles.Remove(particle);
                }
                particlesToRemove.Clear();
            }


            base.UpdateObject(gameTime);
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

        public override Element Clone()
        {
            ParticleEmitter clone = (ParticleEmitter)base.Clone();
            clone.Particles = new List<Particle>();
            foreach (Particle particle in Particles)
            {
                clone.Particles.Add((Particle)particle.Clone());
            }
            return clone;
        }
    }
}
