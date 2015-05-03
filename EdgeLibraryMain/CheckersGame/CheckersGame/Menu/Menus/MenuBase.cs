using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;


namespace CheckersGame
{
    public class MenuBase : Scene
    {
        public string Name;

        public MenuBase(string name)
        {
            Name = name;
        }

        public virtual void SwitchTo()
        {

        }

        public virtual void SwitchOut()
        {

        }
    }
}
