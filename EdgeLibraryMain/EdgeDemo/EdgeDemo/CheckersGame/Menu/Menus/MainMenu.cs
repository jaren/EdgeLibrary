using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;

namespace EdgeDemo.CheckersGame
{
    public class MainMenu : MenuBase
    {
        private Sprite spriteToClone;
        private List<Sprite> PhysicsSprites;
        private float max;
        private Vector2 force;
        private Vector2 point;

        public MainMenu() : base("MainMenu")
        {
            ParticleEmitter emitter = new ParticleEmitter("Fire", EdgeGame.WindowSize / 2)
            {
                MinEmitWait = 10,
                MaxEmitWait = 20,

                BlendState = BlendState.Additive,

                MinVelocity = new Vector2(-1.5f),
                MaxVelocity = new Vector2(2.5f),

                GrowSpeed = 0,

                //MinColorIndex = new ColorChangeIndex(1000, Color.MediumAquamarine, Color.Purple, Color.OrangeRed, Color.Transparent),
                //MaxColorIndex = new ColorChangeIndex(1000, Color.Aquamarine, Color.MediumPurple, Color.DarkOrange, Color.Transparent),

                TextureIndex = TextureChangeIndex.FromXMLSpriteSheet(10, "Particles/Explosion", "Particles/Explosion"),

                MinLife = 4000,
                MaxLife = 4500,

                MinScale = new Vector2(2),
                MaxScale = new Vector2(4),

                MaxParticles = 1000
            };
            Components.Add(emitter);

            Button screenButton = new Button("Pixel", new Vector2(EdgeGame.WindowSize.X/2, EdgeGame.WindowSize.Y/2)) { Visible = false, Scale = new Vector2(EdgeGame.WindowSize.X, EdgeGame.WindowSize.Y) };
            screenButton.OnClick += screenButton_OnClick;
            Components.Add(screenButton);

            Button screenRightButton = (Button)screenButton.Clone();
            screenRightButton.LeftClick = false;
            screenRightButton.OnClick -= screenButton_OnClick;
            screenRightButton.OnClick += screenRightButton_OnClick;
            Components.Add(screenRightButton);

            spriteToClone = new Sprite("Checkers", new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.5f)) { Scale = new Vector2(0.3f) };
            spriteToClone.EnablePhysics(BodyFactory.CreateCircle(EdgeGame.World, (spriteToClone.Width * spriteToClone.Scale.X / 2f).ToSimUnits(), 1));
            spriteToClone.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Dynamic;
            spriteToClone.Body.Restitution = 1;
            max = 100;
            force = new Vector2(RandomTools.RandomFloat(-max, max), RandomTools.RandomFloat(-max, max));
            point = spriteToClone.Position;
            spriteToClone.Body.ApplyForce(ref force, ref point);

            PhysicsSprites = new List<Sprite>();

            Sprite bottom = new Sprite("Pixel", new Vector2(EdgeGame.WindowSize.X/2, EdgeGame.WindowSize.Y)) { Visible = false, Scale = new Vector2(EdgeGame.WindowSize.X, 10), Color = Color.White };
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

            TextSprite title = new TextSprite(Config.MenuTitleFont, "Checkers Game", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(title);

            TextSprite subTitle = new TextSprite(Config.MenuSubtitleFont, "Click!", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.1f)) { Color = Config.MenuTextColor };
            Components.Add(subTitle);

            Button startGameButton = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.5f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            startGameButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("MainChooseMenu"); };
            startGameButton.Style.NormalTexture = EdgeGame.GetTexture("grey_button00");
            startGameButton.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button02");
            startGameButton.Style.ClickTexture = EdgeGame.GetTexture("grey_button01");
            startGameButton.Style.AllColors = Config.MenuButtonColor;
            Components.Add(startGameButton);

            startGameButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (startGameButton.Width * startGameButton.Scale.X).ToSimUnits(), (startGameButton.Height * startGameButton.Scale.Y).ToSimUnits(), 1));
            startGameButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            TextSprite startGameButtonText = new TextSprite(Config.MenuButtonTextFont, "START A GAME", startGameButton.Position);
            Components.Add(startGameButtonText);

            Button optionsButton = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.7f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            optionsButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("NoGameOptionsMenu"); };
            optionsButton.Style.NormalTexture = EdgeGame.GetTexture("grey_button00");
            optionsButton.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button02");
            optionsButton.Style.ClickTexture = EdgeGame.GetTexture("grey_button01");
            optionsButton.Style.AllColors = Config.MenuButtonColor;
            Components.Add(optionsButton);

            optionsButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (optionsButton.Width * optionsButton.Scale.X).ToSimUnits(), (optionsButton.Height * optionsButton.Scale.Y).ToSimUnits(), 1));
            optionsButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            TextSprite optionsButtonText = new TextSprite(Config.MenuButtonTextFont, "OPTIONS", optionsButton.Position);
            Components.Add(optionsButtonText);

            Button creditsButton = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.9f)) { Color = Config.MenuButtonColor, Scale = new Vector2(0.9f) };
            creditsButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("CreditsMenu"); };
            creditsButton.Style.NormalTexture = EdgeGame.GetTexture("grey_button00");
            creditsButton.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button02");
            creditsButton.Style.ClickTexture = EdgeGame.GetTexture("grey_button01");
            creditsButton.Style.AllColors = Config.MenuButtonColor;
            Components.Add(creditsButton);

            creditsButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (creditsButton.Width * creditsButton.Scale.X).ToSimUnits(), (creditsButton.Height * creditsButton.Scale.Y).ToSimUnits(), 1));
            creditsButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            TextSprite creditsButtonText = new TextSprite(Config.MenuButtonTextFont, "CREDITS", creditsButton.Position);
            Components.Add(creditsButtonText);

            Input.OnKeyRelease += Input_OnKeyRelease;
        }

        void Input_OnKeyRelease(Microsoft.Xna.Framework.Input.Keys key)
        {
            if (MenuManager.SelectedMenu == this && key == Config.BackKey)
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure you want to quit?", "Quit", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                {
                    EdgeGame.Stop();
                }
            }
        }

        void screenButton_OnClick(Button sender, GameTime gameTime)
        {
            Sprite physicsSprite2 = (Sprite)spriteToClone.Clone();
            physicsSprite2.Position = new Vector2(RandomTools.RandomFloat(0, EdgeGame.WindowSize.X), RandomTools.RandomFloat(0, EdgeGame.WindowSize.Y));
            physicsSprite2.AddAction(new AColorChange(new InfiniteColorChangeIndex(Color.Black, Color.White, 1000)));
            force = new Vector2(RandomTools.RandomFloat(-max, max), RandomTools.RandomFloat(-max, max));
            point = physicsSprite2.Position;
            physicsSprite2.Body.ApplyForce(ref force, ref point);
            PhysicsSprites.Add(physicsSprite2);
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
            foreach(Sprite physicsSprite in PhysicsSprites)
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

        public override void SwitchTo()
        {
            EdgeGame.ClearColor = Color.Gray;
            base.SwitchTo();
        }
    }
}
