using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EdgeLibrary
{
    //An alternative to creating a new Random object for every generation
    public static class RandomTools
    {
        private static Random random = new Random();
        public static bool SeedAfterGeneration = true;

        /// <summary>
        /// Gets a random int
        /// </summary>
        public static int RandomInt(int min, int max)
        {
            if (SeedAfterGeneration)
            {
                random = new Random(random.Next());
            }
            return random.Next(Math.Min(min, max), Math.Max(min, max));
        }
        /// <summary>
        /// Gets a random int
        /// </summary>
        public static int RandomInt(int max)
        {
            if (SeedAfterGeneration)
            {
                random = new Random(random.Next());
            }
            return random.Next(max);
        }
        /// <summary>
        /// Gets a random int
        /// </summary>
        public static int RandomInt()
        {
            if (SeedAfterGeneration)
            {
                random = new Random(random.Next());
            }
            return random.Next();
        }
        /// <summary>
        /// Gets a random float
        /// </summary>
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
        /// <summary>
        /// Gets a random float between 0 and 1
        /// </summary>
        public static float RandomFloat()
        {
            if (SeedAfterGeneration)
            {
                random = new Random(random.Next());
            }
            return RandomFloat(0, 1);
        }

        /// <summary>
        /// Generates a color with random red, green, and blue channel values.
        /// </summary>
        public static Color RandomColor()
        {
            return RandomColor(Color.Black, Color.White);
        }
        /// <summary>
        /// Generates a random color bounded by the ARGB values of each color.
        /// </summary>
        /// <remarks>
        /// "min" and "max" really mean nothing in this case, as Math.Min and Math.Max are used in the function.
        /// </remarks>
        public static Color RandomColor(Color min, Color max)
        {
            return new Color(RandomTools.RandomInt(Math.Min(min.R, max.R), Math.Max(min.R, max.R)), RandomTools.RandomInt(Math.Min(min.G, max.G), Math.Max(min.G, max.G)), RandomTools.RandomInt(Math.Min(min.B, max.B), Math.Max(min.B, max.B)), RandomTools.RandomInt(Math.Min(min.A, max.A), Math.Max(min.A, max.A)));
        }
        /// <summary>
        /// Generates a random color which may not be between the ARGB values of each color because it adds the same random number to the R, G, B, and A
        /// </summary>
        public static Color RandomUniformColor(Color min, Color max)
        {
            int random = RandomTools.RandomInt(((max.R - min.R) + (max.G - min.G) + (max.B - min.G)) / 3);
            return new Color(min.R + random, min.G + random, min.B + random, min.A + random);
        }
    }
}
