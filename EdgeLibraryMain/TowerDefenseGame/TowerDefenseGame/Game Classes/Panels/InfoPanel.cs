using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public class InfoPanel : Scene
    {
        public TextSprite RoundNumber;
        public TextSprite RoundText;
        public TextSprite LivesNumber;
        public TextSprite LivesText;
        public TextSprite MoneyNumber;
        public TextSprite MoneyText;
        public TextSprite RemainingText;
        public TextSprite RemainingNumber;
        public TextSprite GameSpeedText;
        public TextSprite NextRoundText;
        public TextSprite DebugModeText;
        public TextSprite DebugGameSpeedText;

        public Button GameSpeedButton;
        public Button NextRoundButton;

        public InfoPanel()
        {
            RoundText = new TextSprite(Config.MenuSubtitleFont, "ROUND", new Vector2(EdgeGame.WindowSize.X * (Config.CommonRatio.X + (1f - Config.CommonRatio.X) / 2f), EdgeGame.WindowSize.Y * 0.05f));
            Components.Add(RoundText);

            DebugModeText = new TextSprite(Config.MenuSubtitleFont, "DEBUG", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.03f), Color.Yellow, Vector2.One);
            DebugModeText.Visible = false;
            Components.Add(DebugModeText);

            RoundNumber = new TextSprite("Georgia-60", "0", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.09f));
            Components.Add(RoundNumber);

            LivesText = new TextSprite(Config.MenuSubtitleFont, "LIVES", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.2f));
            Components.Add(LivesText);

            LivesNumber = new TextSprite("Georgia-60", "", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.24f));
            Components.Add(LivesNumber);

            MoneyText = new TextSprite(Config.MenuSubtitleFont, "MONEY", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.35f));
            Components.Add(MoneyText);

            MoneyNumber = new TextSprite("Georgia-60", "", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.39f));
            Components.Add(MoneyNumber);

            RemainingText = new TextSprite(Config.MenuSubtitleFont, "ENEMIES", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.5f));
            Components.Add(RemainingText);

            RemainingNumber = new TextSprite("Georgia-60", "0", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.54f));
            Components.Add(RemainingNumber);

            GameSpeedText = new TextSprite("Georgia-20", "GAME\nSPEED", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.64f));
            Components.Add(GameSpeedText);

            DebugGameSpeedText = new TextSprite("Georgia-30", "x" + EdgeGame.GameSpeed, new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.73f), Color.Green, Vector2.One);

            GameSpeedButton = new Button("ShadedDark25", DebugGameSpeedText.Position) { Color = Color.White, Scale = new Vector2(1f) };
            GameSpeedButton.Style.NormalTexture = EdgeGame.GetTexture("ShadedDark25");
            GameSpeedButton.Style.MouseOverTexture = EdgeGame.GetTexture("ShadedDark25");
            GameSpeedButton.Style.ClickTexture = EdgeGame.GetTexture("FlatDark24");
            GameSpeedButton.Style.AllColors = Color.White;
            Components.Add(GameSpeedButton);
            Components.Add(DebugGameSpeedText);

            NextRoundText = new TextSprite("Georgia-20", "NEXT\nROUND", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.85f));
            Components.Add(NextRoundText);

            NextRoundButton = new Button("ShadedDark25", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.94f)) { Color = Color.White, Scale = new Vector2(1f) };
            NextRoundButton.Style.NormalTexture = EdgeGame.GetTexture("ShadedDark25");
            NextRoundButton.Style.MouseOverTexture = EdgeGame.GetTexture("ShadedDark25");
            NextRoundButton.Style.ClickTexture = EdgeGame.GetTexture("FlatDark24");
            NextRoundButton.Style.AllColors = Color.White;
            Components.Add(NextRoundButton);
        }
    }
}
