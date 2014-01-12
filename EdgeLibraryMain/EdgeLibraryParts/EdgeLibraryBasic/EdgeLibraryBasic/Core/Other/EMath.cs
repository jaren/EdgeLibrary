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
    //Holds all the calculation and simple drawing functions
    public static class EMath
    {
        public static Texture2D Pixel;
        public static Texture2D Blank;
        public static string ContentRootDirectory;

        public static void Init(GraphicsDevice graphicsDevice)
        {
            Pixel = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Pixel.SetData(new Color[1]{Color.White});
            Blank = new Texture2D(graphicsDevice, 1, 1);
            Blank.SetData(new Color[1]{new Color(0, 0, 0, 0)});
        }

        public static float circlePointStep = 8;
        public static float outerCirclePointStep = 1;

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
            float actualStep = circlePointStep / radius;

            for (float currentRadius = radius; currentRadius > 0; currentRadius -= actualStep)
            {
                for (float x = centerPosition.X - currentRadius; x <= centerPosition.X + currentRadius; x += actualStep)
                {
                    /* Solve for y based on: x^2 + y^2 = r^2 at center 0, 0
                                             (x-centerX)^2 + (y-centerY)^2 = r^2
                                             y = SqRt(r^2 - (x-centerX)^2) + centerY  */

                    //First point's y coordinate - bottom half
                    float y = (float)(Math.Sqrt(Math.Pow(currentRadius, 2) - Math.Pow(x - centerPosition.X, 2)) + centerPosition.Y);

                    //Second point's y coordinate - top half
                    float y1 = -(y - centerPosition.Y) + centerPosition.Y;

                    points.Add(new Vector2(x, y));
                    points.Add(new Vector2(x, y1));
                }
            }

            return points;
        }

        public static List<Vector2> GetOuterCirclePoints(Vector2 centerPosition, float radius)
        {
            List<Vector2> points = new List<Vector2>();
            float actualStep = outerCirclePointStep / radius;

            for (float x = centerPosition.X - radius; x <= centerPosition.X + radius; x += actualStep)
            {
                /* Solve for y based on: x^2 + y^2 = r^2 at center 0, 0
                                         (x-centerX)^2 + (y-centerY)^2 = r^2
                                         y = SqRt(r^2 - (x-centerX)^2) + centerY  */

                //First point's y coordinate - bottom half
                float y = (float)(Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(x - centerPosition.X, 2)) + centerPosition.Y);

                //Second point's y coordinate - top half
                float y1 = -(y - centerPosition.Y) + centerPosition.Y;

                points.Add(new Vector2(x, y));
                points.Add(new Vector2(x, y1));
            }

            return points;
        }

        public static void DrawPixelAt(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(Pixel, new Rectangle((int)position.X, (int)position.Y, 1, 1), color);
        }
        public static void DrawRectangleAt(SpriteBatch spriteBatch, Vector2 position, float width, float height, Color color)
        {
            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
            spriteBatch.Draw(Pixel, rectangle, color);
        }

        public static double RadiansToDegrees(float radians) { return radians * (180 / Math.PI); }
        public static double DegreesToRadians(float degrees) { return degrees / (180 / Math.PI); }

    }
}
