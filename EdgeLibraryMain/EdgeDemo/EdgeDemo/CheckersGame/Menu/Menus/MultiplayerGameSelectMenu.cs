using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;
using EdgeDemo.CheckersService;

namespace EdgeDemo.CheckersGame
{
    public class MultiplayerGameSelectMenu : ToGameMenu
    {
        private List<int> gameIDs;
        private CheckersServiceClient ServiceClient;

        private TextBox enterNameBox;

        private Button hostButton;
        private TextSprite hostButtonText;
        private Button join1Button;
        private TextSprite join1ButtonText;
        private Button join2Button;
        private TextSprite join2ButtonText;
        private Button join3Button;
        private TextSprite join3ButtonText;
        private Button refreshButton;
        private TextSprite refreshButtonText;
        private List<string> buttonTexts;
        private Dictionary<int, GameState> joinableGames;

        public MultiplayerGameSelectMenu()
            : base("MultiplayerGameSelectMenu")
        {
            ServiceClient = new CheckersServiceClient();

            TextSprite title = new TextSprite(Config.MenuTitleFont, "Multiplayer Game", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(title);

            TextSprite subTitle = new TextSprite(Config.MenuSubtitleFont, "Join or Host a Game", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.1f)) { Color = Config.MenuTextColor };
            Components.Add(subTitle);

            enterNameBox = new TextBox("grey_button00", Config.MenuButtonTextFont, new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.2f)) { Color = Color.LightGray, Scale = new Vector2(2, 1.25f) };
            enterNameBox.DefaultText = "Enter your team name here";
            enterNameBox.Style.NormalTexture = EdgeGame.GetTexture("grey_button00");
            enterNameBox.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button01");
            enterNameBox.Style.ClickTexture = EdgeGame.GetTexture("grey_button02");
            enterNameBox.Style.AllColors = Color.LightGray;
            Components.Add(enterNameBox);

            #region HOSTGAME


