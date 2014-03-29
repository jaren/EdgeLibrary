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
    /// <summary>
    /// TODO:
    /// -Make modifiers easier to use - automatically add all subclasses to Sprites as variables (used like Sprite.Movement), if possible
    /// if not, then add all subclasses of Modifier to sprites automatically
    /// -Add a physics engine
    /// -Add a camera
    /// </summary>

    /// <summary>
    /// A demo of EdgeLibrary
    /// </summary>
    #if WINDOWS || XBOX
    static class Program
    {
        static EdgeGame game;

        static void Main(string[] args)
        {
            game = new EdgeGame();
            game.OnInit += new EdgeGame.EdgeGameEvent(game_OnInit);
            game.OnLoadContent += new EdgeGame.EdgeGameEvent(game_OnLoadContent);
            game.Run();
        }

        static void game_OnInit(EdgeGame game)
        {
            game.WindowSize = new Vector2(1000);
            game.ClearColor = MathTools.ColorFromHex("#051047");

            Sprite sprite = new Sprite("enemyShip", new Vector2(400));
            sprite.AddModifier(new MovementModifier());
            ((MovementModifier)sprite.Modifiers["Movement"]).MoveTo(Vector2.Zero, 1);
            sprite.AddToGame();

            DebugText debug = new DebugText("ComicSans-10", Vector2.Zero);
            debug.AddToGame();

            ParticleEmitter emitter = new ParticleEmitter("Pixel", new Vector2(400, 400));
            emitter.AddToGame();
        }

        static void game_OnLoadContent(EdgeGame game)
        {
            Resources.LoadFont("Fonts/Comic Sans/ComicSans-10");
            Resources.LoadFont("Fonts/Comic Sans/ComicSans-20");
            Resources.LoadFont("Fonts/Comic Sans/ComicSans-30");
            Resources.LoadFont("Fonts/Comic Sans/ComicSans-40");
            Resources.LoadFont("Fonts/Comic Sans/ComicSans-50");
            Resources.LoadFont("Fonts/Comic Sans/ComicSans-60");
            Resources.LoadFont("Fonts/Courier New/CourierNew-10");
            Resources.LoadFont("Fonts/Courier New/CourierNew-20");
            Resources.LoadFont("Fonts/Courier New/CourierNew-30");
            Resources.LoadFont("Fonts/Courier New/CourierNew-40");
            Resources.LoadFont("Fonts/Courier New/CourierNew-50");
            Resources.LoadFont("Fonts/Courier New/CourierNew-60");
            Resources.LoadFont("Fonts/Georgia/Georgia-10");
            Resources.LoadFont("Fonts/Georgia/Georgia-20");
            Resources.LoadFont("Fonts/Georgia/Georgia-30");
            Resources.LoadFont("Fonts/Georgia/Georgia-40");
            Resources.LoadFont("Fonts/Georgia/Georgia-50");
            Resources.LoadFont("Fonts/Georgia/Georgia-60");
            Resources.LoadFont("Fonts/Impact/Impact-10");
            Resources.LoadFont("Fonts/Impact/Impact-20");
            Resources.LoadFont("Fonts/Impact/Impact-30");
            Resources.LoadFont("Fonts/Impact/Impact-40");
            Resources.LoadFont("Fonts/Impact/Impact-50");
            Resources.LoadFont("Fonts/Impact/Impact-60");
            Resources.LoadTexture("enemyShip");
        }

    }
    #endif
}

