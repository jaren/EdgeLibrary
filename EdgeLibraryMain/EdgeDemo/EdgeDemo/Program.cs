using System;
using EdgeLibrary;

namespace EdgeDemo
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (EdgeGame game = new EdgeGame())
            {
                game.OnInit += new EdgeGame.EdgeGameEvent(game_OnInit);
                game.OnLoadContent += new EdgeGame.EdgeGameEvent(game_OnLoadContent);
                game.Run();
            }
        }

        static void game_OnLoadContent(EdgeGame game)
        {
        }

        static void game_OnInit(EdgeGame game)
        {
        }
    }
#endif
}

