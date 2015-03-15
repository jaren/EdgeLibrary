using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeDemo.CheckersService;

namespace EdgeDemo.CheckersGame
{
    public class OptionsMenu : MenuBase
    {
        public static bool MusicOn = true;
        public static bool SoundEffectsOn = true;
        public static bool FullscreenOn = true;
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

        public OptionsMenu()
            : base("OptionsMenu")
        {
            CheckersServiceClient ServiceClient = new CheckersServiceClient();

            Title = new TextSprite(Config.MenuTitleFont, "Options Menu", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(Title);

            SubTitle = new TextSprite(Config.MenuSubtitleFont, "Click!", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.1f)) { Color = Config.MenuTextColor };
            Components.Add(SubTitle);

            QuitButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.85f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            QuitButton.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            QuitButton.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            QuitButton.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            QuitButton.Style.AllColors = Config.MenuButtonColor;
            QuitButton.OnRelease += (x, y) =>
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure you want to leave this game?", "Leave?", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK) 
                {
                    if (Config.GetGameType() == Config.GameType.Online)
                    {
                        ServiceClient.Disconnect(((WebPlayer)Config.GetWebPlayer()).ThisGameID, Config.IsHost());
                    }

                    MenuManager.SwitchMenu("MainMenu");
                }
            };
            Components.Add(QuitButton);

            QuitButtonText = new TextSprite(Config.MenuButtonTextFont, "Quit Game", QuitButton.Position);
            Components.Add(QuitButtonText);

            MusicButton = new ButtonToggle("grey_boxCheckmark", new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.25f)) {  Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            MusicButton.OnStyle.AllTextures = EdgeGame.GetTexture("grey_boxCheckmark");
            MusicButton.OnStyle.AllColors = Config.MenuButtonColor;
            MusicButton.OffStyle.AllTextures = EdgeGame.GetTexture("grey_box");
            MusicButton.OffStyle.AllColors = Config.MenuButtonColor;
            MusicButton.Style = MusicButton.OnStyle;
            MusicButton.OnRelease += (x, y) => { MusicOn = MusicButton.On; };
            MusicButton.On = MusicOn;
            Components.Add(MusicButton);

            MusicButtonText = new TextSprite(Config.MenuButtonTextFont, "Music", MusicButton.Position + new Vector2(MusicButton.Width, -MusicButton.Height / 2)) { CenterAsOrigin = false };
            Components.Add(MusicButtonText);

            SoundEffectsButton = new ButtonToggle("grey_boxCheckmark", new Vector2(EdgeGame.WindowSize.X * 0.75f, EdgeGame.WindowSize.Y * 0.25f)) {  Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            SoundEffectsButton.OnStyle.AllTextures = EdgeGame.GetTexture("grey_boxCheckmark");
            SoundEffectsButton.OnStyle.AllColors = Config.MenuButtonColor;
            SoundEffectsButton.OffStyle.AllTextures = EdgeGame.GetTexture("grey_box");
            SoundEffectsButton.OffStyle.AllColors = Config.MenuButtonColor;
            SoundEffectsButton.Style = SoundEffectsButton.OnStyle;
            SoundEffectsButton.OnRelease += (x, y) => { SoundEffectsOn = SoundEffectsButton.On; };
            SoundEffectsButton.On = SoundEffectsOn;
            Components.Add(SoundEffectsButton);

            SoundEffectsButtonText = new TextSprite(Config.MenuButtonTextFont, "Sound Effects", SoundEffectsButton.Position + new Vector2(SoundEffectsButton.Width, -SoundEffectsButton.Height / 2)) { CenterAsOrigin = false };
            Components.Add(SoundEffectsButtonText);

            FullscreenButton = new ButtonToggle("grey_boxCheckmark", new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.75f)) {  Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            FullscreenButton.OnStyle.AllTextures = EdgeGame.GetTexture("grey_boxCheckmark");
            FullscreenButton.OnStyle.AllColors = Config.MenuButtonColor;
            FullscreenButton.OffStyle.AllTextures = EdgeGame.GetTexture("grey_box");
            FullscreenButton.OffStyle.AllColors = Config.MenuButtonColor;
            FullscreenButton.Style = FullscreenButton.OnStyle;
            FullscreenButton.OnRelease += (x, y) => { FullscreenOn = FullscreenButton.On; };
            FullscreenButton.On = FullscreenOn;
            Components.Add(FullscreenButton);

            FullscreenButtonText = new TextSprite(Config.MenuButtonTextFont, "Fullscreen", FullscreenButton.Position + new Vector2(FullscreenButton.Width, -FullscreenButton.Height / 2)) { CenterAsOrigin = false };
            Components.Add(FullscreenButtonText);

            ParticlesButton = new ButtonToggle("grey_boxCheckmark", new Vector2(EdgeGame.WindowSize.X * 0.75f, EdgeGame.WindowSize.Y * 0.75f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            ParticlesButton.OnStyle.AllTextures = EdgeGame.GetTexture("grey_boxCheckmark");
            ParticlesButton.OnStyle.AllColors = Config.MenuButtonColor;
            ParticlesButton.OffStyle.AllTextures = EdgeGame.GetTexture("grey_box");
            ParticlesButton.OffStyle.AllColors = Config.MenuButtonColor;
            ParticlesButton.Style = ParticlesButton.OnStyle;
            ParticlesButton.OnRelease += (x, y) => { ParticlesOn = ParticlesButton.On; };
            ParticlesButton.On = ParticlesOn;
            Components.Add(ParticlesButton);

            ParticlesButtonText = new TextSprite(Config.MenuButtonTextFont, "Particles", ParticlesButton.Position + new Vector2(ParticlesButton.Width, -ParticlesButton.Height / 2)) { CenterAsOrigin = false };
            Components.Add(ParticlesButtonText);

            ReturnButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.7f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.1f) };
            ReturnButton.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            ReturnButton.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            ReturnButton.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            ReturnButton.Style.AllColors = Config.MenuButtonColor;
            ReturnButton.OnRelease += (x, y) =>
            {
                MenuManager.SwitchMenu(MenuManager.PreviousMenu.Name);
            };
            Components.Add(ReturnButton);

            ReturnButtonText = new TextSprite(Config.MenuButtonTextFont, "Return to Game", ReturnButton.Position);
            Components.Add(ReturnButtonText);

            Input.OnKeyRelease += Input_OnKeyRelease;
        }

        void Input_OnKeyRelease(Microsoft.Xna.Framework.Input.Keys key)
        {
            if (MenuManager.SelectedMenu == this && key == Config.BackKey)
            {
                MenuManager.SwitchMenu(MenuManager.PreviousMenu.Name);
            }
        }
    }
}
