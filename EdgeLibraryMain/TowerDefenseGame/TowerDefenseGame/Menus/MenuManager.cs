using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public static class MenuManager
    {
        public static List<MenuBase> Menus { get; private set; }
        public static MenuBase SelectedMenu { get; private set; }
        public static MenuBase PreviousMenu { get; private set; }

        public static void Init()
        {
            Menus = new List<MenuBase>();
        }

        public static void Update(GameTime gameTime)
        {
            SelectedMenu.Update(gameTime);
        }

        public static void Draw(GameTime gameTime)
        {
            SelectedMenu.Draw(gameTime);
        }

        public static bool SwitchMenu(string name)
        {
            foreach(MenuBase menu in Menus)
            {
                if (menu.Name == name)
                {
                    PreviousMenu = SelectedMenu;
                    SelectedMenu = menu;
                    if (PreviousMenu != null)
                    {
                        PreviousMenu.SwitchOut();
                    }
                    SelectedMenu.SwitchTo();
                    return true;
                }
            }
            return false;
        }

        public static void AddMenu(MenuBase menu)
        {
            Menus.Add(menu);
        }
    }
}
