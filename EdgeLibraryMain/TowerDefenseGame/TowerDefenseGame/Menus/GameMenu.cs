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
        public RoundManager Manager;

        public GameMenu() : base("GameMenu")
        {
            Input.OnKeyRelease += Input_OnKeyRelease;
        }

        public override void SwitchTo()
        {
            Manager = new RoundManager(Config.RoundList);

            base.SwitchTo();
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
            if (MenuManager.SelectedMenu == this && key == Config.BackKey)
            {
                MenuManager.SwitchMenu("OptionsMenu");
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
