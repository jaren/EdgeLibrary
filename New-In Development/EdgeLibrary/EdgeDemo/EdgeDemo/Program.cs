using System;
using EdgeLibrary;

namespace EdgeDemo
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// This is a demonstration of how to use EdgeGame with loading content and initializing
        /// </summary>
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

