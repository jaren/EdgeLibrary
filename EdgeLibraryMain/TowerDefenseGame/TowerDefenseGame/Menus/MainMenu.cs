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
        private Button startGameButton;
        private Button optionsButton;
        private Button creditsButton;

        public MainMenu() : base("MainMenu")
        {
            TextSprite title = new TextSprite(Config.MenuTitleFont, "Tower Defense Game", new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(title);

            TextSprite subTitle = new TextSprite(Config.MenuSubtitleFont, "Click for more fire! Right click to clear!", new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.1f)) { Color = Config.MenuTextColor };
            Components.Add(subTitle);

            Style buttonStyle = new Style(EdgeGame.GetTexture(Config.ButtonMouseOverTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonNormalTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonClickTexture), Config.MenuButtonColor);

            startGameButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.3f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.55f) };
            startGameButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("GameSelectMenu"); };
            startGameButton.Style = buttonStyle;
            Components.Add(startGameButton);

            TextSprite startLocalGameButtonText = new TextSprite(Config.MenuButtonTextFont, "START GAME", startGameButton.Position);
            Components.Add(startLocalGameButtonText);

            optionsButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.5f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.55f) };
            optionsButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("OptionsMenu"); };
            optionsButton.Style = buttonStyle;
            Components.Add(optionsButton);

            TextSprite optionsButtonText = new TextSprite(Config.MenuButtonTextFont, "OPTIONS", optionsButton.Position);
            Components.Add(optionsButtonText);

            creditsButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.7f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.55f) };
            creditsButton.OnRelease += (x, y) => { MenuManager.SwitchMenu("CreditsMenu"); };
            creditsButton.Style = buttonStyle;
            Components.Add(creditsButton);
            TextSprite creditsButtonText = new TextSprite(Config.MenuButtonTextFont, "CREDITS", creditsButton.Position);
            Components.Add(creditsButtonText);

            Input.OnKeyRelease += Input_OnKeyRelease;
        }

        void Input_OnKeyRelease(Keys key)
        {
            if (MenuManager.SelectedMenu == this && key == Config.BackKey && !EdgeGame.Game.Graphics.IsFullScreen && !MenuManager.InputEventHandled)
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure you want to quit?", "Quit", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                {
                    EdgeGame.Stop();
                }
                MenuManager.InputEventHandled = true;
            }
        }

        public override void SwitchTo()
        {
            base.SwitchTo();

            startGameButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (startGameButton.Width * startGameButton.Scale.X).ToSimUnits(), (startGameButton.Height * startGameButton.Scale.Y).ToSimUnits(), 1));
            startGameButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

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