            hostButton = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X * 0.8f, EdgeGame.WindowSize.Y * 0.8f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            hostButton.Style.NormalTexture = EdgeGame.GetTexture("grey_button00");
            hostButton.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button02");
            hostButton.Style.ClickTexture = EdgeGame.GetTexture("grey_button01");
            hostButton.Style.AllColors = Config.MenuButtonColor;
            hostButton.OnRelease += (x, y) =>
            {
                if (!enterNameBox.TextSpriteBlank)
                {
                    Player1 = new WebPlayer(enterNameBox.TextSprite.Text, false);
                    Player2 = new NormalPlayer();
                    BoardManager.ResetGame = true;
                    MenuManager.SwitchMenu("GameMenu");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please enter a team name into the text box", "Invalid team name", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            };
            Components.Add(hostButton);

            hostButtonText = new TextSprite(Config.MenuButtonTextFont, "Host a Game", hostButton.Position);
            Components.Add(hostButtonText);

            #endregion

            #region JOINGAME
            gameIDs = new List<int>(3);

            join1Button = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X * 0.8f, EdgeGame.WindowSize.Y * 0.4f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            join1Button.Style.NormalTexture = EdgeGame.GetTexture("grey_button00");
            join1Button.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button02");
            join1Button.Style.ClickTexture = EdgeGame.GetTexture("grey_button01");
            join1Button.Style.AllColors = Config.MenuButtonColor;
            join1Button.OnRelease += (x, y) =>
            {
                if (!enterNameBox.TextSpriteBlank)
                {
                    if (ServiceClient.GetSpecificGames(GameState.State.WaitingForPlayers).Count > 0 && ServiceClient.GetAllGames().ElementAt(gameIDs[0]).GameInfo == GameState.State.WaitingForPlayers)
                    {
                        BoardManager.ResetGame = true;
                        Player1 = new NormalPlayer();
                        Player2 = new WebPlayer(gameIDs[0], "Other Team", false);
                        MenuManager.SwitchMenu("GameMenu");
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("That game is no longer joinable or does not exist", "Not Joinable", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please enter a team name into the text box", "Invalid team name", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            };
            Components.Add(join1Button);

            join2Button = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X * 0.8f, EdgeGame.WindowSize.Y * 0.5f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            join2Button.Style.NormalTexture = EdgeGame.GetTexture("grey_button00");
            join2Button.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button02");
            join2Button.Style.ClickTexture = EdgeGame.GetTexture("grey_button01");
            join2Button.Style.AllColors = Config.MenuButtonColor;
            join2Button.OnRelease += (x, y) =>
            {
                if (!enterNameBox.TextSpriteBlank)
                {
                    if (ServiceClient.GetSpecificGames(GameState.State.WaitingForPlayers).Count > 1 && ServiceClient.GetAllGames().ElementAt(gameIDs[1]).GameInfo == GameState.State.WaitingForPlayers)
                    {
                        BoardManager.ResetGame = true;
                        Player1 = new NormalPlayer();
                        Player2 = new WebPlayer(gameIDs[1], "Other Team", false);
                        MenuManager.SwitchMenu("GameMenu");
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("That game is no longer joinable or does not exist", "Not Joinable", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please enter a team name into the text box", "Invalid team name", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            };
            Components.Add(join2Button);

            join3Button = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X * 0.8f, EdgeGame.WindowSize.Y * 0.6f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            join3Button.Style.NormalTexture = EdgeGame.GetTexture("grey_button00");
            join3Button.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button02");
            join3Button.Style.ClickTexture = EdgeGame.GetTexture("grey_button01");
            join3Button.Style.AllColors = Config.MenuButtonColor;
            join3Button.OnRelease += (x, y) =>
            {
                if (!enterNameBox.TextSpriteBlank)
                {
                    if (ServiceClient.GetSpecificGames(GameState.State.WaitingForPlayers).Count > 2 && ServiceClient.GetAllGames().ElementAt(gameIDs[2]).GameInfo == GameState.State.WaitingForPlayers)
                    {
                        BoardManager.ResetGame = true;
                        Player1 = new NormalPlayer();
                        Player2 = new WebPlayer(gameIDs[2], "Other Team", false);
                        MenuManager.SwitchMenu("GameMenu");
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("That game is no longer joinable or does not exist", "Not Joinable", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please enter a team name into the text box", "Invalid team name", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            };
            Components.Add(join3Button);


            join1ButtonText = new TextSprite(Config.MenuButtonTextFont, "No Game", join1Button.Position);
            Components.Add(join1ButtonText);

            join2ButtonText = new TextSprite(Config.MenuButtonTextFont, "No Game", join2Button.Position);
            Components.Add(join2ButtonText);

            join3ButtonText = new TextSprite(Config.MenuButtonTextFont, "No Game", join3Button.Position);
            Components.Add(join3ButtonText);

            refreshButton = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.5f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            refreshButton.Style.NormalTexture = EdgeGame.GetTexture("grey_button00");
            refreshButton.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button02");
            refreshButton.Style.ClickTexture = EdgeGame.GetTexture("grey_button01");
            refreshButton.Style.AllColors = Config.MenuButtonColor;
            refreshButton.OnRelease += (x, y) =>
            {
                Refresh();
            };
            Components.Add(refreshButton);

            refreshButtonText = new TextSprite(Config.MenuButtonTextFont, "Refresh", refreshButton.Position);
            Components.Add(refreshButtonText);
            #endregion

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
            Refresh();
            base.SwitchTo();
        }

        private void Refresh()
        {
            gameIDs = new List<int>(3);
            buttonTexts = new List<string>();
            try
            {
                joinableGames = ServiceClient.GetSpecificGames(GameState.State.WaitingForPlayers);
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

                join1ButtonText.Text = "Join: " + buttonTexts[0];
                join2ButtonText.Text = "Join: " + buttonTexts[1];
                join3ButtonText.Text = "Join: " + buttonTexts[2];
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("The multiplayer service is not available. Please try again later.", "Multiplayer not Available", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
            }
        }
    }
}
