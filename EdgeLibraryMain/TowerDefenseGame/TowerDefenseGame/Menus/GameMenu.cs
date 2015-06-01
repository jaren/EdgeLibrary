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
        public RoundManager Manager;

        public int Lives;
        public int Money;

        public TextSprite RoundSmall;
        public TextSprite RoundLarge;
        public TextSprite LivesSmall;
        public TextSprite LivesLarge;
        public TextSprite MoneySmall;
        public TextSprite MoneyLarge;

        public GameMenu() : base("GameMenu")
        {
            Input.OnKeyRelease += Input_OnKeyRelease;
            ShouldReset = false;
        }

        public void Reset()
        {
            Manager = new RoundManager(Config.RoundList);
        }

        public override void SwitchTo()
        {
            if (ShouldReset)
            {
                Reset();
                ShouldReset = false;
            }

            Vector2 CommonRatio = new Vector2(0.9f);

            CurrentLevel.Position = new Vector2(EdgeGame.WindowSize.X * 0.5f * CommonRatio.X, EdgeGame.WindowSize.Y * 0.5f * CommonRatio.Y);
            CurrentLevel.ResizeLevel(EdgeGame.WindowSize * CommonRatio);

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
            CurrentLevel.Update(gameTime);

            base.UpdateObject(gameTime);
        }

        public override void DrawObject(GameTime gameTime)
        {
            CurrentLevel.Draw(gameTime);

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
