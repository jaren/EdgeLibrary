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
        public BlendState DrawState;
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
            DrawState = BlendState.Additive;
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
           reloadBoundingBox();
        }

        public void EmitSingleParticle()
        {
            Particle particle = new Particle(MathTools.RandomID("particle"), "", InputManager.Random.Next((int)MinLife, (int)MaxLife), new Vector2(InputManager.Random.Next((int)MinVelocity.X, (int)MaxVelocity.X), InputManager.Random.Next((int)MinVelocity.Y, (int)MaxVelocity.Y)), InputManager.Random.Next((int)MinRotationSpeed, (int)MaxRotationSpeed), GrowSpeed);
            particle.REMOVE();
            if (InputManager.Random.Next(1, 3) == 2)
            {
                particle.velocity += new Vector2((float)InputManager.Random.NextDouble());
            }
            else
            {
                particle.velocity -= new Vector2((float)InputManager.Random.NextDouble());
            }
            particle.Texture = Texture;
            particle.CollisionBody = null;
            particle.Position = new Vector2(InputManager.Random.Next(BoundingBox.Left, BoundingBox.Right), InputManager.Random.Next(BoundingBox.Top, BoundingBox.Bottom));
            particle.Style.Rotation = InputManager.Random.Next((int)MinStartRotation, (int)MaxStartRotation);
            particle.Height = InputManager.Random.Next((int)MinSize.Y, (int)MaxSize.Y);
            particle.Width = InputManager.Random.Next((int)MinSize.X, (int)MaxSize.X);
            particle.StartColor = Color.Transparent;
            particle.StartColor.R = (byte)InputManager.Random.Next(Math.Min(MinStartColor.R, MaxStartColor.R), Math.Max(MinStartColor.R, MaxStartColor.R));
            particle.StartColor.G = (byte)InputManager.Random.Next(Math.Min(MinStartColor.G, MaxStartColor.G), Math.Max(MinStartColor.G, MaxStartColor.G));
            particle.StartColor.B = (byte)InputManager.Random.Next(Math.Min(MinStartColor.B, MaxStartColor.B), Math.Max(MinStartColor.B, MaxStartColor.B));
            particle.StartColor.A = (byte)InputManager.Random.Next(Math.Min(MinStartColor.A, MaxStartColor.A), Math.Max(MinStartColor.A, MaxStartColor.A));

            particle.FinishColor = Color.Transparent;
            particle.FinishColor.R = (byte)InputManager.Random.Next(Math.Min(MinFinishColor.R, MaxFinishColor.R), Math.Max(MinFinishColor.R, MaxFinishColor.R));
            particle.FinishColor.G = (byte)InputManager.Random.Next(Math.Min(MinFinishColor.G, MaxFinishColor.G), Math.Max(MinFinishColor.G, MaxFinishColor.G));
            particle.FinishColor.B = (byte)InputManager.Random.Next(Math.Min(MinFinishColor.B, MaxFinishColor.B), Math.Max(MinFinishColor.B, MaxFinishColor.B));
            particle.FinishColor.A = (byte)InputManager.Random.Next(Math.Min(MinFinishColor.A, MaxFinishColor.A), Math.Max(MinFinishColor.A, MaxFinishColor.A));

            particles.Add(particle);

            if (OnEmit != null)
            {
                OnEmit(this);
            }
        }

        protected override void drawElement(GameTime gameTime)
        {
            EdgeGame.RestartSpriteBatch(SpriteSortMode.Deferred, DrawState);

            foreach (Particle particle in particles)
            {
                if (!particlesToRemove.Contains(particle))
                {
                    particle.Draw(gameTime);
                }
            }
            
            EdgeGame.RestartSpriteBatch();
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
            TextureTools.DrawHollowRectangleAt(BoundingBox, color, 1);
        }
    }
}
