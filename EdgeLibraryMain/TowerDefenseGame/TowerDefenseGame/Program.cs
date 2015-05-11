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
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;

namespace TowerDefenseGame
{
#if WINDOWS || XBOX
    static class Program
    {
        static TowerDefenseGame.Game MainGame;

        static void Main(string[] args)
        {
            MainGame = new TowerDefenseGame.Game();

            EdgeGame.OnInit += new EdgeGame.EdgeGameEvent(MainGame.OnInit);
            EdgeGame.OnLoadContent += new EdgeGame.EdgeGameEvent(MainGame.OnLoadContent);

            EdgeGame.Start();
        }
    }
#endif
}

