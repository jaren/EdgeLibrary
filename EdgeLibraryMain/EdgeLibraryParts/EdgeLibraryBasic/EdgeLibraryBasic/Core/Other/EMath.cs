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

namespace EdgeLibrary.Basic
{
    public static class EMath
    {
        public static Color ColorFromString(string colorString)
        {
            var typeProperty = typeof(Color).GetProperty(colorString);
            if (typeProperty != null)
            {
                return (Color)typeProperty.GetValue(null, null);
            }
            else
            {
                return Color.Black;
            }
        }

        public static float DistanceBetween(Vector2 point, Vector2 point2)
        {
            float distX = point2.X - point.X;
            float distY = point.Y - point2.Y;
            return (float)Math.Sqrt(distX*distX + distY*distY);
        }

        public static double RadiansToDegrees(float radians) { return radians * (180 / Math.PI); }
        public static double DegreesToRadians(float degrees) { return degrees / (180 / Math.PI); }

    }
}
