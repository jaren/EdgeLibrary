using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;

namespace EdgeDemo.CheckersGame
{
    public class LocalGameSelectMenu : ToGameMenu
    {
        public LocalGameSelectMenu()
            : base("LocalGameSelectMenu")
        {
            TextSprite title = new TextSprite(Config.MenuTitleFont, "Local Game", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(title);

            TextSprite subTitle = new TextSprite(Config.MenuSubtitleFont, "Choose Players!", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.1f)) { Color = Config.MenuTextColor };
            Components.Add(subTitle);

            #region PLAYER1
            TextSprite player1Label = new TextSprite(Config.MenuButtonTextFont, "Player 1 Type", new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.2f));
            Components.Add(player1Label);

            ButtonToggle player1Button = new ButtonToggle(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.25f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            player1Button.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            player1Button.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            player1Button.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            player1Button.Style.AllColors = Config.MenuButtonColor;
            player1Button.OnStyle = player1Button.Style;
            player1Button.OffStyle = player1Button.Style;
            Components.Add(player1Button);

            TextSprite player1ButtonText = new TextSprite(Config.MenuButtonTextFont, "Human", player1Button.Position);
            Components.Add(player1ButtonText);

            TextBox player1NameBox = new TextBox(Config.ButtonNormalTexture, Config.MenuButtonTextFont, new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.45f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            player1NameBox.DefaultText = "Player 1";
            player1NameBox.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            player1NameBox.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            player1NameBox.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            player1NameBox.Style.AllColors = Config.MenuButtonColor;
            Components.Add(player1NameBox);

            TextSprite player1NameLabel = new TextSprite(Config.MenuButtonTextFont, "Player 1 Name", new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.4f));
            Components.Add(player1NameLabel);

            ButtonMultiToggle player1DifficultyBox = new ButtonMultiToggle(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.65f), 2) { Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            player1DifficultyBox.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            player1DifficultyBox.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            player1DifficultyBox.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            player1DifficultyBox.Style.AllColors = Config.MenuButtonColor;
            player1DifficultyBox.Styles[0] = player1DifficultyBox.Style;
            player1DifficultyBox.Visible = false;
            Components.Add(player1DifficultyBox);

            
            TextSprite player1DifficultyDisplay = new TextSprite(Config.MenuButtonTextFont, "Easy", player1DifficultyBox.Position);
            player1DifficultyDisplay.Color = Color.Transparent;
            Components.Add(player1DifficultyDisplay);

            player1DifficultyBox.OnToggled += (x, y) =>
            {
                switch (((ButtonMultiToggle)x).CurrentIndex)
                {
                    case 0:
                        player1DifficultyDisplay.Text = "Easy";
                        break;
                    case 1:
                        player1DifficultyDisplay.Text = "Medium";
                        break;
                    case 2:
                        player1DifficultyDisplay.Text = "Hard";
                        break;
                }
            };

            TextSprite player1DifficultyLabel = new TextSprite(Config.MenuButtonTextFont, "Player 1 Difficulty", new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.6f));
            player1DifficultyLabel.Color = Color.Transparent;
            Components.Add(player1DifficultyLabel);

            player1Button.OnRelease += (x, y) =>
            {
                player1ButtonText.Text = ((ButtonToggle)x).On ? "Computer" : "Human";

                if (((ButtonToggle)x).On)
                {
                    player1DifficultyBox.Visible = true;
                    player1DifficultyDisplay.Color = Color.White;
                    player1DifficultyLabel.Color = Color.White;
                }
                else
                {
                    player1DifficultyBox.Visible = false;
                    player1DifficultyDisplay.Color = Color.Transparent;
                    player1DifficultyLabel.Color = Color.Transparent;
                }
            };
            #endregion

