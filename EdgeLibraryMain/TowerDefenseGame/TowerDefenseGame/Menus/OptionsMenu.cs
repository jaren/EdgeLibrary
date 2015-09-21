using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Factories;

namespace TowerDefenseGame
{
    public class OptionsMenu : MenuBase
    {
        public static bool MusicOn
        {
            get { return musicOn; }
            set 
            {
                musicOn = value; 
                if (value)
                {
                    EdgeGame.playPlaylist("Music");
                }
                else
                {
                    EdgeGame.StopPlaylist();
                }
            }
        }
        private static bool musicOn;
        public static bool SoundEffectsOn = true;
        public static bool FullscreenOn
        {
            get { return fullscreenOn; }
            set 
            {
                fullscreenOn = value;

                EdgeGame.Game.Graphics.IsFullScreen = value;
                EdgeGame.Game.Graphics.ApplyChanges();
            }
        }
        private static bool fullscreenOn;
        public static bool ParticlesOn = true;

        public TextSprite Title;
        public TextSprite SubTitle;
        public Button QuitButton;
        public TextSprite QuitButtonText;
        public ButtonToggle MusicButton;
        public TextSprite MusicButtonText;
        public ButtonToggle SoundEffectsButton;
        public TextSprite SoundEffectsButtonText;
        public ButtonToggle FullscreenButton;
        public TextSprite FullscreenButtonText;
        public ButtonToggle ParticlesButton;
        public TextSprite ParticlesButtonText;
        public Button ReturnButton;
        public TextSprite ReturnButtonText;
        public DialogPanel QuitPanel;

