using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    public class GameMenu : MenuBase
    {
        BoardManager manager;

        public GameMenu() : base("GameMenu")
        {
            Input.OnKeyRelease += Input_OnKeyRelease;
        }

        public override void SwitchTo()
        {
            EdgeGame.ClearColor = Color.Gray;
            if (BoardManager.ResetGame)
            {
                Components.Remove(manager);
                manager = new BoardManager();
            }
            Components.Add(manager);

            base.SwitchTo();
        }

        void Input_OnKeyRelease(Microsoft.Xna.Framework.Input.Keys key)
        {
            if (MenuManager.SelectedMenu == this && key == Config.BackKey)
            {
                MenuManager.SwitchMenu("OptionsMenu");
            }
        }
    }
}
