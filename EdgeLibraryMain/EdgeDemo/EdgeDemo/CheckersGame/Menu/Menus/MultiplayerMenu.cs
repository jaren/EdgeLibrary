using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;

namespace EdgeDemo.CheckersGame
{
    public class MultiplayerMenu : MenuBase
    {
        public MultiplayerMenu() : base("MultiplayerMenu")
        {
            Input.OnKeyRelease += Input_OnKeyRelease;
        }

        void Input_OnKeyRelease(Microsoft.Xna.Framework.Input.Keys key)
        {
            if (key == Config.BackKey)
            {
                MenuManager.SwitchMenu(MenuManager.PreviousMenu.Name);
            }
        }
    }
}
