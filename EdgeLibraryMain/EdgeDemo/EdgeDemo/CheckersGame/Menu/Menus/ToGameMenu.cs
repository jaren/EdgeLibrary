using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    public class ToGameMenu : MenuBase
    {
        public Player Player1;
        public Player Player2;

        public ToGameMenu(string name) : base(name) { }
    }
}
