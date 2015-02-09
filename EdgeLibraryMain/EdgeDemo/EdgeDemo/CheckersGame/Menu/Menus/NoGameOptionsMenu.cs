using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    //An options menu for when there is no game to return to
    public class NoGameOptionsMenu : OptionsMenu
    {
        public NoGameOptionsMenu() : base()
        {
            Name = "NoGameOptionsMenu";

            base.ReturnButton.Position = base.QuitButton.Position;
            base.ReturnButtonText.Position = base.QuitButtonText.Position;
            base.ReturnButtonText.Text = "Back to Menu";

            base.QuitButton.Position = new Microsoft.Xna.Framework.Vector2(-10000);
            base.QuitButtonText.Position = new Microsoft.Xna.Framework.Vector2(-10000);
        }
    }
}
