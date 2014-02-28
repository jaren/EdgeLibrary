	﻿using System;
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
                Pixel = EdgeGame.NewTexture(1, 1);
	            Pixel.SetData(new Color[1] { Color.White });
                Blank = EdgeGame.NewTexture(1, 1);
	            Blank.SetData(new Color[1] { Color.Transparent });
	
	            ResourceManager.addTexture("Pixel", Pixel);
                ResourceManager.addTexture("Blank", Blank);
	        }
	
	        #region GENERATING TOOLS
            public static Texture2D Colorize(Texture2D texture, Color color, float factor)
            {
                Texture2D returnTexture = texture;
                Color[] colorData = new Color[texture.Width * texture.Height];
                texture.GetData<Color>(colorData);

                for (int i = 0; i < colorData.Length; i++ )
                {
                    Color currentColor = colorData[i];
                    byte darkness = (byte)((currentColor.R + currentColor.G + currentColor.B)/3);
                    colorData[i] = Color.Lerp(currentColor, color, ((float)darkness/255)*factor*2);
                }

                returnTexture.SetData<Color>(colorData);
                return returnTexture;
            }
            public static Texture2D SetInnerTexture(Texture2D texture, Texture2D innerTexture, Vector2 startPosition)
            {
                Texture2D returnTexture = texture;
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

                returnTexture.SetData<Color>(colorData);
                return returnTexture;
            }
            public static Texture2D CreateCircleTexture(int radius, Color color)
            {
                Texture2D texture = EdgeGame.NewTexture(radius * 2, radius * 2);
                Color[] colors = new Color[radius * radius * 4];

                foreach(Vector2 point in MathTools.GetCirclePoints(new Vector2(radius, radius), radius, radius))
                {
                    colors[(int)(point.X + point.Y * radius * 2)] = color;
                }
                texture.SetData<Color>(colors);
                return texture;
            }

            public static Texture2D CreateVerticalGradient(int width, int height, Color color1, Color color2)
            {
                Texture2D texture = EdgeGame.NewTexture(width, height);
                Color[] colors = new Color[width*height];

                for (int y = 0; y < height; y++ )
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (y < width * height / 2)
                        {
                            colors[x + y*width] = Color.Lerp(color1, color2, y / height / 2);
                        }
                        else
                        {
                            colors[x + y * width] = Color.Lerp(color2, color1, (height/2 - y) / height / 2);
                        }
                    }
                }
                texture.SetData<Color>(colors);
                return texture;
            }
	        #endregion
	
	        #region SPLITTING TOOLS
	        public static Texture2D GetInnerTexture(Texture2D texture, Rectangle rectangle)
	        {
                Texture2D returnTexture = EdgeGame.NewTexture(rectangle.Width, rectangle.Height);
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
	            return SplitSpritesheet(ResourceManager.textureFromString(spriteSheetTexture), XMLPath);
	        }
	
	        public static Dictionary<string, Texture2D> SplitSpritesheet(Texture2D spriteSheetTexture, string XMLPath)
	        {
	            Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
	            string completePath = string.Format("{0}\\{1}", ResourceManager.ContentRootDirectory, XMLPath);
	            XDocument textureResourceData = XDocument.Load(completePath);
	
	            foreach (XElement element in textureResourceData.Root.Elements())
	            {
	                Rectangle rectangle = new Rectangle(int.Parse(element.Attribute("x").Value), int.Parse(element.Attribute("y").Value), int.Parse(element.Attribute("width").Value), int.Parse(element.Attribute("height").Value));
	                textures.Add(element.Attribute("name").Value, GetInnerTexture(spriteSheetTexture, rectangle));
	            }
	
	            return textures;
	        }
	        #endregion
	
	        #region DRAWING TOOLS
	        public static void DrawPixelAt(Vector2 position, Color color)
	        {
	            EdgeGame.drawTexture(Pixel, new Rectangle((int)position.X, (int)position.Y, 1, 1), null, color, 0f, Vector2.Zero, SpriteEffects.None);
	        }
	        public static void DrawRectangleAt(Vector2 position, float width, float height, Color color)
	        {
	            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
	            EdgeGame.drawTexture(Pixel, rectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None);
	        }
	        #endregion
	    }
	}
