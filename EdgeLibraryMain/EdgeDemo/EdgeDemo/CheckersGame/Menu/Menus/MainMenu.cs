using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using FarseerPhysics.Factories;

namespace EdgeDemo.CheckersGame
{
    public class MainMenu : MenuBase
    {
        Sprite physicsSprite;
        Sprite physicsSprite2;

        public MainMenu() : base("MainMenu")
        {
            Button screenButton = new Button("Pixel", new Vector2(EdgeGame.WindowSize.X/2, EdgeGame.WindowSize.Y/2)) { Visible = false, Scale = new Vector2(EdgeGame.WindowSize.X, EdgeGame.WindowSize.Y) };
            screenButton.OnClick += screenButton_OnClick;
            Components.Add(screenButton);

            physicsSprite = new Sprite("Checkers", new Vector2(EdgeGame.WindowSize.X*0.5f, EdgeGame.WindowSize.Y*0.5f)) { Scale = new Vector2(0.5f), Color = Color.Red };
            physicsSprite.EnablePhysics(BodyFactory.CreateCircle(EdgeGame.World, (physicsSprite.Width*physicsSprite.Scale.X/2f).ToSimUnits(), 1));
            physicsSprite.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Dynamic;
            physicsSprite.Body.Restitution = 1;
            float max = 500;
            Vector2 force = new Vector2(RandomTools.RandomFloat(-max, max), RandomTools.RandomFloat(-max, max));
            Vector2 point = physicsSprite.Position;
            physicsSprite.Body.ApplyForce(ref force, ref point);
            Components.Add(physicsSprite);

            physicsSprite2 = (Sprite)physicsSprite.Clone();
            physicsSprite2.Color = Color.Black;
            force = new Vector2(RandomTools.RandomFloat(-max, max), RandomTools.RandomFloat(-max, max));
            point = physicsSprite2.Position;
            physicsSprite2.Body.ApplyForce(ref force, ref point);
            Components.Add(physicsSprite2);

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

            Button singleplayerButton = new Button("blue_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.2f)) { ClickTexture = EdgeGame.GetTexture("blue_button01"), MouseOverTexture = EdgeGame.GetTexture("blue_button02"), Scale = new Vector2(1) };
            singleplayerButton.OnRelease += (x, y) => {MenuManager.SwitchMenu("SingleplayerMenu"); };
            Components.Add(singleplayerButton);

            TextSprite singleplayerButtonText = new TextSprite(Config.MenuButtonTextFont, "Singleplayer", singleplayerButton.Position);
            Components.Add(singleplayerButtonText);

            Button hotseatButton = new Button("blue_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.3f)) { ClickTexture = EdgeGame.GetTexture("blue_button01"), MouseOverTexture = EdgeGame.GetTexture("blue_button02"), Scale = new Vector2(1) };
            hotseatButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("HotseatMenu"); };
            Components.Add(hotseatButton);

            TextSprite hotseatButtonText = new TextSprite(Config.MenuButtonTextFont, "Hotseat", hotseatButton.Position);
            Components.Add(hotseatButtonText);

            Button multiplayerButton = new Button("blue_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.4f)) { ClickTexture = EdgeGame.GetTexture("blue_button01"), MouseOverTexture = EdgeGame.GetTexture("blue_button02"), Scale = new Vector2(1) };
            multiplayerButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("MultiplayerMenu"); };
            Components.Add(multiplayerButton);

            TextSprite multiplayerButtonText = new TextSprite(Config.MenuButtonTextFont, "Multiplayer", multiplayerButton.Position);
            Components.Add(multiplayerButtonText);

            Button optionsButton = new Button("blue_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.5f)) { ClickTexture = EdgeGame.GetTexture("blue_button01"), MouseOverTexture = EdgeGame.GetTexture("blue_button02"), Scale = new Vector2(1) };
            optionsButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("OptionsMenu"); };
            Components.Add(optionsButton);

            TextSprite optionsButtonText = new TextSprite(Config.MenuButtonTextFont, "Options", optionsButton.Position);
            Components.Add(optionsButtonText);

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
            float max = 1000;
            Vector2 force = new Vector2(RandomTools.RandomFloat(-max, max), RandomTools.RandomFloat(-max, max));
            Vector2 point = physicsSprite.Position;
            physicsSprite.Body.ApplyForce(ref force, ref point);

            force = new Vector2(RandomTools.RandomFloat(-max, max), RandomTools.RandomFloat(-max, max));
            point = physicsSprite2.Position;
            physicsSprite2.Body.ApplyForce(ref force, ref point);
        }

        public override void Update(GameTime gameTime)
        {
            physicsSprite.Rotation = 0;
            physicsSprite2.Rotation = 0;
            base.Update(gameTime);
        }

        public override void SwitchTo()
        {
            EdgeGame.ClearColor = Color.Gray;
            base.SwitchTo();
        }
    }
}
