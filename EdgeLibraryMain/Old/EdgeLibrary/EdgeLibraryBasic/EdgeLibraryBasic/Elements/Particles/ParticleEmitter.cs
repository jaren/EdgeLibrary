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
        public bool ShouldEmit { get; set; }
        public RangeArray EmitPositionVariance { get; set; }
        public RangeArray ColorVariance { get; set; }
        public RangeArray VelocityVariance { get; set; }
        public RangeArray SizeVariance { get; set; }
        public float GrowSpeed { get; set; }
        public Range StartRotationVariance { get; set; }
        public Range RotationSpeedVariance { get; set; }
        public Range LifeVariance { get; set; }
        public float EmitWait { get; set; }
        public BlendState DrawState { get; set; }
        public int MaxParticles { get; set; }
        public EAction ActionToRunOnParticles { get; set; }

        protected List<Particle> particles;
        protected TimeSpan timeSinceLastEmit;
        //To stop garbage collection every single time
        protected List<Particle> particlesToRemove;
        protected const int maxParticlesToRemove = 30;

        protected Random random;

        public delegate void ParticleEventHandler(ParticleEmitter sender);
        public event ParticleEventHandler OnEmit;

        public ParticleEmitter(string eTextureName, Vector2 ePosition) : base(eTextureName, ePosition, 0, 0) 
        {
            ActionToRunOnParticles = new EAction();

            particles = new List<Particle>();

            EmitPositionVariance = new RangeArray(new Range(0, 0), new Range(0, 0));
            ColorVariance = new RangeArray(new Range(0, 0), new Range(0, 0), new Range(0, 0));
            VelocityVariance = new RangeArray(new Range(0, 0), new Range(0, 0));
            SizeVariance = new RangeArray(new Range(0, 0), new Range(0, 0));
            GrowSpeed = 0;
            StartRotationVariance = new Range(0, 0);
            RotationSpeedVariance = new Range(0, 0);
            LifeVariance = new Range( 0, 0);
            EmitWait = 9999999;

            MaxParticles = 10000;

            particlesToRemove = new List<Particle>();

            DrawState = BlendState.Additive;

            clampPos = Vector2.Zero;


            timeSinceLastEmit = TimeSpan.Zero;

            random = new Random();
        }

        public ParticleEmitter(string eTextureName, Vector2 ePosition, RangeArray eEmitPositionVariance, RangeArray eColorVariance, RangeArray eVelocityVariance, RangeArray eSizeVariance, float eGrowSpeed, Range eStartRotationVariance, Range eRotationSpeedVariance, Range eLifeVariance, float eEmitRate) : this(eTextureName, ePosition)
        {
            EmitPositionVariance = eEmitPositionVariance;
            ColorVariance = eColorVariance;
            VelocityVariance = eVelocityVariance;
            SizeVariance = eSizeVariance;
            GrowSpeed = eGrowSpeed;
            StartRotationVariance = eStartRotationVariance;
            RotationSpeedVariance = eRotationSpeedVariance;
            LifeVariance = eLifeVariance;
            EmitWait = eEmitRate;
        }

        public void EmitSinglParticle()
        {
                Particle particle = new Particle(LifeVariance.GetRandom(random), new Vector2(VelocityVariance.GetRandom(0, random), VelocityVariance.GetRandom(1, random)), RotationSpeedVariance.GetRandom(random), GrowSpeed);
                particle.Rotation = StartRotationVariance.GetRandom(random);
                particle.Position = new Vector2(Position.X + EmitPositionVariance.GetRandom(0, random), Position.Y + EmitPositionVariance.GetRandom(1, random));
                particle.Texture = Texture;
                particle.Height = (int)SizeVariance.GetRandom(0, random);
                particle.Width = (int)SizeVariance.GetRandom(0, random);
                particle.Color = Color.Transparent;
                particle.Color.R = (byte)ColorVariance.GetRandom(0, random);
                particle.Color.G = (byte)ColorVariance.GetRandom(1, random);
                particle.Color.B = (byte)ColorVariance.GetRandom(2, random);
                particle.Color.A = (byte)ColorVariance.GetRandom(3, random);
                particle.runAction(ActionToRunOnParticles);
                particles.Add(particle);

                if (OnEmit != null)
                {
                    OnEmit(this);
                }
        }

        public override void drawElement(GameTime gameTime)
        {
            EdgeGame.EndSpriteBatch();
            EdgeGame.BeginSpriteBatch(SpriteSortMode.FrontToBack, DrawState);

            foreach (Particle particle in particles)
            {
                if (!particlesToRemove.Contains(particle))
                {
                    particle.Draw(gameTime);
                }
            }

            EdgeGame.EndSpriteBatch();
            EdgeGame.BeginSpriteBatch();
        }

        public override void updatElement(UpdateArgs updateArgs)
        {
                timeSinceLastEmit += updateArgs.gameTime.ElapsedGameTime;

                if (timeSinceLastEmit.TotalMilliseconds >= EmitWait)
                {
                    timeSinceLastEmit = new TimeSpan(0);
                    if (ShouldEmit) { EmitSinglParticle(); }
                }

                foreach (Particle particle in particles)
                {
                    if (!particlesToRemove.Contains(particle))
                    {
                        particle.Update(updateArgs);
                        if (particle.shouldRemove)
                        {
                            particlesToRemove.Add(particle);
                        }
                    }
                }

                if (particlesToRemove.Count >= maxParticlesToRemove)
                {
                    foreach (Particle particle in particlesToRemove)
                    {
                        particles.Remove(particle);
                    }
                    particlesToRemove.Clear();
                }

                while (particles.Count > MaxParticles)
                {
                    particles.RemoveAt(0);
                }
            }
    }
}
