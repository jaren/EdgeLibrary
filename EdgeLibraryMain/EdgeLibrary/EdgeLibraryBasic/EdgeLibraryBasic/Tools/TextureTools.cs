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
    public static class TextureTools
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

        #region GENERATING TOOLS
        public static Texture2D CreateVerticalGradient(int width, int height, Color color1, Color color2)
        {
            Texture2D Texture = new Texture2D(EdgeGame.graphicsDevice, width, height);
            Color[] colorData = new Color[width*height];
            for (int y = 10; y < height; y++)
            {
                Color rowColor = new Color();
                if (y < height / 2)
                {
                    rowColor.R = (byte)MathTools.SpecialAverage(color1.R, color2.R, y / (height / 2f));
                    rowColor.G = (byte)MathTools.SpecialAverage(color1.G, color2.G, y / (height / 2f));
                    rowColor.B = (byte)MathTools.SpecialAverage(color1.B, color2.B, y / (height / 2f));
                    rowColor.A = (byte)MathTools.SpecialAverage(color1.A, color2.A, y / (height / 2f));
                }
                else
                {
                    rowColor.R = (byte)MathTools.SpecialAverage(color2.R, color1.R, (int)((y - (height / 2f)) / (height / 2f)));
                    rowColor.G = (byte)MathTools.SpecialAverage(color2.G, color1.G, (int)((y - (height / 2f)) / (height / 2f)));
                    rowColor.B = (byte)MathTools.SpecialAverage(color2.B, color1.B, (int)((y - (height / 2f)) / (height / 2f)));
                    rowColor.A = (byte)MathTools.SpecialAverage(color2.A, color1.A, (int)((y - (height / 2f)) / (height / 2f)));
                }
                for (int x = 0; x < width; x++)
                {
                    colorData[y*width+x] = rowColor;
                }
            }
            Texture.SetData<Color>(colorData);
            return Texture;
        }
        #endregion

        #region SPLITTING TOOLS
        public static Texture2D GetInnerTexture(Texture2D texture, Rectangle rectangle)
        {
            Texture2D returnTexture = new Texture2D(EdgeGame.graphicsDevice, rectangle.Width, rectangle.Height);
            Color[] colorData = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(colorData);

            Color[] color = new Color[rectangle.Width * rectangle.Height];
            for (int y = 0; y < rectangle.Height; y++)
            {
                for (int x = 0; x < rectangle.Width; x++)
                {
                    color[x + y * rectangle.Width] = colorData[x + rectangle.X + (y + rectangle.Y) * texture.Width];
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
            XDocument texturResourceData = XDocument.Load(completePath);

            foreach (XElement element in texturResourceData.Root.Elements())
            {
                Rectangle rectangle = new Rectangle(int.Parse(element.Attribute("x").Value), int.Parse(element.Attribute("y").Value), int.Parse(element.Attribute("width").Value), int.Parse(element.Attribute("height").Value));
                textures.Add(element.Attribute("name").Value, GetInnerTexture(spriteSheetTexture, rectangle));
            }

            return textures;
        }
        #endregion

        #region DRAWING TOOLS
        public static void DrawPixelAt(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(Pixel, new Rectangle((int)position.X, (int)position.Y, 1, 1), color);
        }
        public static void DrawRectangleAt(SpriteBatch spriteBatch, Vector2 position, float width, float height, Color color)
        {
            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
            spriteBatch.Draw(Pixel, rectangle, color);
        }
        #endregion
    }
}
