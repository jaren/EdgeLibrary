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
        private Sprite refreshButtonTexture;
        private List<string> buttonTexts;
        private Dictionary<int, GameState> joinableGames;

        public MultiplayerGameSelectMenu()
            : base("MultiplayerGameSelectMenu")
        {
            try
            {
                ServiceClient = new CheckersServiceClient();

                TextSprite title = new TextSprite(Config.MenuTitleFont, "Multiplayer Game", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
                Components.Add(title);

                enterNameBox = new TextBox(Config.ButtonNormalTexture, Config.MenuButtonTextFont, new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.2f)) { Color = Config.MenuButtonColor, Scale = new Vector2(2, 1.25f) };
                enterNameBox.DefaultText = "Enter your team name here";
                enterNameBox.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
                enterNameBox.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
                enterNameBox.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
                enterNameBox.Style.AllColors = Config.MenuButtonColor;
                Components.Add(enterNameBox);

                #region HOSTGAME
                TextSprite hostText = new TextSprite(Config.MenuSubtitleFont, "Host a Game", new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.35f));
                Components.Add(hostText);

                hostButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.45f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
                hostButton.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
                hostButton.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
                hostButton.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
                hostButton.Style.AllColors = Config.MenuButtonColor;
                hostButton.OnRelease += (x, y) =>
                {
                    if (!enterNameBox.TextBlank)
                    {
                        try
                        {
                            ServiceClient.GetAllGames();

                            Player1 = new WebPlayer(enterNameBox.TextSprite.Text, false);
                            Player2 = new NormalPlayer("This shouldn't be seen...");
                            BoardManager.Instance.ResetGame = true;
                            MenuManager.SwitchMenu("GameMenu");
                        }
                        catch
                        {
                            if (!EdgeGame.Game.Graphics.IsFullScreen)
                            {
                                System.Windows.Forms.MessageBox.Show("The multiplayer service is not available. Please try again later.", "Multiplayer not Available", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        if (!EdgeGame.Game.Graphics.IsFullScreen)
                        {
                            System.Windows.Forms.MessageBox.Show("Please enter a team name into the text box", "Invalid team name", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        }
                    }
                };
                Components.Add(hostButton);

                hostButtonText = new TextSprite(Config.MenuButtonTextFont, "Host a Game", hostButton.Position);
                Components.Add(hostButtonText);

                #endregion

                #region JOINGAME
                gameIDs = new List<int>(3);

                TextSprite joinText = new TextSprite(Config.MenuSubtitleFont, "Join a Game", new Vector2(EdgeGame.WindowSize.X * 0.75f, EdgeGame.WindowSize.Y * 0.35f));
                Components.Add(joinText);

                join1Button = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.8f, EdgeGame.WindowSize.Y * 0.45f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
                join1Button.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
                join1Button.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
                join1Button.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
                join1Button.Style.AllColors = Config.MenuButtonColor;
                join1Button.OnRelease += (x, y) =>
                {
                    if (!enterNameBox.TextBlank)
                    {
                        if (ServiceClient.GetSpecificGames(GameState.State.WaitingForPlayers).Count > 0 && ServiceClient.GetAllGames().ElementAt(gameIDs[0]).GameInfo == GameState.State.WaitingForPlayers)
                        {
                            BoardManager.Instance.ResetGame = true;
                            Player1 = new NormalPlayer("This shouldn't be seen...");
                            Player2 = new WebPlayer(enterNameBox.Text, gameIDs[0], false);
                            MenuManager.SwitchMenu("GameMenu");
                        }
                        else
                        {
                            if (!EdgeGame.Game.Graphics.IsFullScreen)
                            {
                                System.Windows.Forms.MessageBox.Show("That game is no longer joinable or does not exist", "Not Joinable", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        if (!EdgeGame.Game.Graphics.IsFullScreen)
                        {
                            System.Windows.Forms.MessageBox.Show("Please enter a team name into the text box", "Invalid team name", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        }
                    }
                };
                Components.Add(join1Button);

                join2Button = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.8f, EdgeGame.WindowSize.Y * 0.55f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
                join2Button.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
                join2Button.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
                join2Button.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
                join2Button.Style.AllColors = Config.MenuButtonColor;
                join2Button.OnRelease += (x, y) =>
                {
                    if (!enterNameBox.TextBlank)
                    {
                        if (ServiceClient.GetSpecificGames(GameState.State.WaitingForPlayers).Count > 1 && ServiceClient.GetAllGames().ElementAt(gameIDs[1]).GameInfo == GameState.State.WaitingForPlayers)
                        {
                            BoardManager.Instance.ResetGame = true;
                            Player1 = new NormalPlayer("This shouldn't be seen...");
                            Player2 = new WebPlayer(enterNameBox.Text, gameIDs[1], false);
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

                join3Button = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.8f, EdgeGame.WindowSize.Y * 0.65f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
                join3Button.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
                join3Button.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
                join3Button.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
                join3Button.Style.AllColors = Config.MenuButtonColor;
                join3Button.OnRelease += (x, y) =>
                {
                    if (!enterNameBox.TextBlank)
                    {
                        if (ServiceClient.GetSpecificGames(GameState.State.WaitingForPlayers).Count > 2 && ServiceClient.GetAllGames().ElementAt(gameIDs[2]).GameInfo == GameState.State.WaitingForPlayers)
                        {
                            BoardManager.Instance.ResetGame = true;
                            Player1 = new NormalPlayer("This shouldn't be seen...");
                            Player2 = new WebPlayer(enterNameBox.Text, gameIDs[2], false);
                            MenuManager.SwitchMenu("GameMenu");
                        }
                        else
                        {
                            if (!EdgeGame.Game.Graphics.IsFullScreen)
                            {
                                System.Windows.Forms.MessageBox.Show("That game is no longer joinable or does not exist", "Not Joinable", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        if (!EdgeGame.Game.Graphics.IsFullScreen)
                        {
                            System.Windows.Forms.MessageBox.Show("Please enter a team name into the text box", "Invalid team name", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        }
                    }
                };
                Components.Add(join3Button);


                join1ButtonText = new TextSprite(Config.MenuButtonTextFont, "No Game", join1Button.Position);
                Components.Add(join1ButtonText);

                join2ButtonText = new TextSprite(Config.MenuButtonTextFont, "No Game", join2Button.Position);
                Components.Add(join2ButtonText);

                join3ButtonText = new TextSprite(Config.MenuButtonTextFont, "No Game", join3Button.Position);
                Components.Add(join3ButtonText);

                refreshButton = new Button("grey_button07", new Vector2(EdgeGame.WindowSize.X * 0.65f, EdgeGame.WindowSize.Y * 0.55f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
                refreshButton.Style.NormalTexture = EdgeGame.GetTexture("grey_button07");
                refreshButton.Style.MouseOverTexture = EdgeGame.GetTexture("grey_button08");
                refreshButton.Style.ClickTexture = EdgeGame.GetTexture("grey_button09");
                refreshButton.Style.AllColors = Config.MenuButtonColor;
                refreshButton.OnRelease += (x, y) =>
                {
                    Refresh();
                };
                Components.Add(refreshButton);

                refreshButtonTexture = new Sprite("return_white2", refreshButton.Position) { Scale = new Vector2(0.5f) };
                Components.Add(refreshButtonTexture);
                #endregion

                Input.OnKeyRelease += (x) =>
                {
                    if (MenuManager.SelectedMenu == this && x == Config.BackKey)
                    {
                        MenuManager.SwitchMenu(MenuManager.PreviousMenu.Name);
                    }
                };
            }
            catch
            {
                if (!EdgeGame.Game.Graphics.IsFullScreen)
                {
                    System.Windows.Forms.MessageBox.Show("The multiplayer service is not available. Please try again later.", "Multiplayer not Available", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
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
                if (!EdgeGame.Game.Graphics.IsFullScreen)
                {
                    System.Windows.Forms.MessageBox.Show("The multiplayer service is not available. Please try again later.", "Multiplayer not Available", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
        }
    }
}
