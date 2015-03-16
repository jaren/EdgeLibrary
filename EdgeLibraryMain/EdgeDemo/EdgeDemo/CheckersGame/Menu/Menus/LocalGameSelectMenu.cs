using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;

namespace EdgeDemo.CheckersGame
{
    public class LocalGameSelectMenu : MenuBase
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
            player1NameBox.DefaultText = "Player 1 Name";
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
            player1DifficultyBox.Styles.Add(player1DifficultyBox.Style);
            Components.Add(player1DifficultyBox);

            
            TextSprite player1DifficultyDisplay = new TextSprite(Config.MenuButtonTextFont, "Easy", player1DifficultyBox.Position);
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
            Components.Add(player1DifficultyLabel);

            player1Button.OnRelease += (x, y) => 
            {
                player1ButtonText.Text = ((ButtonToggle)x).On ? "Computer" : "Human";

                if (((ButtonToggle)x).On)
                {

                }
            };
            #endregion

            Button startButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.85f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            startButton.Style.NormalTexture = EdgeGame.GetTexture(Config.ButtonNormalTexture);
            startButton.Style.MouseOverTexture = EdgeGame.GetTexture(Config.ButtonMouseOverTexture);
            startButton.Style.ClickTexture = EdgeGame.GetTexture(Config.ButtonClickTexture);
            startButton.Style.AllColors = Config.MenuButtonColor;
            startButton.OnRelease += (x, y) => { BoardManager.ResetGame = true; MenuManager.SwitchMenu("GameMenu"); };
            Components.Add(startButton);

            TextSprite startButtonText = new TextSprite(Config.MenuButtonTextFont, "Start Game", startButton.Position);
            Components.Add(startButtonText);

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
