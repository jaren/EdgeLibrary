using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Input;

namespace TowerDefenseGame
{
    public class MainMenu : ParticleMenu
    {
        private Button startLocalGameButton;
        private Button optionsButton;
        private Button creditsButton;

        public MainMenu() : base("MainMenu")
        {
            TextSprite title = new TextSprite(Config.MenuTitleFont, "Tower Defense Game", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(title);

            TextSprite subTitle = new TextSprite(Config.MenuSubtitleFont, "Click for more Fire!", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.1f)) { Color = Config.MenuTextColor };
            Components.Add(subTitle);

            startLocalGameButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.3f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.55f) };
            startLocalGameButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("GameSelectMenu"); };
            startLocalGameButton.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            startLocalGameButton.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            startLocalGameButton.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            startLocalGameButton.Style.AllColors = Config.MenuButtonColor;
            Components.Add(startLocalGameButton);

            TextSprite startLocalGameButtonText = new TextSprite(Config.MenuButtonTextFont, "START GAME", startLocalGameButton.Position);
            Components.Add(startLocalGameButtonText);

            optionsButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.5f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.55f) };
            optionsButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("NoGameOptionsMenu"); };
            optionsButton.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            optionsButton.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            optionsButton.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            optionsButton.Style.AllColors = Config.MenuButtonColor;
            Components.Add(optionsButton);

            TextSprite optionsButtonText = new TextSprite(Config.MenuButtonTextFont, "OPTIONS", optionsButton.Position);
            Components.Add(optionsButtonText);

            creditsButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.7f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.55f) };
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

            EdgeGame.ClearColor = Color.Black;

            startLocalGameButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (startLocalGameButton.Width * startLocalGameButton.Scale.X).ToSimUnits(), (startLocalGameButton.Height * startLocalGameButton.Scale.Y).ToSimUnits(), 1));
            startLocalGameButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

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
