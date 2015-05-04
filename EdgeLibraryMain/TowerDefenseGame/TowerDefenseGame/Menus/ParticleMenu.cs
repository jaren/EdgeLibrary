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
        protected float max = 100;
        protected Vector2 force;
        protected Vector2 point;
        protected int particleWait = 100;
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
        }

        void screenButton_OnClick(Button sender, GameTime gameTime)
        {
            clicking = true;
        }

        void screenButton_OnRelease(Button sender, GameTime gameTime)
        {
            clicking = false;
        }

        public override void Update(GameTime gameTime)
        {
            Fire.Position = Input.MousePosition;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void SwitchOut()
        {
            base.SwitchOut();
        }

        public override void SwitchTo()
        {
            base.SwitchTo();
        }

    }
}
