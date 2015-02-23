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
        public TextSprite Title;
        public TextSprite SubTitle;
        public Button QuitButton;
        public TextSprite QuitButtonText;
        public ButtonToggle MusicButton;
        public TextSprite MusicButtonText;
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

            QuitButton = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.85f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            QuitButton.Style.NormalTexture = EdgeGame.GetTexture("grey_button00");
            QuitButton.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button02");
            QuitButton.Style.ClickTexture = EdgeGame.GetTexture("grey_button01");
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

            MusicButton = new ButtonToggle("grey_boxCheckmark", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X * 0.45f, EdgeGame.WindowSize.Y * 0.5f)) {  Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            MusicButton.OnStyle.AllTextures = EdgeGame.GetTexture("grey_boxCheckmark");
            MusicButton.OnStyle.AllColors = Config.MenuButtonColor;
            MusicButton.OffStyle.AllTextures = EdgeGame.GetTexture("grey_box");
            MusicButton.OffStyle.AllColors = Config.MenuButtonColor;
            MusicButton.Style = MusicButton.OnStyle;
            MusicButton.On = true;
            Components.Add(MusicButton);

            MusicButtonText = new TextSprite(Config.MenuButtonTextFont, "Music", MusicButton.Position + new Vector2(MusicButton.Width, -MusicButton.Height / 2)) { CenterAsOrigin = false };
            Components.Add(MusicButtonText);

            ReturnButton = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.7f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.1f) };
            ReturnButton.Style.NormalTexture = EdgeGame.GetTexture("grey_button00");
            ReturnButton.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button02");
            ReturnButton.Style.ClickTexture = EdgeGame.GetTexture("grey_button01");
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
