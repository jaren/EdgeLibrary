using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EdgeLibrary
{
    public static class RandomTools
    {
        private static Random random = new Random();
        public static bool SeedAfterGeneration = true;

        public static int RandomInt(int min, int max)
        {
            if (SeedAfterGeneration)
            {
                random = new Random(random.Next());
            }
            return random.Next(Math.Min(min, max), Math.Max(min, max));
        }
        public static int RandomInt(int max)
        {
            if (SeedAfterGeneration)
            {
                random = new Random(random.Next());
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
                random = new Random(random.Next());
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
            if (SeedAfterGeneration)
            {
                random = new Random(random.Next());
            }
            return RandomFloat(0, 1);
        }
    }
}
