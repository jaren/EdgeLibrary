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
    /// Emits particles (sprites) which are drawn and updated through the particle emitter
    /// </summary>
    public class ParticleEmitter : Sprite
    {
        //Sets the min and max value for both variables at the same time
        public ColorChangeIndex ColorIndex { set { MinColorIndex = value; MaxColorIndex = value; } }
        public Vector2 Velocity { set { MaxVelocity = value; MinVelocity = value; } }
        public new Vector2 Scale { set { MinScale = value; MaxScale = value; } }
        public float StartRotation { set { MaxStartRotation = value; MinStartRotation = value; } }
        public float RotationSpeed { set { MinRotationSpeed = value; MaxRotationSpeed = value; } }
        public float Life { set { MinLife = value; MaxLife = value; } }
        public double EmitWait { set { MinEmitWait = value; MaxEmitWait = value; } }
        public Vector2 EmitPositionVariance { set { MinEmitPositionVariance = value; MaxEmitPositionVariance = value; } }
        public int ParticlesToEmit { set { MinParticlesToEmit = value; MaxParticlesToEmit = value; } }
        public bool ShouldEmit { get; set; }

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
        public double MinEmitWait { get { return Ticker.MinMilliseconds; } set { Ticker.MinMilliseconds = value; } }
        public double MaxEmitWait { get { return Ticker.MaxMilliseconds; } set { Ticker.MaxMilliseconds = value; } }
        //The maximum number of particles that can exist at once
        public int MaxParticles;
        //The variance in emit position
        public Vector2 MinEmitPositionVariance;
        public Vector2 MaxEmitPositionVariance;
        //The number of particles to emit each count
        public int MinParticlesToEmit;
        public int MaxParticlesToEmit;

        //If set to true, generates the same X, Y, and Z coordinates for the property
        public bool CommonRotation;
        public bool CommonRotationSpeed;
        public bool CommonScale;

        protected RandomTicker Ticker;

        protected List<Sprite> Particles;
        protected double elapsedSinceEmit;
        protected double currentEmitWait;

        //To stop garbage collection every single time
        protected List<Sprite> particlesToRemove;
        protected const int maxParticlesToRemove = 30;

        //Particle emitter event called when a particle is emitted
        public delegate void ParticleEventHandler(ParticleEmitter sender, Sprite particle, GameTime gameTime);
        public event ParticleEventHandler OnEmit = delegate { };

        public ParticleEmitter(string textureName, Vector2 position)
            : base(textureName, position)
        {
            Particles = new List<Sprite>();
            Ticker = new RandomTicker(0);
            Ticker.OnTick += new RandomTicker.TickerEventHandler(Ticker_OnTick);

            CommonRotation = false;
            CommonRotationSpeed = false;
            CommonScale = true;

            ShouldEmit = true;

            ColorIndex = new ColorChangeIndex(2000, Color.White, Color.Transparent);
            MinVelocity = -Vector2.One;
            MaxVelocity = Vector2.One * 2;
            Scale = Vector2.One;
            GrowSpeed = 0;
            StartRotation = 0;
            RotationSpeed = 0;
            EmitPositionVariance = Vector2.Zero;
            Life = 2000;
            ParticlesToEmit = 1;
            EmitWait = 10;
            MaxParticles = 10000;

            currentEmitWait = RandomTools.RandomDouble(MinEmitWait, MaxEmitWait);

            particlesToRemove = new List<Sprite>();
            elapsedSinceEmit = 0;
        }

        /// <summary>
        /// Clears all the particles from the emitter
        /// </summary>
        public void ClearParticles()
        {
            Particles.Clear();
        }

        /// <summary>
        /// Emits a single particle by generating random values between the Min and Max
        /// </summary>
        public void EmitSingleParticle(GameTime gameTime)
        {
            Sprite particle = new Sprite("", Vector2.Zero);
            particle.Texture = Texture;

            //The data is used to store life time
            particle.Data.Add("LivedTime", "0");
            particle.Data.Add("LifeTime", RandomTools.RandomFloat(MinLife, MaxLife).ToString());

            //Generates a random velocity
            particle.Data.Add("Velocity", RandomTools.RandomFloat(MinVelocity.X, MaxVelocity.X) + "," + RandomTools.RandomFloat(MinVelocity.Y, MaxVelocity.Y));

            //Generates a random rotation speed
            particle.Data.Add("RotationSpeed", RandomTools.RandomFloat(MinRotationSpeed, MaxRotationSpeed).ToString());

            //Generates a random color change index
            particle.AddAction("Color", new AColorChange(ColorChangeIndex.Lerp(MinColorIndex, MaxColorIndex, RandomTools.RandomFloat(0, 1))));

            //Generates a random EmitPositionVariance to be used in the position
            Vector2 randomEmitPositionVariance = new Vector2(RandomTools.RandomFloat(MinEmitPositionVariance.X, MinEmitPositionVariance.X),
                RandomTools.RandomFloat(MinEmitPositionVariance.Y, MinEmitPositionVariance.Y));

            //Generates a random position
            particle.Position = new Vector2(RandomTools.RandomFloat(Position.X - randomEmitPositionVariance.X / 2f, Position.X + randomEmitPositionVariance.X / 2f),
                RandomTools.RandomFloat(Position.Y - randomEmitPositionVariance.Y / 2f, Position.Y + randomEmitPositionVariance.Y / 2f));

            //Generates a random rotation
            particle.Rotation = RandomTools.RandomFloat(MinStartRotation, MaxStartRotation);

            //Generates a random scale
            particle.Scale = new Vector2(RandomTools.RandomFloat(MinScale.X, MaxScale.X),
                RandomTools.RandomFloat(MinScale.Y, MaxScale.Y));

            //Checks if the particles' properties should have the same X, Y, and Z
            if (CommonRotationSpeed)
            {
                float rotation = RandomTools.RandomFloat(MinRotationSpeed, MaxRotationSpeed);
                ARotate rotate = particle.Action<ARotate>("Rotation");
                rotate = new ARotate(rotation);
            }
            if (CommonScale)
            {
                particle.Scale = new Vector2(particle.Scale.X);
            }

            Particles.Add(particle);

            OnEmit(this, particle, gameTime);
        }

        /// <summary>
        /// When the ticker has finished, emit particles
        /// </summary>
        protected void Ticker_OnTick(GameTime gameTime)
        {
            int p = RandomTools.RandomInt(MinParticlesToEmit, MaxParticlesToEmit);
            for (int i = 0; i < p; i++)
            {
                EmitSingleParticle(gameTime);
            }
        }

        /// <summary>
        /// Checks which particle should be removed and updates particles
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (ShouldEmit)
            {
                Ticker.Update(gameTime);
            }

            //Checks if each particle should be removed
            foreach (Sprite particle in Particles)
            {
                if (!particlesToRemove.Contains(particle))
                {
                    particle.Update(gameTime);

                    if (!particle.PhysicsEnabled)
                    {
                        //Moves the particle
                        particle.Position += new Vector2(float.Parse(particle.Data["Velocity"].Split(',')[0]), float.Parse(particle.Data["Velocity"].Split(',')[1]));

                        //Rotates the particle
                        particle.Rotation += float.Parse(particle.Data["RotationSpeed"]);
                    }

                    //Increments the LivedTime and checks if the particle should be removed
                    particle.Data["LivedTime"] = (double.Parse(particle.Data["LivedTime"]) + gameTime.ElapsedGameTime.TotalMilliseconds * EdgeGame.GameSpeed).ToString();
                    if (double.Parse(particle.Data["LivedTime"]) > double.Parse(particle.Data["LifeTime"]))
                    {
                        if (particle.PhysicsEnabled && EdgeGame.World.BodyList.Contains(Body))
                        {
                            EdgeGame.World.RemoveBody(Body);
                        }
                        particlesToRemove.Add(particle);
                        particle.Disable();
                    }
                }
            }

            //Possibly better way - set particle.ShouldBeRemoved to be true: Particles.RemoveAll(p => p.ShouldBeRemoved);

            //If enough particles should be removed, delete them
            if (particlesToRemove.Count >= maxParticlesToRemove)
            {
                foreach (Sprite particle in particlesToRemove)
                {
                    Particles.Remove(particle);
                }
                particlesToRemove.Clear();
            }

            //Needed for running actions
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws all the particles that are not going to be removed
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            RestartSpriteBatch();

            foreach (Sprite particle in Particles)
            {
                if (!particlesToRemove.Contains(particle))
                {
                    particle.Draw(gameTime);
                }
            }

            RestartSpriteBatch();
        }

        public override object Clone()
        {
            ParticleEmitter clone = (ParticleEmitter)base.Clone();
            clone.Particles = new List<Sprite>();
            foreach (Sprite particle in Particles)
            {
                clone.Particles.Add((Sprite)particle.Clone());
            }
            return clone;
        }
    }
}
