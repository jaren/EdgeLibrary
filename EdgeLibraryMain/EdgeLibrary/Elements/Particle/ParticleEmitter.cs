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
        public ColorChangeIndex MinColorIndex;
        public ColorChangeIndex MaxColorIndex;
        public Vector2 MinVelocity;
        public Vector2 MaxVelocity;
        public Vector2 MinScale;
        public Vector2 MaxScale;
        public float GrowSpeed;
        public float MaxStartRotation;
        public float MinStartRotation;
        public float MaxRotationSpeed;
        public float MinRotationSpeed;
        public float MaxLife;
        public float MinLife;
        public float EmitWait;
        public int MaxParticles;
        public bool SquareParticles;
        public float EmitWidth;
        public float EmitHeight;

        protected List<Particle> particles;
        protected TimeSpan timeSinceLastEmit;

        //To stop garbage collection every single time
        protected List<Particle> particlesToRemove;
        protected const int maxParticlesToRemove = 30;

        public delegate void ParticleEventHandler(ParticleEmitter sender, Particle particle, GameTime gameTime);
        public event ParticleEventHandler OnEmit;

        public ParticleEmitter(string eTextureName, Vector2 ePosition) : this(MathTools.RandomID(typeof(ParticleEmitter)), eTextureName, ePosition) { }

        public ParticleEmitter(string id, string eTextureName, Vector2 ePosition) : base(eTextureName, ePosition)
        {
            particles = new List<Particle>();

            SquareParticles = true;

            MinColorIndex = new ColorChangeIndex(Color.White);
            MaxColorIndex = MinColorIndex;
            MinVelocity = -Vector2.One;
            MaxVelocity = Vector2.One*2;
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

        public void ClearParticles()
        {
            particles.Clear();
        }

        public void EmitSingleParticle(GameTime gameTime)
        {
            Particle particle = new Particle(MathTools.RandomID(typeof(Particle)), "", RandomTools.RandomFloat(MinLife, MaxLife), RandomTools.RandomFloat(MinRotationSpeed, MaxRotationSpeed), GrowSpeed);
            
            //Should be unnecessary, but in case it was added anyways...
            particle.REMOVE();

            particle.velocity = new Vector2(RandomTools.RandomFloat(MinVelocity.X, MaxVelocity.X), RandomTools.RandomFloat(MinVelocity.Y, MaxVelocity.Y));

            particle.Texture = Texture;
            particle.CollisionBody = null;
            particle.Position = new Vector2(RandomTools.RandomFloat(Position.X - EmitWidth / 2f, Position.X + EmitWidth / 2f), RandomTools.RandomFloat(Position.Y - EmitHeight / 2f, Position.Y + EmitHeight / 2f));
            particle.Style.Rotation = RandomTools.RandomFloat(MinStartRotation, MaxStartRotation);
            particle.Scale = new Vector2(RandomTools.RandomFloat(MinScale.X, MaxScale.X), RandomTools.RandomFloat(MinScale.Y, MaxScale.Y));
            particle.ColorIndex = ColorChangeIndex.Lerp(MinColorIndex, MaxColorIndex, RandomTools.RandomFloat());
            particle.REMOVE();

            if (SquareParticles)
            {
                particle.Scale = new Vector2(particle.Scale.X, particle.Scale.X);
            }
            particle.ColorIndex = ColorChangeIndex.Lerp(MinColorIndex, MaxColorIndex, RandomTools.RandomFloat());

            particles.Add(particle);

            if (OnEmit != null)
            {
                OnEmit(this, particle, gameTime);
            }
        }

        protected override void drawElement(GameTime gameTime)
        {
            foreach (Particle particle in particles)
            {
                if (!particlesToRemove.Contains(particle))
                {
                    particle.Draw(gameTime);
                }
            }
        }

        protected override void updateElement(GameTime gameTime)
        {
            timeSinceLastEmit += gameTime.ElapsedGameTime;

            if (timeSinceLastEmit.TotalMilliseconds >= EmitWait)
            {
                timeSinceLastEmit = new TimeSpan(0);
                EmitSingleParticle(gameTime);
            }

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

        public override void DebugDraw(Color color)
        {
            TextureTools.DrawHollowRectangleAt(new Rectangle((int)(Position.X - EmitWidth/2), (int)(Position.Y - EmitHeight/2), (int)EmitWidth, (int)EmitHeight), color, 1);
        }
    }
}