            #region Player2
            TextSprite player2Label = (TextSprite)player1Label.Clone();
            player2Label.Text = "Player 2 Type";
            player2Label.Position = new Vector2(EdgeGame.WindowSize.X * 0.75f, EdgeGame.WindowSize.Y * 0.2f);
            Components.Add(player2Label);
            ButtonToggle player2Button = new ButtonToggle(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.75f, EdgeGame.WindowSize.Y * 0.25f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            player2Button.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            player2Button.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            player2Button.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            player2Button.Style.AllColors = Config.MenuButtonColor;
            player2Button.OnStyle = player2Button.Style;
            player2Button.OffStyle = player2Button.Style;
            Components.Add(player2Button);
            TextSprite player2ButtonText = (TextSprite)player1ButtonText.Clone();
            player2ButtonText.Position = player2Button.Position;
            Components.Add(player2ButtonText);
            TextBox player2NameBox = new TextBox(Config.ButtonNormalTexture, Config.MenuButtonTextFont, new Vector2(EdgeGame.WindowSize.X * 0.75f, EdgeGame.WindowSize.Y * 0.45f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            player2NameBox.DefaultText = "Player 2";
            player2NameBox.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            player2NameBox.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            player2NameBox.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            player2NameBox.Style.AllColors = Config.MenuButtonColor;
            Components.Add(player2NameBox);
            TextSprite player2NameLabel = (TextSprite)player1NameLabel.Clone();
            player2NameLabel.Text = "Player 2 Name";
            player2NameLabel.Position = new Vector2(EdgeGame.WindowSize.X * 0.75f, EdgeGame.WindowSize.Y * 0.4f);
            Components.Add(player2NameLabel);
            ButtonMultiToggle player2DifficultyBox = new ButtonMultiToggle(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.75f, EdgeGame.WindowSize.Y * 0.65f), 2) { Color = Config.MenuButtonColor, Scale = new Vector2(1.25f) };
            player2DifficultyBox.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            player2DifficultyBox.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            player2DifficultyBox.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            player2DifficultyBox.Style.AllColors = Config.MenuButtonColor;
            player2DifficultyBox.Styles[0] = player2DifficultyBox.Style;
            player2DifficultyBox.Visible = false;
            Components.Add(player2DifficultyBox);
            TextSprite player2DifficultyDisplay = (TextSprite)player1DifficultyDisplay.Clone();
            player2DifficultyDisplay.Position = player2DifficultyBox.Position;
            Components.Add(player2DifficultyDisplay);
            TextSprite player2DifficultyLabel = (TextSprite)player1DifficultyLabel.Clone();
            player2DifficultyLabel.Position = new Vector2(EdgeGame.WindowSize.X * 0.75f, EdgeGame.WindowSize.Y * 0.6f);
            Components.Add(player2DifficultyLabel);

            player2DifficultyBox.OnToggled += (x, y) =>
            {
                switch (((ButtonMultiToggle)x).CurrentIndex)
                {
                    case 0:
                        player2DifficultyDisplay.Text = "Easy";
                        break;
                    case 1:
                        player2DifficultyDisplay.Text = "Medium";
                        break;
                    case 2:
                        player2DifficultyDisplay.Text = "Hard";
                        break;
                }
            };

            player2Button.OnRelease += (x, y) =>
            {
                player2ButtonText.Text = ((ButtonToggle)x).On ? "Computer" : "Human";

                if (((ButtonToggle)x).On)
                {
                    player2DifficultyBox.Visible = true;
                    player2DifficultyDisplay.Color = Color.White;
                    player2DifficultyLabel.Color = Color.White;
                }
                else
                {
                    player2DifficultyBox.Visible = false;
                    player2DifficultyDisplay.Color = Color.Transparent;
                    player2DifficultyLabel.Color = Color.Transparent;
                }
            };
            #endregion

            Button startButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.85f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            startButton.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            startButton.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            startButton.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            startButton.Style.AllColors = Config.MenuButtonColor;
            startButton.OnRelease += (x, y) => 
            {
                BoardManager.Instance.ResetGame = true; 
                
                if (player1Button.On)
                {
                    Player1 = new ComputerPlayer(player1NameBox.TextBlank ? player1NameBox.DefaultText : player1NameBox.Text, player1DifficultyBox.CurrentIndex);
                }
                else
                {
                    Player1 = new NormalPlayer(player1NameBox.TextBlank ? player1NameBox.DefaultText : player1NameBox.Text);
                }

                if (player2Button.On)
                {
                    Player2 = new ComputerPlayer(player2NameBox.TextBlank ? player2NameBox.DefaultText : player2NameBox.Text, player2DifficultyBox.CurrentIndex);
                }
                else
                {
                    Player2 = new NormalPlayer(player2NameBox.TextBlank ? player2NameBox.DefaultText : player2NameBox.Text);
                }

                MenuManager.SwitchMenu("GameMenu");
            };
            Components.Add(startButton);

            TextSprite startButtonText = new TextSprite(Config.MenuButtonTextFont, "Start Game", startButton.Position);
            Components.Add(startButtonText);

            Input.OnKeyRelease += (x) =>
            {
                if (MenuManager.SelectedMenu == this && x == Config.BackKey)
                {
                    MenuManager.SwitchMenu(MenuManager.PreviousMenu.Name);
                }
            };
        }
    }
}
