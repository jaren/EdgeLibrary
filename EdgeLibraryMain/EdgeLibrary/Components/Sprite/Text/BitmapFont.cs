using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace EdgeLibrary
{
    /// <summary>
    /// This class is used for BitmapFonts for use in BitmapTextSprites
    /// This is built to use the XML type exported by FontBuilder
    /// </summary>
    public class BitmapFont
    {
        public Texture2D FontTexture;
        public Dictionary<char, Rectangle> Characters;

        public BitmapFont(string xmlPath, Texture2D imageTexture)
        {
            FontTexture = imageTexture.Clone();

            XDocument document = XDocument.Load(xmlPath);
            string[] rectangle;

            foreach(XElement element in document.Root.Elements())
            {
                rectangle = ((string)element.Attribute("rect")).Split(' ');
                Characters.Add(((string)element.Attribute("code"))[0], new Rectangle(Convert.ToInt32(rectangle[0]), Convert.ToInt32(rectangle[1]), Convert.ToInt32(rectangle[2]), Convert.ToInt32(rectangle[3])));
            }
        }

        public Vector2 MeasureString(string text, float characterSpacing)
        {
            Vector2 size = Vector2.Zero;

            foreach(Rectangle rectangle in Characters.Values)
            {
                size = new Vector2(size.X + rectangle.Width + characterSpacing, size.Y < rectangle.Height ? rectangle.Height : size.Y);
            }
            size = new Vector2(size.X - characterSpacing, size.Y);

            return size;
        }

        public void DrawString(string text, Vector2 position, Color color, float rotation, Vector2 scale, SpriteEffects spriteEffects, int layerDepth, float characterSpacing, SpriteBatch spritebatch)
        {
            Vector2 currentOffset = Vector2.Zero;

            foreach(char character in text.ToCharArray())
            {
                spritebatch.Draw(FontTexture, new Rectangle((int)position.X, (int)position.Y, (int)(Characters[character].Width*scale.X), (int)(Characters[character].Height*scale.Y)), Characters[character], color, rotation, Vector2.Zero, spriteEffects, layerDepth);
                currentOffset = new Vector2(currentOffset.X + Characters[character].Width + characterSpacing, currentOffset.Y);
            }
        }
    }
}
