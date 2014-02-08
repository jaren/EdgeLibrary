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
        /* Will not be used
        public static Texture2D CreateVerticalGradient(int width, int height, float midpoint, Color color1, Color color2)
        {
            Texture2D Texture = new Texture2D(EdgeGame.graphicsDevice, width, height);
            Color[] colorData = new Color[width*height];
            for (int y = 0; y < height; y++)
            {
                Color rowColor = new Color();
                if (y < midpoint)
                {
                    rowColor.R = (byte)MathTools.SpecialAverage(color1.R, color2.R, y / (midpoint));
                    rowColor.G = (byte)MathTools.SpecialAverage(color1.G, color2.G, y / (midpoint));
                    rowColor.B = (byte)MathTools.SpecialAverage(color1.B, color2.B, y / (midpoint));
                    rowColor.A = (byte)MathTools.SpecialAverage(color1.A, color2.A, y / (midpoint));
                }
                else
                {
                    rowColor.R = (byte)MathTools.SpecialAverage(color2.R, color1.R, ((height - y) / (midpoint)));
                    rowColor.G = (byte)MathTools.SpecialAverage(color2.G, color1.G, ((height - y) / (midpoint)));
                    rowColor.B = (byte)MathTools.SpecialAverage(color2.B, color1.B, ((height - y) / (midpoint)));
                    rowColor.A = (byte)MathTools.SpecialAverage(color2.A, color1.A, ((height - y) / (midpoint)));
                }
                for (int x = 0; x < width; x++)
                {
                    colorData[y*width+x] = rowColor;
                }
            }
            Texture.SetData<Color>(colorData);
            return Texture;
        }
         */

        //Incomplete
        public static Texture2D CreateGradient(int width, int height, Color color1, Color color2, Vector2 colorEmitter1, Vector2 colorEmitter2)
        {
            Texture2D Texture = new Texture2D(EdgeGame.graphicsDevice, width, height);
            Color[] colorData = new Color[width * height];

            Line emitterLine = new Line(colorEmitter1, colorEmitter2);
            Line compareLine = Line.PerpendicularToAt(MathTools.MidPoint(colorEmitter1, colorEmitter2), emitterLine);

            Line emitter1Line = Line.PerpendicularToAt(colorEmitter1, emitterLine);
            Line emitter2Line = Line.PerpendicularToAt(colorEmitter2, emitterLine);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //Checks if the current pixel is past the "halfway" mark
                    Line passThroughLine = new Line(colorEmitter1, new Vector2(x, y));
                    Vector2 intersection = (Vector2)passThroughLine.Intersection(compareLine);

                    //NOTE: THE POINTS THAT ARE BEHIND THE TWO EMITTERS MAY BE INCLUDED IN THIS
                    //ADD A CHECK TO SEE IF THE POINTS ARE BEHIND THE EMITTERS, THEN SET THEM TO THE EMITTER COLOR
                    if (Vector2.Distance(colorEmitter1, new Vector2(x, y)) > Vector2.Distance(colorEmitter1, intersection))
                    {

                    }

                    colorData[y * width + x] = new Color();
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
        public static void DrawPixelAt(Vector2 position, Color color)
        {
            EdgeGame.DrawTexture(Pixel, new Rectangle((int)position.X, (int)position.Y, 1, 1), null, color, 0f, Vector2.Zero, SpriteEffects.None, 0);
        }
        public static void DrawRectangleAt(Vector2 position, float width, float height, Color color)
        {
            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
            EdgeGame.DrawTexture(Pixel, rectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None, 0);
        }
        #endregion
    }
}
