using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    public class MainMenu : MenuBase
    {
        public MainMenu() : base("MainMenu")
        {
            Button button = new Button("ShadedDark01", new Microsoft.Xna.Framework.Vector2(500));
            button.OnClick += button_OnClick;
            Components.Add(button);
        }

        void button_OnClick(Button sender, Microsoft.Xna.Framework.GameTime gameTime)
        {
            MenuManager.SwitchMenu("GameMenu");
        }
    }
}
