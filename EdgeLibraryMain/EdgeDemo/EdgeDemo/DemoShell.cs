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
#if WINDOWS || XBOX
    //A basic EdgeGame setup
    public static class DemoShell
    {
        public static EdgeGame Game;

        public static void Main(string[] args)
        {
            Game = new EdgeGame();
            Game.OnInit += new EdgeGame.EdgeGameEvent(Game_OnInit);
            Game.OnLoadContent += new EdgeGame.EdgeGameEvent(Game_OnLoadContent);
            Game.Run();
        }

        private static void Game_OnInit(EdgeGame game)
        {

        }

        private static void Game_OnLoadContent(EdgeGame game)
        {
        }
    }
#endif
}
