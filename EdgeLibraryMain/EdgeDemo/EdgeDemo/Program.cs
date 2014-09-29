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

/*
 *             Fire = new ParticleEmitter("Fire", new Vector2(500))
            {
                BlendState = BlendState.Additive,
                Life = 2000,

                EmitPositionVariance = new Vector2(0, 0),

                MinVelocity = new Vector2(2, 2),
                MaxVelocity = new Vector2(-3, -3),

                MinScale = new Vector2(2.5f),
                MaxScale = new Vector2(3f),

                MinColorIndex = new ColorChangeIndex(400, Color.Magenta, Color.Orange, Color.Purple, Color.Transparent),
                MaxColorIndex = new ColorChangeIndex(400, Color.Teal, Color.OrangeRed, Color.DarkOrange, Color.Transparent),
                EmitWait = 0,
                ParticlesToEmit = 10,
            };
            Fire.AddToGame();
*/