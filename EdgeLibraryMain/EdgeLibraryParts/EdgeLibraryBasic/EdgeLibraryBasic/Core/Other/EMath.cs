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
        public static float circlePointStep = 0.05f;

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

        public static List<Vector2> GetCirclePoints(Vector2 centerPosition, float radius)
        {
            List<Vector2> points = new List<Vector2>();

            for (float x = centerPosition.X - radius; x <= centerPosition.X + radius; x += circlePointStep)
            {
                /* Solve for y based on: x^2 + y^2 = r^2 at center 0, 0
                                         (x-centerX)^2 + (y-centerY)^2 = r^2
                                         y = SqRt(r^2 - (x-centerX)^2) + centerY  */

                //First point's y coordinate - bottom half
                float y = (float)(Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(x - centerPosition.X, 2)) + centerPosition.Y);

                //Second point's y coordinate - top half
                float y1 = -(y - centerPosition.Y) + centerPosition.Y;

                points.Add(new Vector2(x, y));
            }

            return points;
        }

        public static double RadiansToDegrees(float radians) { return radians * (180 / Math.PI); }
        public static double DegreesToRadians(float degrees) { return degrees / (180 / Math.PI); }

    }
}
