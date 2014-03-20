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
    public static class RandomTools
    {
        private static Random random;

        public static bool SeedAfterGeneration;

        public static void Init()
        {
            random = new Random();
            SeedAfterGeneration = true;
        }

        public static int RandomInt(int min, int max)
        {
            if (SeedAfterGeneration)
            {
                random = new Random(random.Next(min, max));
            }
            return random.Next(Math.Min(min, max), Math.Max(min, max));
        }
        public static int RandomInt(int max)
        {
            if (SeedAfterGeneration)
            {
                random = new Random(random.Next(max));
            }
            return random.Next(max);
        }
        public static int RandomInt()
        {
            if (SeedAfterGeneration)
            {
                random = new Random(random.Next());
            }
            return random.Next();
        }
        public static float RandomFloat(float min, float max)
        {
            if (SeedAfterGeneration)
            {
                random = new Random(random.Next((int)min, (int)max));
            }

            if (min + 1 > max)
            {
                return (float)MathHelper.Lerp(min, max, (float)random.NextDouble());
            }
            else
            {
                return RandomInt((int)min, (int)max - 1) + (float)random.NextDouble();
            }
        }
        public static float RandomFloat()
        {
            return RandomFloat(0, 1);
        }
    }
}
