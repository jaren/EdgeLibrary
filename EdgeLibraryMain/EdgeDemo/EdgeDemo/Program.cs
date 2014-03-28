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
    /// -Resolve all "TOINCLUDE"
    /// -Add a physics engine
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
            game.WindowSize = new Vector2(700);

            Sprite sprite = new Sprite("enemyShip", new Vector2(400));
            sprite.AddToGame();

            TextSprite textSprite = new TextSprite("ComicSans-50", "Apple \n Pear \n Banana \n Orange \n", new Vector2(300, 200));
            textSprite.CenterAsOrigin = false;
            textSprite.AddToGame();
        }

        static void game_OnLoadContent(EdgeGame game)
        {
            game.Resources.LoadFont("Fonts/Comic Sans/ComicSans-10");
            game.Resources.LoadFont("Fonts/Comic Sans/ComicSans-20");
            game.Resources.LoadFont("Fonts/Comic Sans/ComicSans-30");
            game.Resources.LoadFont("Fonts/Comic Sans/ComicSans-40");
            game.Resources.LoadFont("Fonts/Comic Sans/ComicSans-50");
            game.Resources.LoadFont("Fonts/Comic Sans/ComicSans-60");
            game.Resources.LoadFont("Fonts/Courier New/CourierNew-10");
            game.Resources.LoadFont("Fonts/Courier New/CourierNew-20");
            game.Resources.LoadFont("Fonts/Courier New/CourierNew-30");
            game.Resources.LoadFont("Fonts/Courier New/CourierNew-40");
            game.Resources.LoadFont("Fonts/Courier New/CourierNew-50");
            game.Resources.LoadFont("Fonts/Courier New/CourierNew-60");
            game.Resources.LoadFont("Fonts/Georgia/Georgia-10");
            game.Resources.LoadFont("Fonts/Georgia/Georgia-20");
            game.Resources.LoadFont("Fonts/Georgia/Georgia-30");
            game.Resources.LoadFont("Fonts/Georgia/Georgia-40");
            game.Resources.LoadFont("Fonts/Georgia/Georgia-50");
            game.Resources.LoadFont("Fonts/Georgia/Georgia-60");
            game.Resources.LoadFont("Fonts/Impact/Impact-10");
            game.Resources.LoadFont("Fonts/Impact/Impact-20");
            game.Resources.LoadFont("Fonts/Impact/Impact-30");
            game.Resources.LoadFont("Fonts/Impact/Impact-40");
            game.Resources.LoadFont("Fonts/Impact/Impact-50");
            game.Resources.LoadFont("Fonts/Impact/Impact-60");
            game.Resources.LoadTexture("enemyShip");
        }

    }
    #endif
}

