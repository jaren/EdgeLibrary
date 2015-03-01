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
        private List<Sprite> PhysicsSprites;
        private float max;
        private Vector2 force;
        private Vector2 point;
        private int particleWait = 1;
        private bool clicking = false;
        private Button startGameButton;
        private Button optionsButton;
        private Button creditsButton;

        public MainMenu() : base("MainMenu")
        {
            Button screenButton = new Button("Pixel", new Vector2(EdgeGame.WindowSize.X/2, EdgeGame.WindowSize.Y/2)) { Visible = false, Scale = new Vector2(EdgeGame.WindowSize.X, EdgeGame.WindowSize.Y) };
            screenButton.OnClick += screenButton_OnClick;
            screenButton.OnRelease += screenButton_OnRelease;
            Components.Add(screenButton);

            Button screenRightButton = (Button)screenButton.Clone();
            screenRightButton.LeftClick = false;
            screenRightButton.OnClick -= screenButton_OnClick;
            screenRightButton.OnClick += screenRightButton_OnClick;
            Components.Add(screenRightButton);

            PhysicsSprites = new List<Sprite>();

            Ticker particleTicker = new Ticker(particleWait);
            particleTicker.OnTick += particleTicker_OnTick;
            Components.Add(particleTicker);

            TextSprite title = new TextSprite(Config.MenuTitleFont, "Checkers Game", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(title);

            TextSprite subTitle = new TextSprite(Config.MenuSubtitleFont, "Click!", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.1f)) { Color = Config.MenuTextColor };
            Components.Add(subTitle);

            startGameButton = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.5f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            startGameButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("MainChooseMenu"); };
            startGameButton.Style.NormalTexture = EdgeGame.GetTexture("grey_button00");
            startGameButton.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button02");
            startGameButton.Style.ClickTexture = EdgeGame.GetTexture("grey_button01");
            startGameButton.Style.AllColors = Config.MenuButtonColor;
            Components.Add(startGameButton);

            TextSprite startGameButtonText = new TextSprite(Config.MenuButtonTextFont, "START A GAME", startGameButton.Position);
            Components.Add(startGameButtonText);

            optionsButton = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.7f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            optionsButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("NoGameOptionsMenu"); };
            optionsButton.Style.NormalTexture = EdgeGame.GetTexture("grey_button00");
            optionsButton.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button02");
            optionsButton.Style.ClickTexture = EdgeGame.GetTexture("grey_button01");
            optionsButton.Style.AllColors = Config.MenuButtonColor;
            Components.Add(optionsButton);

            TextSprite optionsButtonText = new TextSprite(Config.MenuButtonTextFont, "OPTIONS", optionsButton.Position);
            Components.Add(optionsButtonText);

            creditsButton = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.9f)) { Color = Config.MenuButtonColor, Scale = new Vector2(0.9f) };
            creditsButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("CreditsMenu"); };
            creditsButton.Style.NormalTexture = EdgeGame.GetTexture("grey_button00");
            creditsButton.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button02");
            creditsButton.Style.ClickTexture = EdgeGame.GetTexture("grey_button01");
            creditsButton.Style.AllColors = Config.MenuButtonColor;
            Components.Add(creditsButton);

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

            startGameButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (startGameButton.Width * startGameButton.Scale.X).ToSimUnits(), (startGameButton.Height * startGameButton.Scale.Y).ToSimUnits(), 1));
            startGameButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            optionsButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (optionsButton.Width * optionsButton.Scale.X).ToSimUnits(), (optionsButton.Height * optionsButton.Scale.Y).ToSimUnits(), 1));
            optionsButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            creditsButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (creditsButton.Width * creditsButton.Scale.X).ToSimUnits(), (creditsButton.Height * creditsButton.Scale.Y).ToSimUnits(), 1));
            creditsButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            EdgeGame.ClearColor = Color.Gray;
            base.SwitchTo();
        }
    }
}
