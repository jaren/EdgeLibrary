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

namespace EdgeLibrary.Edge
{
    /// <summary>
    /// An emmiter of particles.
    /// </summary>
    public class EParticleEmitter : ESprite
    {
        public bool ShouldEmit { get; set; }
        public ERangeArray EmitPositionVariance { get; set; }
        public ERangeArray ColorVariance { get; set; }
        public ERangeArray VelocityVariance { get; set; }
        public ERangeArray SizeVariance { get; set; }
        public float GrowSpeed { get; set; }
        public ERange StartRotationVariance { get; set; }
        public ERange RotationSpeedVariance { get; set; }
        public ERange LifeVariance { get; set; }
        public float EmitWait { get; set; }
        public BlendState DrawState { get; set; }
        public int MaxParticles { get; set; }
        public EAction ActionToRunOnParticles { get; set; }

        protected List<EParticle> particles;
        protected EElement clampedObject;
        protected Vector2 clampPos;
        protected TimeSpan timeSinceLastEmit;
        //To stop garbage collection every single time
        protected List<EParticle> particlesToRemove;
        protected const int maxParticlesToRemove = 30;

        protected Random random;

        public delegate void ParticleEventHandler(EParticleEmitter sender);
        public event ParticleEventHandler OnEmit;

        public EParticleEmitter(string eTextureName, Vector2 ePosition) : base(eTextureName, ePosition, 0, 0) 
        {
            ActionToRunOnParticles = new EAction();

            particles = new List<EParticle>();

            EmitPositionVariance = new ERangeArray(new ERange(0, 0), new ERange(0, 0));
            ColorVariance = new ERangeArray(new ERange(0, 0), new ERange(0, 0), new ERange(0, 0));
            VelocityVariance = new ERangeArray(new ERange(0, 0), new ERange(0, 0));
            SizeVariance = new ERangeArray(new ERange(0, 0), new ERange(0, 0));
            GrowSpeed = 0;
            StartRotationVariance = new ERange(0, 0);
            RotationSpeedVariance = new ERange(0, 0);
            LifeVariance = new ERange( 0, 0);
            EmitWait = 9999999;

            MaxParticles = 10000;

            particlesToRemove = new List<EParticle>();

            DrawState = BlendState.Additive;

            clampPos = Vector2.Zero;


            timeSinceLastEmit = TimeSpan.Zero;

            random = new Random();
        }

        public EParticleEmitter(string eTextureName, Vector2 ePosition, ERangeArray eEmitPositionVariance, ERangeArray eColorVariance, ERangeArray eVelocityVariance, ERangeArray eSizeVariance, float eGrowSpeed, ERange eStartRotationVariance, ERange eRotationSpeedVariance, ERange eLifeVariance, float eEmitRate) : this(eTextureName, ePosition)
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

        public void clampTo(EElement eElement)
        {
            clampedObject = eElement;
        }

        public void clampToAt(EElement eElement, Vector2 eClampPos)
        {
            clampedObject = eElement;
            clampPos = eClampPos;
        }

        public void unclampFromObject() { clampedObject = null; }

        public void EmitSingleParticle()
        {
                EParticle particle = new EParticle(LifeVariance.GetRandom(random), new Vector2(VelocityVariance.GetRandom(0, random), VelocityVariance.GetRandom(1, random)), RotationSpeedVariance.GetRandom(random), GrowSpeed);
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

        public override void drawElement(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.FrontToBack, DrawState);

            foreach (EParticle particle in particles)
            {
                if (!particlesToRemove.Contains(particle))
                {
                    particle.Draw(spriteBatch, gameTime);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin();
        }

        public override void updateElement(EUpdateArgs updateArgs)
        {
                timeSinceLastEmit += updateArgs.gameTime.ElapsedGameTime;

                if (clampedObject != null)
                {
                    Position = clampedObject.Position + clampPos;
                }

                if (timeSinceLastEmit.TotalMilliseconds >= EmitWait)
                {
                    timeSinceLastEmit = new TimeSpan(0);
                    if (ShouldEmit) { EmitSingleParticle(); }
                }

                foreach (EParticle particle in particles)
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
                    foreach (EParticle particle in particlesToRemove)
                    {
                        particles.Remove(particle);
                    }
                    particlesToRemove.Clear();
                }

                if (particles.Count >= MaxParticles)
                {
                    particles.Clear();
                }

                if (ClampedToMouse) { Position = new Vector2(updateArgs.mouseState.X, updateArgs.mouseState.Y); }
            }
    }
}
