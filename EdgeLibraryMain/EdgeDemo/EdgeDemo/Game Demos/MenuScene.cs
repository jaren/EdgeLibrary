using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using EdgeLibrary;

namespace EdgeDemo
{
    public class SwitchEventArgs : EventArgs
    {
        public int value;

        //An enum could be used here, but it doesn't really matter.......
        //0 - SpaceGame
    }

    public class MenuScene : Scene
    {
        public event EventHandler<SwitchEventArgs> OnSwitch = delegate { };

        public MenuScene() : base("MenuScene")
        {
            AddAndSwitch();
        }
    }
}
