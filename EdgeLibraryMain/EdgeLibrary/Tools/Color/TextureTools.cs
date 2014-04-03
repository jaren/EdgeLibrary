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
using System.Xml.Linq;

namespace EdgeLibrary
{
    //Provides many tools for editing textures and colors
        public static class TextureTools
        {
            #region GENERATING TOOLS
            //Makes a copy of a Texture2D
            public static Texture2D Copy(Texture2D texture)
            {
                Color[] colors = new Color[texture.Width * texture.Height];
                texture.GetData<Color>(colors);
                Texture2D returnTexture = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);
                returnTexture.SetData<Color>(colors);
                return returnTexture;
            }
            //Colorizes a Texture using a color and a float of the amount to apply that color
            public static void Colorize(Texture2D texture, Color color, float factor)
            {
                Color[] colorData = new Color[texture.Width * texture.Height];
                texture.GetData<Color>(colorData);

                for (int i = 0; i < colorData.Length; i++)
                {
                    Color currentColor = colorData[i];
                    byte darkness = (byte)((currentColor.R + currentColor.G + currentColor.B) / 3);
                    colorData[i] = Color.Lerp(currentColor, color, ((float)darkness / 255) * factor * 2);
                }

                texture.SetData<Color>(colorData);
            }
            //The same as Colorize, but doesn't modify the original texture and instead returns a modified copy
            public static Texture2D GetAsColorized(Texture2D texture, Color color, float factor)
            {
                Texture2D returnTexture = Copy(texture);
                Colorize(returnTexture, color, factor);
                return returnTexture;
            }
            //Sets the inner rectangle portion of a Texture2D
            public static void SetInnerTexture(Texture2D texture, Texture2D innerTexture, Vector2 startPosition)
            {
                Color[] colorData = new Color[texture.Width * texture.Height];
                texture.GetData<Color>(colorData);
                Color[] innerColorData = new Color[innerTexture.Width * innerTexture.Height];
                innerTexture.GetData<Color>(innerColorData);

                for (int y = 0; y < innerTexture.Height; y++)
                {
                    for (int x = 0; x < innerTexture.Width; x++)
                    {
                        if (((x + (int)startPosition.X) + (y + (int)startPosition.Y) * texture.Width) < colorData.Length)
                        {
                            colorData[(x + (int)startPosition.X) + (y + (int)startPosition.Y) * texture.Width] = innerColorData[x + y * innerTexture.Width];
                        }
                    }
                }

                texture.SetData<Color>(colorData);
            }
            //The same as SetInnerTexture, but doesn't modify the original texture and instead returns a modified copy
            public static Texture2D GetAsSetInnerTexture(Texture2D texture, Texture2D innerTexture, Vector2 startPosition)
            {
                Texture2D returnTexture = Copy(texture);
                SetInnerTexture(returnTexture, innerTexture, startPosition);
                return returnTexture;
            }
            #endregion

            #region SPLITTING TOOLS
            //Gets an inner rectangle portion of a Texture2D
            public static Texture2D GetInnerTexture(Texture2D texture, Rectangle rectangle)
            {
                Texture2D returnTexture = EdgeGame.CreateNewTexture(rectangle.Width, rectangle.Height);
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

            //Splits a spritesheet using an xml document and a texture
            public static Dictionary<string, Texture2D> SplitSpritesheet(Texture2D spriteSheetTexture, string XMLPath)
            {
                Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
                string completePath = string.Format("{0}\\{1}", Resources.ContentRootDirectory, XMLPath);
                XDocument textureResourceData = XDocument.Load(completePath);

                foreach (XElement element in textureResourceData.Root.Elements())
                {
                    Rectangle rectangle = new Rectangle(int.Parse(element.Attribute("x").Value), int.Parse(element.Attribute("y").Value), int.Parse(element.Attribute("width").Value), int.Parse(element.Attribute("height").Value));
                    textures.Add(element.Attribute("name").Value, GetInnerTexture(spriteSheetTexture, rectangle));
                }

                return textures;
            }
            public static Dictionary<string, Texture2D> SplitSpritesheet(string spriteSheetTexture, string XMLPath)
            {
                return SplitSpritesheet(Resources.TextureFromString(spriteSheetTexture), XMLPath);
            }
            #endregion
        }
}
