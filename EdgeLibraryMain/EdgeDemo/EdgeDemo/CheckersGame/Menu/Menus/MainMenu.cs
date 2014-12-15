using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;

namespace EdgeDemo.CheckersGame
{
    public class MainMenu : MenuBase
    {
        public MainMenu() : base("MainMenu")
        {
            BoardManager manager = new BoardManager() { Visible = false };
            Components.Add(manager);

            Button button = new Button("FlatDark01", new Microsoft.Xna.Framework.Vector2(500));
            button.OnClick += button_OnClick;
            Components.Add(button);

  
        }

        void button_OnClick(Button sender, Microsoft.Xna.Framework.GameTime gameTime)
        {
            MenuManager.SwitchMenu("GameMenu");
        }
    }
}
