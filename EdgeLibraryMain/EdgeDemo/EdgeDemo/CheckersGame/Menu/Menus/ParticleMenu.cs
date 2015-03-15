using EdgeLibrary;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    public class ParticleMenu : MenuBase
    {
        protected List<Sprite> PhysicsSprites;
        protected float max = 100;
        protected Vector2 force;
        protected Vector2 point;
        protected int particleWait = 100;
        protected bool clicking = false;

        public ParticleMenu(string name)
            : base(name)
        {
            PhysicsSprites = new List<Sprite>();

            Ticker particleTicker = new Ticker(particleWait);
            particleTicker.OnTick += particleTicker_OnTick;
            Components.Add(particleTicker);

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

        void screenButton_OnClick(Button sender, GameTime gameTime)
        {
            clicking = true;
        }

        void screenButton_OnRelease(Button sender, GameTime gameTime)
        {
            clicking = false;
        }

        void particleTicker_OnTick(GameTime gameTime)
        {
            if (clicking)
            {
                Sprite physicsSprite2 = new Sprite("Checkers", new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.5f)) { Scale = new Vector2(0.3f) };
                physicsSprite2.Position = Input.MousePosition;
                physicsSprite2.EnablePhysics(BodyFactory.CreateCircle(EdgeGame.World, (physicsSprite2.Width * physicsSprite2.Scale.X / 2f).ToSimUnits(), 1));
                physicsSprite2.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Dynamic;
                physicsSprite2.Body.Restitution = 1;
                physicsSprite2.AddAction(new AColorChange(new InfiniteColorChangeIndex(Color.Black, Color.White, 1000)));
                force = new Vector2(RandomTools.RandomFloat(-max, max), RandomTools.RandomFloat(-max, max));
                point = physicsSprite2.Position;
                physicsSprite2.Body.ApplyForce(ref force, ref point);
                PhysicsSprites.Add(physicsSprite2);
            }
        }

        void screenRightButton_OnClick(Button sender, GameTime gameTime)
        {
            force = new Vector2(RandomTools.RandomFloat(-max, max), RandomTools.RandomFloat(-max, max));

            foreach (Sprite sprite in PhysicsSprites)
            {
                point = sprite.Position;
                sprite.Body.ApplyForce(ref force, ref point);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Sprite physicsSprite in PhysicsSprites)
            {
                physicsSprite.Rotation = 0;
                physicsSprite.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Sprite physicsSprite in PhysicsSprites)
            {
                physicsSprite.Draw(gameTime);
            }
            base.Draw(gameTime);
        }

        public override void SwitchOut()
        {
            PhysicsSprites.Clear();
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

            EdgeGame.ClearColor = Color.Gray;
            base.SwitchTo();
        }

    }
}
