using EdgeLibrary;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class ParticleMenu : MenuBase
    {
        protected List<Sprite> PhysicsSprites;
        protected List<ParticleEmitter> PhysicsSpritesEmitters;
        protected float max = 100;
        protected Vector2 force;
        protected Vector2 point;
        protected bool clicking = false;

        ParticleEmitter Fire;

        public ParticleMenu(string name)
            : base(name)
        {
            Fire = new ParticleEmitter("Fire", new Vector2(500))
            {
                BlendState = BlendState.Additive,
                Life = 700,

                EmitPositionVariance = new Vector2(0, 0),

                MinVelocity = new Vector2(-3, -8),
                MaxVelocity = new Vector2(4, -5),

                MinScale = new Vector2(1f),
                MaxScale = new Vector2(2f),

                MinColorIndex = new ColorChangeIndex(200, Color.Magenta, Color.Orange, Color.Purple, Color.Transparent),
                MaxColorIndex = new ColorChangeIndex(200, Color.Teal, Color.OrangeRed, Color.DarkOrange, Color.Transparent),
                EmitWait = 0,
                ParticlesToEmit = 5,
                GrowSpeed = new Vector2(0.03f)
            };
            Components.Add(Fire);

            PhysicsSprites = new List<Sprite>();
            PhysicsSpritesEmitters = new List<ParticleEmitter>();

            Button screenButton = new Button("Pixel", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y / 2)) { Visible = false, Scale = new Vector2(EdgeGame.WindowSize.X, EdgeGame.WindowSize.Y) };
            screenButton.OnClick += screenButton_OnClick;
            screenButton.OnRelease += screenButton_OnRelease;
            Components.Add(screenButton);

            Button screenRightButton = (Button)screenButton.Clone();
            screenRightButton.LeftClick = false;
            screenRightButton.OnClick -= screenButton_OnClick;
            screenRightButton.OnClick += screenRightButton_OnClick;
            Components.Add(screenRightButton);
        }

        private void screenRightButton_OnClick(Button sender, GameTime gameTime)
        {
            PhysicsSprites.Clear();
            PhysicsSpritesEmitters.Clear();
        }

        void screenButton_OnClick(Button sender, GameTime gameTime)
        {
            Sprite physicsSprite = new Sprite("Fire", new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.5f)) { Scale = new Vector2(2f) };
            physicsSprite.Position = Input.MousePosition;
            physicsSprite.EnablePhysics(BodyFactory.CreateCircle(EdgeGame.World, (physicsSprite.Width * physicsSprite.Scale.X / 2f).ToSimUnits(), 1));
            physicsSprite.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Dynamic;
            physicsSprite.Body.Restitution = 1;
            force = new Vector2(RandomTools.RandomFloat(-max, max), RandomTools.RandomFloat(-max, max));
            point = physicsSprite.Position;
            physicsSprite.Body.ApplyForce(ref force, ref point);
            PhysicsSprites.Add(physicsSprite);

            ParticleEmitter emitter = new ParticleEmitter("Fire", physicsSprite.Position)
            {
                BlendState = BlendState.Additive,
                Life = 700,

                EmitPositionVariance = new Vector2(0, 0),

                MinVelocity = new Vector2(0, 0),
                MaxVelocity = new Vector2(0, 0),

                MinScale = new Vector2(1f),
                MaxScale = new Vector2(2f),

                MinColorIndex = new ColorChangeIndex(200, Color.Magenta, Color.Orange, Color.Purple, Color.Transparent),
                MaxColorIndex = new ColorChangeIndex(200, Color.Teal, Color.OrangeRed, Color.DarkOrange, Color.Transparent),
                EmitWait = 0,
                ParticlesToEmit = 1,
                GrowSpeed = new Vector2(0.03f)
            };
            PhysicsSpritesEmitters.Add(emitter);
        }

        void screenButton_OnRelease(Button sender, GameTime gameTime)
        {
        }

        public override void UpdateObject(GameTime gameTime)
        {
            Fire.Position = Input.MousePosition;

            for (int i = 0; i < PhysicsSprites.Count; i++)
            {
                PhysicsSprites[i].Rotation = 0;
                PhysicsSprites[i].Update(gameTime);

                PhysicsSpritesEmitters[i].Position = PhysicsSprites[i].Position;
                PhysicsSpritesEmitters[i].Update(gameTime);
            }
            foreach (ParticleEmitter emitter in PhysicsSpritesEmitters)
            {
                emitter.Update(gameTime);
            }

            base.UpdateObject(gameTime);
        }

        public override void DrawObject(GameTime gameTime)
        {
            foreach (Sprite physicsSprite in PhysicsSprites)
            {
                physicsSprite.Draw(gameTime);
            }
            foreach (ParticleEmitter emitter in PhysicsSpritesEmitters)
            {
                emitter.Draw(gameTime);
            }

            base.DrawObject(gameTime);
        }

        public override void SwitchOut()
        {
            PhysicsSprites.Clear();
            PhysicsSpritesEmitters.Clear();
            EdgeGame.InitializeWorld(EdgeGame.World.Gravity);

            base.SwitchOut();
        }

        public override void SwitchTo()
        {
            Sprite bottom = new Sprite("Pixel", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y)) { Visible = false, Scale = new Vector2(EdgeGame.WindowSize.X, 10), Color = Color.White };
            bottom.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (bottom.Width * bottom.Scale.X).ToSimUnits(), (bottom.Height * bottom.Scale.Y).ToSimUnits(), 1));
            bottom.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
            Components.Add(bottom);

            Sprite left = new Sprite("Pixel", new Vector2(0, EdgeGame.WindowSize.Y / 2)) { Visible = false, Scale = new Vector2(10, EdgeGame.WindowSize.Y), Color = Color.White };
            left.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (left.Width * left.Scale.X).ToSimUnits(), (left.Height * left.Scale.Y).ToSimUnits(), 1));
            left.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
            Components.Add(left);

            Sprite right = new Sprite("Pixel", new Vector2(EdgeGame.WindowSize.X, EdgeGame.WindowSize.Y / 2)) { Visible = false, Scale = new Vector2(10, EdgeGame.WindowSize.Y), Color = Color.White };
            right.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (right.Width * right.Scale.X).ToSimUnits(), (right.Height * right.Scale.Y).ToSimUnits(), 1));
            right.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
            Components.Add(right);

            Sprite top = new Sprite("Pixel", new Vector2(EdgeGame.WindowSize.X / 2, 0)) { Visible = false, Scale = new Vector2(EdgeGame.WindowSize.X, 10), Color = Color.White };
            top.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (bottom.Width * top.Scale.X).ToSimUnits(), (top.Height * top.Scale.Y).ToSimUnits(), 1));
            top.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
            Components.Add(top);

            base.SwitchTo();
        }

    }
}
