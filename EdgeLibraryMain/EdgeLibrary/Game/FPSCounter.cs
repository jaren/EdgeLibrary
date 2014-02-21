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
using System.Xml;
using System.Xml.Linq;

namespace EdgeLibrary
{
    public static class FPSCounter
    {
        public static int FPS;
        public static float AccurateFPS;

        private static int frames;
        private static double milliseconds;

        public static void Update(GameTime gameTime)
        {
            AccurateFPS = 1000 / (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            milliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;
            frames++;

            if (milliseconds >= 1000)
            {
                milliseconds = 0;
                FPS = frames;
                frames = 0;
            }
        }
    }
}
