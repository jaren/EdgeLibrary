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

namespace EdgeLibrary.Basic
{
    //Holds all the calculation and simple drawing functions
    public static class EMath
    {
        public static Texture2D Pixel;
        public static Texture2D Blank;

        public static void Init()
        {
            Pixel = new Texture2D(EdgeGame.graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Pixel.SetData(new Color[1] { Color.White });
            Blank = new Texture2D(EdgeGame.graphicsDevice, 1, 1);
            Blank.SetData(new Color[1] { Color.Transparent });
        }

        public static float circlePointStep = 8;
        public static float outerCirclePointStep = 1;

        /// <summary>
        /// Retrieves a color by its name.
        /// As this method uses a (expensive) reflection call, it should only be invoked at load time.
        /// If the color is known at compile time, a static property on the <see cref="Color"/> class should be used instead.
        /// </summary>
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

        //Used for a string such as: 'Planet/Country/State/City/Street/House' - this would return House
        public static string LastPortionOfPath(string path)
        {
            string[] splitParts = path.Split('/');
            return splitParts[splitParts.Length - 1];
        }

        public static float DistanceBetween(Vector2 point, Vector2 point2)
        {
            float distX = point2.X - point.X;
            float distY = point.Y - point2.Y;
            return (float)Math.Sqrt(distX * distX + distY * distY);
        }

        public static Texture2D GetInnerTexture(Texture2D texture, Rectangle rectangle)
        {
            Texture2D returnTexture = new Texture2D(EdgeGame.graphicsDevice, rectangle.Width, rectangle.Height);
            Color[] colorData = new Color[texture.Width*texture.Height];
            texture.GetData<Color>(colorData);

            Color[] color = new Color[rectangle.Width * rectangle.Height];
            for (int y = 0; y < rectangle.Height; y++)
            {
                for (int x = 0; x < rectangle.Width; x++)
                {
                    color[x + y * rectangle.Width] = colorData[x + rectangle.X + (y + rectangle.Y)*texture.Width];
                }
            }

            returnTexture.SetData<Color>(color);
            return returnTexture;
        }

        public static Dictionary<string, Texture2D> SplitSpritesheet(string spriteSheetTexture, string XMLPath)
        {
            return SplitSpritesheet(EdgeGame.GetTexture(spriteSheetTexture), XMLPath);
        }

        public static Dictionary<string, Texture2D> SplitSpritesheet(Texture2D spriteSheetTexture, string XMLPath)
        {
            Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
            string completePath = string.Format("{0}\\{1}", EdgeGame.ContentRootDirectory, XMLPath);
            XDocument texturEData = XDocument.Load(completePath);

            foreach (XElement element in texturEData.Root.Elements())
            {
                Rectangle rectangle = new Rectangle(int.Parse(element.Attribute("x").Value), int.Parse(element.Attribute("y").Value), int.Parse(element.Attribute("width").Value), int.Parse(element.Attribute("height").Value));
                textures.Add(element.Attribute("name").Value, GetInnerTexture(spriteSheetTexture, rectangle));
            }

            return textures;
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
    }
}
