using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;
using EdgeDemo.CheckersService;

namespace EdgeDemo.CheckersGame
{
    class JoinGameMenu : MenuBase
    {
        public JoinGameMenu()
            : base("JoinGameMenu")
        {
            List<int> gameIDs = new List<int>(3);

            CheckersServiceClient ServiceClient = new CheckersServiceClient();

            TextSprite title = new TextSprite(Config.MenuTitleFont, "Join Game", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(title);

            TextSprite subTitle = new TextSprite(Config.MenuSubtitleFont, "Looking for Available Games...", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.1f)) { Color = Config.MenuTextColor };
            Components.Add(subTitle);

            Button join1Button = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.5f)) { ClickTexture = EdgeGame.GetTexture("grey_button01"), MouseOverTexture = EdgeGame.GetTexture("grey_button02"), Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            join1Button.SetColors(Config.MenuButtonColor);
            join1Button.OnRelease += (x, y) => 
            {
                if (ServiceClient.GetAllGames().ElementAt(gameIDs[0]).State == GameManager.GameState.WaitingForPlayers)
                {
                    Config.IsHost = false;
                    Config.ThisGameType = Config.GameType.Online;
                    BoardManager.ResetGame = true;
                    MenuManager.SwitchMenu("GameMenu");
                    ServiceClient.JoinGame(gameIDs[0], "OtherTeam");
                    Config.ThisGameID = gameIDs[0];
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("That game is not joinable", "Not Joinable", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            };
            Components.Add(join1Button);

            Button join2Button = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.6f)) { ClickTexture = EdgeGame.GetTexture("grey_button01"), MouseOverTexture = EdgeGame.GetTexture("grey_button02"), Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            join2Button.SetColors(Config.MenuButtonColor);
            join2Button.OnRelease += (x, y) => 
            {
                if (ServiceClient.GetAllGames().ElementAt(gameIDs[1]).State == GameManager.GameState.WaitingForPlayers)
                {
                    Config.IsHost = false;
                    Config.ThisGameType = Config.GameType.Online;
                    BoardManager.ResetGame = true;
                    MenuManager.SwitchMenu("GameMenu");
                    ServiceClient.JoinGame(gameIDs[1], "OtherTeam");
                    Config.ThisGameID = gameIDs[1];
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("That game is not joinable", "Not Joinable", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            };
            Components.Add(join2Button);

            Button join3Button = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.7f)) { ClickTexture = EdgeGame.GetTexture("grey_button01"), MouseOverTexture = EdgeGame.GetTexture("grey_button02"), Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            join3Button.SetColors(Config.MenuButtonColor);
            join3Button.OnRelease += (x, y) => 
            {
                if (ServiceClient.GetAllGames().ElementAt(gameIDs[2]).State == GameManager.GameState.WaitingForPlayers)
                {
                    Config.IsHost = false;
                    Config.ThisGameType = Config.GameType.Online;
                    BoardManager.ResetGame = true;
                    MenuManager.SwitchMenu("GameMenu");
                    ServiceClient.JoinGame(gameIDs[2], "OtherTeam");
                    Config.ThisGameID = gameIDs[2];
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("That game is not joinable", "Not Joinable", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            };
            Components.Add(join3Button);


            TextSprite join1ButtonText = new TextSprite(Config.MenuButtonTextFont, "No Game", join1Button.Position);
            Components.Add(join1ButtonText);

            TextSprite join2ButtonText = new TextSprite(Config.MenuButtonTextFont, "No Game", join2Button.Position);
            Components.Add(join2ButtonText);

            TextSprite join3ButtonText = new TextSprite(Config.MenuButtonTextFont, "No Game", join3Button.Position);
            Components.Add(join3ButtonText);

            #region InitialRefresh
            gameIDs = new List<int>(3);
            List<string> buttonTexts = new List<string>();
            Dictionary<int, GameManager> joinableGames = ServiceClient.GetJoinableGames();
            for (int i = 0; i < Components.OfType<Button>().Count(); i++)
            {
                if (joinableGames.Count >= i + 1)
                {
                    buttonTexts.Add(joinableGames.Values.ElementAt(i).HostTeamName);
                    gameIDs.Add(joinableGames.Keys.ElementAt(i));
                }
                else
                {
                    buttonTexts.Add("No Game");
                }
            }

            join1ButtonText.Text = buttonTexts[0];
            join2ButtonText.Text = buttonTexts[1];
            join3ButtonText.Text = buttonTexts[2];
            #endregion

            Button refreshButton = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.3f)) { ClickTexture = EdgeGame.GetTexture("grey_button01"), MouseOverTexture = EdgeGame.GetTexture("grey_button02"), Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            refreshButton.SetColors(Config.MenuButtonColor);
            refreshButton.OnRelease += (x, y) =>
            {
                gameIDs = new List<int>(3);
                buttonTexts = new List<string>();
                joinableGames = ServiceClient.GetJoinableGames();
                for (int i = 0; i < Components.OfType<Button>().Count() - 1; i++)
                {
                    if (joinableGames.Count >= i + 1)
                    {
                        buttonTexts.Add(joinableGames.Values.ElementAt(i).HostTeamName);
                        gameIDs.Add(joinableGames.Keys.ElementAt(i));
                    }
                    else
                    {
                        buttonTexts.Add("No Game");
                    }
                }

                join1ButtonText.Text = buttonTexts[0];
                join2ButtonText.Text = buttonTexts[1];
                join3ButtonText.Text = buttonTexts[2];
            };
            Components.Add(refreshButton);

            TextSprite refreshButtonText = new TextSprite(Config.MenuButtonTextFont, "Refresh", refreshButton.Position);
            Components.Add(refreshButtonText);

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
