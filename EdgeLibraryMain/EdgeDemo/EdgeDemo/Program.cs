using System;

namespace EdgeDemo
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (EdgeDemo game = new EdgeDemo())
            {
                game.Run();
            }
        }
    }
#endif
}

