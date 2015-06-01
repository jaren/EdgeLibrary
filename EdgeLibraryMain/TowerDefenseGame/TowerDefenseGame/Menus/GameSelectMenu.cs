using EdgeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class GameSelectMenu : MenuBase
    {
        public List<Level> Levels;
        public int CurrentIndex;
        public Difficulty CurrentDifficulty;

        public Sprite LevelPreview;
        public TextSprite LevelName;
        public TextSprite LevelDifficulty;
        public TextSprite LevelDescription;
        public TextSprite DifficultyName;

        public Button DifficultyLessButton;
        public Button DifficultyMoreButton;
        public Button LevelLessButton;
        public Button LevelMoreButton;
        public Button StartButton;
        public Button BackButton;

        public GameSelectMenu()
            : base("GameSelectMenu")
        {
            CurrentIndex = 0;
            CurrentDifficulty = Difficulty.Medium;

            TextSprite title = new TextSprite(Config.MenuTitleFont, "Game Select", new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(title);

            Style buttonStyle = new Style(EdgeGame.GetTexture(Config.ButtonMouseOverTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonNormalTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonClickTexture), Config.MenuButtonColor);

            Levels = new List<Level>()
            {
                Level.ImportLevel("Levels/Grassy Plains", "Grassy Plains", "Grassy Plains", Config.TrackEasyDifficulty, "A tranquil meandering path"),
                Level.ImportLevel("Levels/Village Loop", "Village Loop", "Village Loop", Config.TrackEasyDifficulty, "Enemies circle twice around a peaceful village"),
                Level.ImportLevel("Levels/Islands", "Islands", "Islands", Config.TrackMediumDifficulty, "Scattered islands in an ocean"),
                Level.ImportLevel("Levels/Rocky Bridges", "Rocky Bridges", "Rocky Bridges", Config.TrackHardDifficulty, "A harrowing journey through an abandoned mine")
            };

            Sprite levelPreviewBacking = new Sprite("panelInset_beige", new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.3f), Color.White, new Vector2(3.2f));
            Components.Add(levelPreviewBacking);

            LevelPreview = new Sprite("", levelPreviewBacking.Position, Color.White, new Vector2(1.0f));
            LevelPreview.Texture = Levels[CurrentIndex].Texture;
            Components.Add(LevelPreview);

            LevelLessButton = new Button("ShadedDark24", new Vector2(EdgeGame.WindowSize.X * 0.25f, EdgeGame.WindowSize.Y * 0.3f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.2f) };
            LevelLessButton.OnRelease += (x, y) =>
            {
                if (CurrentIndex > 0)
                {
                    CurrentIndex--;
                }
                else
                {
                    CurrentIndex = Levels.Count - 1;
                }

                LevelPreview.Texture = Levels[CurrentIndex].Texture;
                LevelName.Text = Levels[CurrentIndex].Name;
                LevelDifficulty.Text = Levels[CurrentIndex].Difficulty;
                LevelDescription.Text = Levels[CurrentIndex].Description;
            };
            LevelLessButton.Style.NormalTexture = EdgeGame.GetTexture("ShadedDark24");
            LevelLessButton.Style.ClickTexture = EdgeGame.GetTexture("FlatDark23");
            LevelLessButton.Style.MouseOverTexture = EdgeGame.GetTexture("ShadedDark24");
            LevelLessButton.Style.AllColors = Config.MenuButtonColor;
            Components.Add(LevelLessButton);

            LevelMoreButton = new Button("ShadedDark25", new Vector2(EdgeGame.WindowSize.X * 0.75f, EdgeGame.WindowSize.Y * 0.3f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.2f) };
            LevelMoreButton.OnRelease += (x, y) =>
            {
                if (CurrentIndex < (Levels.Count - 1))
                {
                    CurrentIndex++;
                }
                else
                {
                    CurrentIndex = 0;
                }

                LevelPreview.Texture = Levels[CurrentIndex].Texture;
                LevelName.Text = Levels[CurrentIndex].Name;
                LevelDifficulty.Text = Levels[CurrentIndex].Difficulty;
                LevelDescription.Text = Levels[CurrentIndex].Description;
            };
            LevelMoreButton.Style.NormalTexture = EdgeGame.GetTexture("ShadedDark25");
            LevelMoreButton.Style.ClickTexture = EdgeGame.GetTexture("FlatDark24");
            LevelMoreButton.Style.MouseOverTexture = EdgeGame.GetTexture("ShadedDark25");
            LevelMoreButton.Style.AllColors = Config.MenuButtonColor;
            Components.Add(LevelMoreButton);

            LevelName = new TextSprite(Config.MenuSubtitleFont, Levels[CurrentIndex].Name, new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.5f), Config.MenuButtonColor, Vector2.One);
            Components.Add(LevelName);

            LevelDifficulty = new TextSprite(Config.MenuSubtitleFont, Levels[CurrentIndex].Difficulty, new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.54f), Config.MenuButtonColor, Vector2.One);
            Components.Add(LevelDifficulty);

            LevelDescription = new TextSprite(Config.MenuSubtitleFont, Levels[CurrentIndex].Description, new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.58f), Config.MenuButtonColor, Vector2.One);
            Components.Add(LevelDescription);

            Sprite difficultyBacking = new Sprite(Config.ButtonClickTexture, new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.65f), Config.MenuButtonColor, new Vector2(1.5f));
            Components.Add(difficultyBacking);

            DifficultyName = new TextSprite(Config.MenuMiniTitleFont, "Medium", difficultyBacking.Position, Color.White, Vector2.One);
            Components.Add(DifficultyName);

            StartButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.8f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1.2f) };
            StartButton.OnRelease += (x, y) => { GameMenu.CurrentLevel = Levels[CurrentIndex]; GameMenu.Difficulty = CurrentDifficulty; GameMenu.ShouldReset = true; MenuManager.SwitchMenu("GameMenu"); };
            StartButton.Style = buttonStyle;
            Components.Add(StartButton);

            TextSprite startButtonText = new TextSprite(Config.MenuButtonTextFont, "START GAME", StartButton.Position);
            Components.Add(startButtonText);

            BackButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.9f)) { Color = Config.MenuButtonColor, Scale = new Vector2(1f) };
            BackButton.OnRelease += (x, y) => { MenuManager.SwitchMenu(MenuManager.PreviousMenu.Name); };
            BackButton.Style = buttonStyle;
            Components.Add(BackButton);

            TextSprite backButtonText = new TextSprite(Config.MenuButtonTextFont, "BACK", BackButton.Position);
            Components.Add(backButtonText);

            DifficultyLessButton = new Button("ShadedDark24", new Vector2(EdgeGame.WindowSize.X * 0.35f, EdgeGame.WindowSize.Y * 0.65f)) { Color = Config.MenuButtonColor, Scale = new Vector2(0.8f) };
            DifficultyLessButton.OnRelease += (x, y) =>
            {
                if (CurrentDifficulty == Difficulty.Medium)
                {
                    CurrentDifficulty = Difficulty.Easy;
                }
                else if (CurrentDifficulty == Difficulty.Hard)
                {
                    CurrentDifficulty = Difficulty.Medium;
                }
                DifficultyName.Text = CurrentDifficulty == Difficulty.Easy ? "Easy" : CurrentDifficulty == Difficulty.Medium ? "Medium" : CurrentDifficulty == Difficulty.Hard ? "Hard" : "Error";
            };
            DifficultyLessButton.Style.NormalTexture = EdgeGame.GetTexture("ShadedDark24");
            DifficultyLessButton.Style.ClickTexture = EdgeGame.GetTexture("FlatDark23");
            DifficultyLessButton.Style.MouseOverTexture = EdgeGame.GetTexture("ShadedDark24");
            DifficultyLessButton.Style.AllColors = Config.MenuButtonColor;
            Components.Add(DifficultyLessButton);

            DifficultyMoreButton = new Button("ShadedDark25", new Vector2(EdgeGame.WindowSize.X * 0.65f, EdgeGame.WindowSize.Y * 0.65f)) { Color = Config.MenuButtonColor, Scale = new Vector2(0.8f) };
            DifficultyMoreButton.OnRelease += (x, y) =>
            {
                if (CurrentDifficulty == Difficulty.Medium)
                {
                    CurrentDifficulty = Difficulty.Hard;
                }
                else if (CurrentDifficulty == Difficulty.Easy)
                {
                    CurrentDifficulty = Difficulty.Medium;
                }
                DifficultyName.Text = CurrentDifficulty == Difficulty.Easy ? "Easy" : CurrentDifficulty == Difficulty.Medium ? "Medium" : CurrentDifficulty == Difficulty.Hard ? "Hard" : "Error";
            };
            DifficultyMoreButton.Style.NormalTexture = EdgeGame.GetTexture("ShadedDark25");
            DifficultyMoreButton.Style.ClickTexture = EdgeGame.GetTexture("FlatDark24");
            DifficultyMoreButton.Style.MouseOverTexture = EdgeGame.GetTexture("ShadedDark25");
            DifficultyMoreButton.Style.AllColors = Config.MenuButtonColor;
            Components.Add(DifficultyMoreButton);

            Input.OnKeyRelease += Input_OnKeyRelease;
        }

        void Input_OnKeyRelease(Keys key)
        {
            if (MenuManager.SelectedMenu == this && key == Config.BackKey && !MenuManager.InputEventHandled)
            {
                MenuManager.SwitchMenu(MenuManager.PreviousMenu.Name);
                MenuManager.InputEventHandled = true;
            }
        }
    }
}
