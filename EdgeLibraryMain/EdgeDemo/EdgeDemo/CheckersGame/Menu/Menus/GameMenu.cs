using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    public class GameMenu : MenuBase
    {
        public GameMenu() : base("GameMenu")
        {
            BoardManager manager = new BoardManager();
            Components.Add(manager);
        }
    }
}
