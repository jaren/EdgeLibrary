using EdgeDemo.CheckersService;
using EdgeLibrary;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace EdgeDemo.CheckersGame
{
    public class OptionsMenu : ParticleMenu
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

        public OptionsMenu()
            : base("OptionsMenu")
        {
            CheckersServiceClient ServiceClient = new CheckersServiceClient();

            Title = new TextSprite(Config.MenuTitleFont, "Options Menu", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(Title);

            SubTitle = new TextSprite(Config.MenuSubtitleFont, "Click for Checkers! Right Click to Move Them!", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.1f)) { Color = Config.MenuTextColor };
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

            MusicButton = new ButtonToggle(MusicOn ? "grey_boxCheckmark" : "grey_boxCross", new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.25f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            MusicButton.OnStyle.AllTextures = EdgeGame.GetTexture("grey_boxCheckmark");
            MusicButton.OnStyle.AllColors = Config.MenuButtonColor;
            MusicButton.OffStyle.AllTextures = EdgeGame.GetTexture("grey_boxCross");
            MusicButton.OffStyle.AllColors = Config.MenuButtonColor;
            MusicButton.Style = MusicOn ? MusicButton.OnStyle : MusicButton.OffStyle;
            MusicButton.OnRelease += (x, y) => { MusicOn = MusicButton.On; };
            MusicButton.On = MusicOn;
            Components.Add(MusicButton);

            MusicButtonText = new TextSprite(Config.MenuButtonTextFont, "Music", MusicButton.Position + new Vector2(MusicButton.Width, -MusicButton.Height / 2)) { CenterAsOrigin = false };
            Components.Add(MusicButtonText);

            SoundEffectsButton = new ButtonToggle(SoundEffectsOn ? "grey_boxCheckmark" : "grey_boxCross", new Vector2(EdgeGame.WindowSize.X * 0.6f, EdgeGame.WindowSize.Y * 0.25f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            SoundEffectsButton.OnStyle.AllTextures = EdgeGame.GetTexture("grey_boxCheckmark");
            SoundEffectsButton.OnStyle.AllColors = Config.MenuButtonColor;
            SoundEffectsButton.OffStyle.AllTextures = EdgeGame.GetTexture("grey_boxCross");
            SoundEffectsButton.OffStyle.AllColors = Config.MenuButtonColor;
            SoundEffectsButton.Style = SoundEffectsOn ? SoundEffectsButton.OnStyle : SoundEffectsButton.OffStyle;
            SoundEffectsButton.OnRelease += (x, y) => { SoundEffectsOn = SoundEffectsButton.On; };
            SoundEffectsButton.On = SoundEffectsOn;
            Components.Add(SoundEffectsButton);

            SoundEffectsButtonText = new TextSprite(Config.MenuButtonTextFont, "Sound Effects", SoundEffectsButton.Position + new Vector2(SoundEffectsButton.Width, -SoundEffectsButton.Height / 2)) { CenterAsOrigin = false };
            Components.Add(SoundEffectsButtonText);

            FullscreenButton = new ButtonToggle(FullscreenOn ? "grey_boxCheckmark" : "grey_boxCross", new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.6f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            FullscreenButton.OnStyle.AllTextures = EdgeGame.GetTexture("grey_boxCheckmark");
            FullscreenButton.OnStyle.AllColors = Config.MenuButtonColor;
            FullscreenButton.OffStyle.AllTextures = EdgeGame.GetTexture("grey_boxCross");
            FullscreenButton.OffStyle.AllColors = Config.MenuButtonColor;
            FullscreenButton.Style = FullscreenOn ? FullscreenButton.OnStyle : FullscreenButton.OffStyle;
            FullscreenButton.OnRelease += (x, y) => { FullscreenOn = FullscreenButton.On; };
            FullscreenButton.On = FullscreenOn;
            Components.Add(FullscreenButton);

            FullscreenButtonText = new TextSprite(Config.MenuButtonTextFont, "Fullscreen", FullscreenButton.Position + new Vector2(FullscreenButton.Width, -FullscreenButton.Height / 2)) { CenterAsOrigin = false };
            Components.Add(FullscreenButtonText);

            ParticlesButton = new ButtonToggle(ParticlesOn ? "grey_boxCheckmark" : "grey_boxCross", new Vector2(EdgeGame.WindowSize.X * 0.6f, EdgeGame.WindowSize.Y * 0.6f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            ParticlesButton.OnStyle.AllTextures = EdgeGame.GetTexture("grey_boxCheckmark");
            ParticlesButton.OnStyle.AllColors = Config.MenuButtonColor;
            ParticlesButton.OffStyle.AllTextures = EdgeGame.GetTexture("grey_boxCross");
            ParticlesButton.OffStyle.AllColors = Config.MenuButtonColor;
            ParticlesButton.Style = ParticlesOn ? ParticlesButton.OnStyle : ParticlesButton.OffStyle;
            ParticlesButton.OnRelease += (x, y) => { ParticlesOn = ParticlesButton.On; };
            ParticlesButton.On = ParticlesOn;
            Components.Add(ParticlesButton);

            ParticlesButtonText = new TextSprite(Config.MenuButtonTextFont, "Particles", ParticlesButton.Position + new Vector2(ParticlesButton.Width, -ParticlesButton.Height / 2)) { CenterAsOrigin = false };
            Components.Add(ParticlesButtonText);

            ReturnButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.95f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.1f) };
            ReturnButton.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            ReturnButton.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            ReturnButton.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            ReturnButton.Style.AllColors = Config.MenuButtonColor;
            ReturnButton.OnRelease += (x, y) =>
            {
                BoardManager.ResetGame = false;
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

        public override void SwitchTo()
        {
            base.SwitchTo();

            MusicButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (MusicButton.Width * MusicButton.Scale.X).ToSimUnits(), (MusicButton.Height * MusicButton.Scale.Y).ToSimUnits(), 1));
            MusicButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            SoundEffectsButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (SoundEffectsButton.Width * SoundEffectsButton.Scale.X).ToSimUnits(), (SoundEffectsButton.Height * SoundEffectsButton.Scale.Y).ToSimUnits(), 1));
            SoundEffectsButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            FullscreenButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (FullscreenButton.Width * FullscreenButton.Scale.X).ToSimUnits(), (FullscreenButton.Height * FullscreenButton.Scale.Y).ToSimUnits(), 1));
            FullscreenButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            ParticlesButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (ParticlesButton.Width * ParticlesButton.Scale.X).ToSimUnits(), (ParticlesButton.Height * ParticlesButton.Scale.Y).ToSimUnits(), 1));
            ParticlesButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            ReturnButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (ReturnButton.Width * ReturnButton.Scale.X).ToSimUnits(), (ReturnButton.Height * ReturnButton.Scale.Y).ToSimUnits(), 1));
            ReturnButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            QuitButton.EnablePhysics(BodyFactory.CreateRectangle(EdgeGame.World, (QuitButton.Width * QuitButton.Scale.X).ToSimUnits(), (QuitButton.Height * QuitButton.Scale.Y).ToSimUnits(), 1));
            QuitButton.Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
        }
    }
}
