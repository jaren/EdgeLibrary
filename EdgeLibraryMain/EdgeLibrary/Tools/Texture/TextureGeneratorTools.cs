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

namespace EdgeLibrary
{
    //Generates multiple types of textures, such as gradients, circles, etc.
    public static class TextureGeneratorTools
    {
        public static float circlePointStep = 8;
        public static float outerCirclePointStep = 1;

        public static Dictionary<string, Texture2D> SplitSpritesheet(string spriteSheetTexture, string XMLPath)
        {
            return EdgeGame.TextureFromString(spriteSheetTexture).SplitSpritesheet(XMLPath);
        }

        public static Texture2D GenerateGradient(Color color1, Color color2, int width, int height, bool vertical)
        {
            Texture2D texture = new Texture2D(EdgeGame.GraphicsDevice, width, height);
            Color[] colors = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (vertical)
                    {
                        colors[x + y * width] = Color.Lerp(color1, color2, y / (float)height);
                    }
                    else
                    {
                        colors[x + y * width] = Color.Lerp(color1, color2, x / (float)width);
                    }
                }
            }
            texture.SetData<Color>(colors);
            return texture;
        }

        public static Texture2D GenerateCircle(int radius, Color color)
        {
            Texture2D texture = new Texture2D(EdgeGame.GraphicsDevice, radius * 2, radius * 2);
            Color[] colors = new Color[radius*radius*4];
            foreach(Vector2 point in GetCirclePoints(new Vector2(radius, radius), radius))
            {
                colors[(int)point.X + (int)point.Y * radius * 2] = color;
            }
            texture.SetData<Color>(colors);
            return texture;
        }


        //Returns all the points of the given circle
        public static List<Vector2> GetCirclePoints(Vector2 centerPosition, float radius, float step)
        {
            List<Vector2> points = new List<Vector2>();
            float actualStep = step / radius;

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
        public static List<Vector2> GetCirclePoints(Vector2 centerPosition, float radius)
        {
            return GetCirclePoints(centerPosition, radius, circlePointStep);
        }

        //Returns all the points on the outside of the given circle
        public static List<Vector2> GetOuterCirclePoints(Vector2 centerPosition, float radius, float step)
        {
            List<Vector2> points = new List<Vector2>();
            float actualStep = step / radius;

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
        public static List<Vector2> GetOuterCirclePoints(Vector2 centerPosition, float radius)
        {
            return GetOuterCirclePoints(centerPosition, radius, outerCirclePointStep);
        }
    }
}