        public OptionsMenu()
            : base("OptionsMenu")
        {
            Title = new TextSprite(Config.MenuTitleFont, "Options Menu", new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(Title);

            SubTitle = new TextSprite(Config.MenuSubtitleFont, "", new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.1f)) { Color = Config.MenuTextColor };
            Components.Add(SubTitle);

            QuitButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.95f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            QuitButton.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            QuitButton.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            QuitButton.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            QuitButton.Style.AllColors = Config.MenuButtonColor;
            QuitButton.OnRelease += (x, y) =>
            {
                if (MenuManager.PreviousMenu.Name == "GameMenu")
                {
                    QuitPanel.Show();
                }
                else
                {
                    MenuManager.SwitchMenu("MainMenu");
                }
            };
            Components.Add(QuitButton);

            QuitButtonText = new TextSprite(Config.MenuButtonTextFont, "Quit Game", QuitButton.Position);
            Components.Add(QuitButtonText);

            MusicButton = new ButtonToggle(MusicOn ? "grey_boxCheckmark" : "grey_boxCross", new Vector2(EdgeGame.WindowSize.X * 0.35f, EdgeGame.WindowSize.Y * 0.35f)) {  Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            MusicButton.OnStyle.AllTextures = EdgeGame.GetTexture("grey_boxCheckmark");
            MusicButton.OnStyle.AllColors = Config.MenuButtonColor;
            MusicButton.OffStyle.AllTextures = EdgeGame.GetTexture("grey_boxCross");
            MusicButton.OffStyle.AllColors = Config.MenuButtonColor;
            MusicButton.Style = MusicOn ? MusicButton.OnStyle : MusicButton.OffStyle;
            MusicButton.OnRelease += (x, y) => { MusicOn = MusicButton.On; };
            MusicButton.On = MusicOn;
            Components.Add(MusicButton);

            MusicButtonText = new TextSprite(Config.MenuButtonTextFont, "Music", MusicButton.Position + new Vector2(0, -MusicButton.Height)) { CenterAsOrigin = true };
            Components.Add(MusicButtonText);

            SoundEffectsButton = new ButtonToggle(SoundEffectsOn ? "grey_boxCheckmark" : "grey_boxCross", new Vector2(EdgeGame.WindowSize.X * 0.65f, EdgeGame.WindowSize.Y * 0.35f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            SoundEffectsButton.OnStyle.AllTextures = EdgeGame.GetTexture("grey_boxCheckmark");
            SoundEffectsButton.OnStyle.AllColors = Config.MenuButtonColor;
            SoundEffectsButton.OffStyle.AllTextures = EdgeGame.GetTexture("grey_boxCross");
            SoundEffectsButton.OffStyle.AllColors = Config.MenuButtonColor;
            SoundEffectsButton.Style = SoundEffectsOn ? SoundEffectsButton.OnStyle : SoundEffectsButton.OffStyle;
            SoundEffectsButton.OnRelease += (x, y) => { SoundEffectsOn = SoundEffectsButton.On; };
            SoundEffectsButton.On = SoundEffectsOn;
            Components.Add(SoundEffectsButton);

            SoundEffectsButtonText = new TextSprite(Config.MenuButtonTextFont, "Sound Effects", SoundEffectsButton.Position + new Vector2(0, -SoundEffectsButton.Height)) { CenterAsOrigin = true };
            Components.Add(SoundEffectsButtonText);

            FullscreenButton = new ButtonToggle(FullscreenOn ? "grey_boxCheckmark" : "grey_boxCross", new Vector2(EdgeGame.WindowSize.X * 0.35f, EdgeGame.WindowSize.Y * 0.65f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            FullscreenButton.OnStyle.AllTextures = EdgeGame.GetTexture("grey_boxCheckmark");
            FullscreenButton.OnStyle.AllColors = Config.MenuButtonColor;
            FullscreenButton.OffStyle.AllTextures = EdgeGame.GetTexture("grey_boxCross");
            FullscreenButton.OffStyle.AllColors = Config.MenuButtonColor;
            FullscreenButton.Style = FullscreenOn ? FullscreenButton.OnStyle : FullscreenButton.OffStyle;
            FullscreenButton.OnRelease += (x, y) => { FullscreenOn = FullscreenButton.On; };
            FullscreenButton.On = FullscreenOn;
            Components.Add(FullscreenButton);

            FullscreenButtonText = new TextSprite(Config.MenuButtonTextFont, "Fullscreen", FullscreenButton.Position + new Vector2(0, -FullscreenButton.Height)) { CenterAsOrigin = true };
            Components.Add(FullscreenButtonText);

            ParticlesButton = new ButtonToggle(ParticlesOn ? "grey_boxCheckmark" : "grey_boxCross", new Vector2(EdgeGame.WindowSize.X * 0.65f, EdgeGame.WindowSize.Y * 0.65f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            ParticlesButton.OnStyle.AllTextures = EdgeGame.GetTexture("grey_boxCheckmark");
            ParticlesButton.OnStyle.AllColors = Config.MenuButtonColor;
            ParticlesButton.OffStyle.AllTextures = EdgeGame.GetTexture("grey_boxCross");
            ParticlesButton.OffStyle.AllColors = Config.MenuButtonColor;
            ParticlesButton.Style = ParticlesOn ? ParticlesButton.OnStyle : ParticlesButton.OffStyle;
            ParticlesButton.OnRelease += (x, y) => { ParticlesOn = ParticlesButton.On; };
            ParticlesButton.On = ParticlesOn;
            Components.Add(ParticlesButton);

            ParticlesButtonText = new TextSprite(Config.MenuButtonTextFont, "Particles", ParticlesButton.Position + new Vector2(0, -ParticlesButton.Height)) { CenterAsOrigin = true };
            Components.Add(ParticlesButtonText);

            ReturnButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.85f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.1f) };
            ReturnButton.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            ReturnButton.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            ReturnButton.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            ReturnButton.Style.AllColors = Config.MenuButtonColor;
            ReturnButton.OnRelease += (x, y) =>
            {
                MenuManager.SwitchMenu(MenuManager.PreviousMenu.Name);
            };
            Components.Add(ReturnButton);

            ReturnButtonText = new TextSprite(Config.MenuButtonTextFont, "Back", ReturnButton.Position);
            Components.Add(ReturnButtonText);

            QuitPanel = new DialogPanel("If you quit you will lose game progrees", "Quit", "Cancel", () => { MenuManager.SwitchMenu("MainMenu"); }, () => { });
            Components.Add(QuitPanel);

            Input.OnKeyRelease += Input_OnKeyRelease;
        }

        void Input_OnKeyRelease(Microsoft.Xna.Framework.Input.Keys key)
        {
            if (MenuManager.SelectedMenu == this && key == Config.BackKey && !MenuManager.InputEventHandled)
            {
                MenuManager.SwitchMenu(MenuManager.PreviousMenu.Name);
                MenuManager.InputEventHandled = true;
            }
        }

        public override void SwitchTo()
        {
            base.SwitchTo();

            /*
            if (MenuManager.PreviousMenu.Name == "GameMenu")
            {
                QuitButton.Visible = true;
                QuitButtonText.Visible = true;
            }
            else
            {
                QuitButton.Visible = false;
                QuitButtonText.Visible = false;
            }
             */
        }
    }
}
