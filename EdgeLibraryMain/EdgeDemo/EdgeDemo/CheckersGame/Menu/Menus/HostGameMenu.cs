using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;
using EdgeDemo.CheckersService;

namespace EdgeDemo.CheckersGame
{
    class HostGameMenu : MenuBase
    {
        public HostGameMenu() : base("HostGameMenu")
        {
            CheckersServiceClient ServiceClient = new CheckersServiceClient();

            TextSprite title = new TextSprite(Config.MenuTitleFont, "Host Game", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(title);

            TextSprite subTitle = new TextSprite(Config.MenuSubtitleFont, "Waiting For Players\nToDo: TextBox for host team name", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.1f)) { Color = Config.MenuTextColor };
            Components.Add(subTitle);

            
            Button hostButton = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.7f)) { ClickTexture = EdgeGame.GetTexture("grey_button01"), MouseOverTexture = EdgeGame.GetTexture("grey_button02"), Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            hostButton.SetColors(Config.MenuButtonColor);
            hostButton.OnRelease += (x, y) => { Config.IsHost = true; Config.ThisGameID = ServiceClient.CreateGame("DefaultName"); Config.ThisGameType = Config.GameType.Online; BoardManager.ResetGame = true; MenuManager.SwitchMenu("GameMenu"); };
            Components.Add(hostButton);
            /*
            Button joinButton = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.8f)) { ClickTexture = EdgeGame.GetTexture("grey_button01"), MouseOverTexture = EdgeGame.GetTexture("grey_button02"), Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            joinButton.SetColors(Config.MenuButtonColor);
            joinButton.OnRelease += (x, y) => { Config.ThisGameType = Config.GameType.Online; BoardManager.ResetGame = true; MenuManager.SwitchMenu("GameMenu"); };
            Components.Add(joinButton);
            */
            TextSprite hostButtonText = new TextSprite(Config.MenuButtonTextFont, "Host Game", hostButton.Position);
            Components.Add(hostButtonText);
            /*
            TextSprite joinButtonText = new TextSprite(Config.MenuButtonTextFont, "Join Game", joinButton.Position);
            Components.Add(joinButtonText);
            */
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
