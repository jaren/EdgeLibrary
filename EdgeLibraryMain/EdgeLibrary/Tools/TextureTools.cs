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
	        //Incomplete
            /*
	        public static Texture2D CreateGradient(int width, int height, Color color1, Color color2, Vector2 colorEmitter1, Vector2 colorEmitter2)
	        {
	            Texture2D Texture = new Texture2D(EdgeGame.graphicsDevice, width, height);
	            Color[] colorData = new Color[width * height];
	
	            Line emitterLine = new Line(colorEmitter1, colorEmitter2);
	            Line compareLine = Line.PerpendicularToAt(emitterLine, MathTools.MidPoint(colorEmitter1, colorEmitter2));
	            Line colorEmitter1Line = Line.PerpendicularToAt(emitterLine, colorEmitter1);
	            Line colorEmitter2Line = Line.PerpendicularToAt(emitterLine, colorEmitter2);
	
	            foreach (Vector2 point in emitterLine.GetPointsWithinRectangle(new Rectangle(0, 0, width, height)))
	            {
	                Line line = Line.PerpendicularToAt(emitterLine, point);
	                foreach (Vector2 linePoint in line.GetPointsWithinRectangle(new Rectangle(0, 0, width, height)))
	                {
	                    //If the line is past the "halfway" mark  may have problems with the lines that are far on the opposite side of the colorEmitter1Line
	                    if (line.DistanceTo((Vector2)emitterLine.Intersection(colorEmitter1Line)) > compareLine.DistanceTo((Vector2)emitterLine.Intersection(colorEmitter1Line)))
	                    {
	                        colorData[(int)linePoint.X + (int)linePoint.Y * width] = Color.Lerp(color1, color2, ((float)line.DistanceTo((Vector2)emitterLine.Intersection(colorEmitter1Line))) / ((float)compareLine.DistanceTo((Vector2)emitterLine.Intersection(colorEmitter1Line))));
	                    }
	                    else
	                    {
	                        colorData[(int)linePoint.X + (int)linePoint.Y * width] = Color.Lerp(color2, color1, ((float)line.DistanceTo((Vector2)emitterLine.Intersection(colorEmitter2Line))) / ((float)compareLine.DistanceTo((Vector2)emitterLine.Intersection(colorEmitter2Line))));
	                    }
	                }
	            }
	
	            Texture.SetData<Color>(colorData);
	            return Texture;
	        }*/
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
	            EdgeGame.drawTexture(Pixel, new Rectangle((int)position.X, (int)position.Y, 1, 1), null, color, 0f, Vector2.Zero, SpriteEffects.None, 0);
	        }
	        public static void DrawRectangleAt(Vector2 position, float width, float height, Color color)
	        {
	            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
	            EdgeGame.drawTexture(Pixel, rectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None, 0);
	        }
	        #endregion
	    }
	}
