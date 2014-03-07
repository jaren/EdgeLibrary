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
        public Color MinStartColor;
        public Color MaxStartColor;
        public Color MinFinishColor;
        public Color MaxFinishColor;
        public Vector2 MinVelocity;
        public Vector2 MaxVelocity;
        public Vector2 MinSize;
        public Vector2 MaxSize;
        public float GrowSpeed;
        public float MaxStartRotation;
        public float MinStartRotation;
        public float MaxRotationSpeed;
        public float MinRotationSpeed;
        public float MaxLife;
        public float MinLife;
        public float EmitWait;
        public int MaxParticles;

        protected List<Particle> particles;
        protected TimeSpan timeSinceLastEmit;

        //To stop garbage collection every single time
        protected List<Particle> particlesToRemove;
        protected const int maxParticlesToRemove = 30;

        public delegate void ParticleEventHandler(ParticleEmitter sender);
        public event ParticleEventHandler OnEmit;

        public ParticleEmitter(string eTextureName, Vector2 ePosition) : this(MathTools.RandomID(), eTextureName, ePosition) { }

        public ParticleEmitter(string id, string eTextureName, Vector2 ePosition) : base(eTextureName, ePosition)
        {
            particles = new List<Particle>();

            MinStartColor = Color.White;
            MaxStartColor = MinStartColor;
            MinFinishColor = MinStartColor;
            MaxFinishColor = MaxStartColor;
            MinVelocity = -Vector2.One;
            MaxVelocity = Vector2.One*2;
            MinSize = new Vector2(Texture.Width, Texture.Height);
            MaxSize = MinSize;
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

        public void SetStartColor(Color c)
        {
            MinStartColor = c;
            MaxStartColor = c;
        }
        public void SetFinishColor(Color c)
        {
            MinFinishColor = c;
            MaxFinishColor = c;
        }
        public void SetVelocity(Vector2 v)
        {
            MinVelocity = v;
            MaxVelocity = v;
        }
        public void SetSize(Vector2 s)
        {
            MinSize = s;
            MaxSize = s;
        }
        public void SetRotation(float r)
        {
            MinStartRotation = r;
            MaxStartRotation = r;
        }
        public void SetRotationSpeed(float r)
        {
            MinRotationSpeed = r;
            MaxRotationSpeed = r;
        }
        public void SetLife(float l)
        {
            MinLife = l;
            MaxLife = l;
        }
        public void SetWidthHeight(int width, int height)
        {
           _width = width;
           _height = height;
        }

        public void EmitSingleParticle()
        {
            Particle particle = new Particle(MathTools.RandomID("particle"), "", InputManager.RandomFloat(MinLife, MaxLife), InputManager.RandomFloat(MinRotationSpeed, MaxRotationSpeed), GrowSpeed);
            particle.REMOVE();

            //It sets the velocity Y in a separate line to prevent the X and Y from being the same "random" number
            particle.velocity = new Vector2(InputManager.AccurateRandomInt((int)MinVelocity.X, (int)MaxVelocity.X), InputManager.AccurateRandomInt((int)MinVelocity.Y, (int)MaxVelocity.Y));

            if (InputManager.RandomInt(1, 3) == 2)
            {
                particle.velocity += new Vector2((float)InputManager.RandomDouble());
            }
            else
            {
                particle.velocity -= new Vector2((float)InputManager.RandomDouble());
            }
            particle.Texture = Texture;
            particle.CollisionBody = null;
            particle.Position = new Vector2(InputManager.RandomInt((int)(Position.X - Width / 2), (int)(Position.X + Width / 2)), InputManager.RandomInt((int)(Position.Y - Height / 2), (int)(Position.Y + Height / 2)));
            particle.Style.Rotation = InputManager.RandomInt((int)MinStartRotation, (int)MaxStartRotation);
            particle.Height = InputManager.RandomInt((int)MinSize.Y, (int)MaxSize.Y);
            particle.Width = InputManager.RandomInt((int)MinSize.X, (int)MaxSize.X);
            particle.StartColor = Color.Lerp(MinStartColor, MaxStartColor, (float)InputManager.RandomDouble());
            particle.FinishColor = Color.Lerp(MinFinishColor, MaxFinishColor, (float)InputManager.RandomDouble());

            particles.Add(particle);

            if (OnEmit != null)
            {
                OnEmit(this);
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
                EmitSingleParticle();
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
            TextureTools.DrawHollowRectangleAt(GetBoundingBox(), color, 1);
        }
    }
}
