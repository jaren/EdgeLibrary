using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Input;

namespace EdgeDemo.CheckersGame
{
    public class MainMenu : ParticleMenu
    {
        private Button startLocalGameButton;
        private Button startMultiplayerGameButton;
        private Button optionsButton;
        private Button creditsButton;

        private CheckersService.CheckersServiceClient WebService = new CheckersService.CheckersServiceClient();

        public MainMenu() : base("MainMenu")
        {
            TextSprite title = new TextSprite(Config.MenuTitleFont, "Checkers Game", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(title);

            TextSprite subTitle = new TextSprite(Config.MenuSubtitleFont, "Click for Checkers! Right Click to Move Them!", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.1f)) { Color = Config.MenuTextColor };
            Components.Add(subTitle);

            BitmapTextSprite bmsprite = new BitmapTextSprite("windsong_regular_65", "This should work...", new Vector2(100));
            Components.Add(bmsprite);

            startLocalGameButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.3f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.55f) };
            startLocalGameButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("LocalGameSelectMenu"); };
            startLocalGameButton.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            startLocalGameButton.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            startLocalGameButton.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            startLocalGameButton.Style.AllColors = Config.MenuButtonColor;
            Components.Add(startLocalGameButton);

            TextSprite startLocalGameButtonText = new TextSprite(Config.MenuButtonTextFont, "LOCAL GAME", startLocalGameButton.Position);
            Components.Add(startLocalGameButtonText);

            System.Windows.Forms.Form form1 = new System.Windows.Forms.Form();

            startMultiplayerGameButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.45f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.55f) };
            startMultiplayerGameButton.OnRelease += (x, y) => {
                try
                {
                    WebService.GetAllGames();
                    MenuManager.SwitchMenu("MultiplayerGameSelectMenu");
                }
                catch
                {
                    if (!EdgeGame.Game.Graphics.IsFullScreen)
                    {
                        System.Windows.Forms.MessageBox.Show("The multiplayer service is not available. Please try again later.", "Multiplayer not Available", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
            };
            startMultiplayerGameButton.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            startMultiplayerGameButton.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            startMultiplayerGameButton.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            startMultiplayerGameButton.Style.AllColors = Config.MenuButtonColor;
            Components.Add(startMultiplayerGameButton);

            TextSprite startMultiplayerGameButtonText = new TextSprite(Config.MenuButtonTextFont, "MULTIPLAYER GAME", startMultiplayerGameButton.Position);
            Components.Add(startMultiplayerGameButtonText);

            optionsButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.6f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.55f) };
            optionsButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("NoGameOptionsMenu"); };
            optionsButton.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            optionsButton.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            optionsButton.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            optionsButton.Style.AllColors = Config.MenuButtonColor;
            Components.Add(optionsButton);

            TextSprite optionsButtonText = new TextSprite(Config.MenuButtonTextFont, "OPTIONS", optionsButton.Position);
            Components.Add(optionsButtonText);

            creditsButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.75f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.55f) };
            creditsButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("CreditsMenu"); };
            creditsButton.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            creditsButton.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            creditsButton.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            creditsButton.Style.AllColors = Config.MenuButtonColor;
            Components.Add(creditsButton);
            TextSprite creditsButtonText = new TextSprite(Config.MenuButtonTextFont, "CREDITS", creditsButton.Position);
            Components.Add(creditsButtonText);

            Input.OnKeyRelease += Input_OnKeyRelease;
        }

        void Input_OnKeyRelease(Keys key)
        {
            if (MenuManager.SelectedMenu == this && key == Config.BackKey && !EdgeGame.Game.Graphics.IsFullScreen)
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure you want to quit?", "Quit", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                {
                    EdgeGame.Stop();
                }
            }
        }

        public override void SwitchTo()
        {
            base.SwitchTo();

            startLocalGameButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (startLocalGameButton.Width * startLocalGameButton.Scale.X).ToSimUnits(), (startLocalGameButton.Height * startLocalGameButton.Scale.Y).ToSimUnits(), 1));
            startLocalGameButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            startMultiplayerGameButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (startMultiplayerGameButton.Width * startMultiplayerGameButton.Scale.X).ToSimUnits(), (startMultiplayerGameButton.Height * startMultiplayerGameButton.Scale.Y).ToSimUnits(), 1));
            startMultiplayerGameButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            optionsButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (optionsButton.Width * optionsButton.Scale.X).ToSimUnits(), (optionsButton.Height * optionsButton.Scale.Y).ToSimUnits(), 1));
            optionsButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            creditsButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (creditsButton.Width * creditsButton.Scale.X).ToSimUnits(), (creditsButton.Height * creditsButton.Scale.Y).ToSimUnits(), 1));
            creditsButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            if (OptionsMenu.MusicOn)
            {
                EdgeGame.playPlaylist("TitleMusic");
            }
        }

        public override void SwitchOut()
        {
            base.SwitchOut();

            if (OptionsMenu.MusicOn)
            {
                EdgeGame.playPlaylist("Music");
            }
        }
    }
}
