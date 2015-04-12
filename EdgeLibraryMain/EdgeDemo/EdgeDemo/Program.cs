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

namespace EdgeDemo
{
    /// <summary>
    /// TODO:
    /// -Fix HeightMap
    /// -Add scaling into physics bodies
    /// 
    /// BUGS:
    /// -Fix SpriteBatch needed to be restart for 2D objects combined with 3D
    /// </summary>

    /// <summary>
    /// A demo of EdgeLibrary
    /// </summary>
    #if WINDOWS
    static class Program
    {
        static CheckersGame.Game MainGame;

        static void Main(string[] args)
        {
            MainGame = new CheckersGame.Game();

            EdgeGame.OnInit += new EdgeGame.EdgeGameEvent(MainGame.OnInit);
            EdgeGame.OnLoadContent += new EdgeGame.EdgeGameEvent(MainGame.OnLoadContent);
            EdgeGame.Start();
        }
    }
#endif
}