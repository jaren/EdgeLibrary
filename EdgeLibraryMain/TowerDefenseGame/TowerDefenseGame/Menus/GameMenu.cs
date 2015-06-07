using EdgeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class GameMenu : MenuBase
    {
        public static Level CurrentLevel;
        public static Difficulty Difficulty;
        public static bool ShouldReset;
        public RoundManager RoundManager;

        public int Lives
        {
            get { return lives; }
            set { lives = value; LivesNumber.Text = lives.ToString(); }
        }
        private int lives;
        public int Money
        {
            get { return money; }
            set { money = value; MoneyNumber.Text = money.ToString(); }
        }
        private int money;

        public TextSprite RoundNumber;
        public TextSprite RoundText;
        public TextSprite LivesNumber;
        public TextSprite LivesText;
        public TextSprite MoneyNumber;
        public TextSprite MoneyText;

        public Button NextRoundButton;

        public List<Button> TowerButtons;

        public GameMenu() : base("GameMenu")
        {
            Input.OnKeyRelease += Input_OnKeyRelease;
            ShouldReset = false;
        }

        public override void SwitchTo()
        {
            if (ShouldReset)
            {
                ShouldReset = false;

                Components.Clear();

                RoundManager = new RoundManager(Config.RoundList);

                Vector2 CommonRatio = new Vector2(0.85f);

                CurrentLevel.Position = new Vector2(EdgeGame.WindowSize.X * 0.5f * CommonRatio.X, EdgeGame.WindowSize.Y * 0.5f * CommonRatio.Y);
                CurrentLevel.ResizeLevel(EdgeGame.WindowSize * CommonRatio);
                Components.Add(CurrentLevel);

                RoundText = new TextSprite(Config.MenuSubtitleFont, "ROUND", new Vector2(EdgeGame.WindowSize.X * (CommonRatio.X + (1f - CommonRatio.X) / 2f), EdgeGame.WindowSize.Y * 0.05f));
                Components.Add(RoundText);

                RoundNumber = new TextSprite("Georgia-60", "0", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.09f));
                Components.Add(RoundNumber);

                LivesText = new TextSprite(Config.MenuSubtitleFont, "LIVES", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.2f));
                Components.Add(LivesText);

                LivesNumber = new TextSprite("Georgia-60", Lives.ToString(), new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.24f));
                Components.Add(LivesNumber);

                MoneyText = new TextSprite(Config.MenuSubtitleFont, "MONEY", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.35f));
                Components.Add(MoneyText);

                MoneyNumber = new TextSprite("Georgia-60", Money.ToString(), new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.39f));
                Components.Add(MoneyNumber);

                NextRoundButton = new Button("ShadedDark25", new Vector2(RoundText.Position.X, EdgeGame.WindowSize.Y * 0.6f)) { Color = Color.White, Scale = new Vector2(1f) };
                NextRoundButton.OnRelease += (x, y) => { if (!RoundManager.RoundRunning) { RoundManager.StartRound(); } };
                NextRoundButton.Style.NormalTexture = EdgeGame.GetTexture("ShadedDark25");
                NextRoundButton.Style.MouseOverTexture = EdgeGame.GetTexture("ShadedDark25");
                NextRoundButton.Style.ClickTexture = EdgeGame.GetTexture("FlatDark24");
                NextRoundButton.Style.AllColors = Color.White;
                Components.Add(NextRoundButton);

                TowerButtons = new List<Button>();
                //Add tower buttons here

                //Must be initialized after the text, otherwise they will be null
                Lives = Config.LivesNumber[(int)Difficulty];
                Money = Config.StartingMoneyNumber[(int)Difficulty];
            }

            EdgeGame.ClearColor = Color.Gray;

            base.SwitchTo();
        }

        public override void SwitchOut()
        {
            EdgeGame.ClearColor = Color.Black;

            base.SwitchOut();
        }

        public override void UpdateObject(GameTime gameTime)
        {
            base.UpdateObject(gameTime);
        }

        public override void DrawObject(GameTime gameTime)
        {
            base.DrawObject(gameTime);
        }

        void Input_OnKeyRelease(Keys key)
        {
            if (MenuManager.SelectedMenu == this && key == Config.BackKey && !MenuManager.InputEventHandled)
            {
                MenuManager.SwitchMenu("OptionsMenu");
                MenuManager.InputEventHandled = true;
            }
        }
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
}
