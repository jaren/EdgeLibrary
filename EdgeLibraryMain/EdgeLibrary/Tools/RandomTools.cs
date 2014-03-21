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

        public static void Init()
        {
            random = new Random();
        }

        public static int RandomInt(int min, int max)
        {
            random = new Random(random.Next());
            return random.Next(Math.Min(min, max), Math.Max(min, max));
        }
        public static int RandomInt(int max)
        {
            random = new Random(random.Next());
            return random.Next(max);
        }
        public static int RandomInt()
        {
            random = new Random(random.Next());
            return random.Next();
        }
        public static float RandomFloat(float min, float max)
        {
            random = new Random(random.Next());
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
            random = new Random(random.Next());
            return RandomFloat(0, 1);
        }
    }
}
