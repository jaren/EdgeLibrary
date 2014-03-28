using System;
using EdgeLibrary;

namespace EdgeDemo
{
#if WINDOWS || XBOX

    /// <summary>
    /// TODO:
    /// -Resolve all "TOINCLUDE"
    /// </summary>

    static class Program
    {
        static void Main(string[] args)
        {
            EdgeGame game = new EdgeGame();
            game.OnInit += new EdgeGame.EdgeGameEvent(game_OnInit);
            game.OnLoadContent += new EdgeGame.EdgeGameEvent(game_OnLoadContent);
            game.Run();
        }

        static void game_OnInit(EdgeGame game)
        {
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
        }

    }
#endif
}

