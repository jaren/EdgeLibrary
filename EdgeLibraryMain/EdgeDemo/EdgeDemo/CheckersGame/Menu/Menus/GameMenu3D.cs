using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    public class GameMenu3D : MenuBase
    {
        BoardManager3D manager;

        public GameMenu3D() : base("GameMenu")
        {
            Input.OnKeyRelease += (x) =>
            {
                if (MenuManager.SelectedMenu == this && x == Config.BackKey)
                {
                    MenuManager.SwitchMenu("OptionsMenu");
                }
            };
        }

        public override void SwitchTo()
        {
            EdgeGame.ClearColor = Color.Gray;
            if (manager.ResetGame)
            {
                if (Components.Contains(manager))
                {
                    Components.Remove(manager);
                }
                manager = new BoardManager3D();
                Components.Add(manager);
            }

            base.SwitchTo();
        }
    }
}
