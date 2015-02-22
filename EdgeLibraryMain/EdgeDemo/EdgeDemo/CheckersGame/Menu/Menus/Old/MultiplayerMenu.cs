using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;

namespace EdgeDemo.CheckersGame
{
    public class MultiplayerMenu : MenuBase
    {
        public MultiplayerMenu()
            : base("MultiplayerMenu")
        {
            TextSprite title = new TextSprite(Config.MenuTitleFont, "Multiplayer Game", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(title);

            TextSprite subTitle = new TextSprite(Config.MenuSubtitleFont, "Click!", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.1f)) { Color = Config.MenuTextColor };
            Components.Add(subTitle);

            Button hostButton = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.7f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            hostButton.Style.NormalTexture = EdgeGame.GetTexture("grey_button00");
            hostButton.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button02");
            hostButton.Style.ClickTexture = EdgeGame.GetTexture("grey_button01");
            hostButton.Style.AllColors = Config.MenuButtonColor;
            hostButton.OnRelease += (x, y) => { BoardManager.ResetGame = true; MenuManager.SwitchMenu("HostGameMenu"); };
            Components.Add(hostButton);

            Button joinButton = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.8f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            joinButton.Style.NormalTexture = EdgeGame.GetTexture("grey_button00");
            joinButton.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button02");
            joinButton.Style.ClickTexture = EdgeGame.GetTexture("grey_button01");
            joinButton.Style.AllColors = Config.MenuButtonColor;
            joinButton.OnRelease += (x, y) => { BoardManager.ResetGame = true; MenuManager.SwitchMenu("JoinGameMenu"); };
            Components.Add(joinButton);

            TextSprite hostButtonText = new TextSprite(Config.MenuButtonTextFont, "Host Game", hostButton.Position);
            Components.Add(hostButtonText);

            TextSprite joinButtonText = new TextSprite(Config.MenuButtonTextFont, "Join Game", joinButton.Position);
            Components.Add(joinButtonText);

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
