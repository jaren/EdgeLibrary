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
    /// <summary>
    /// A sprite which draws text instead of a texture and uses a Bitmap font
    /// </summary>
    public class BitmapTextSprite : Sprite
    {
        //The font to display on the screen
        public BitmapFont Font { get { return font; } set { font = value; reloadOriginPoint(); reloadBoundingBox(); } }
        private BitmapFont font;

        //Sets the font through a string
        public string FontName { set { Font = EdgeGame.GetBitmapFont(value); } }

        //The width/height of the measured text - the Height must be generated in a different way because the line breaks cause it to be generated incorrectly
        public override float Width { get { return Font == null ? 0 : Font.MeasureString(Text).X; } }
        public override float Height { get { return Font == null ? 0 : Font.MeasureString(text.Split("\n".ToArray())[0]).Y; } }

        //The text to display on the screen
        public string Text { get { return text; } set { text = value; reloadOriginPoint(); reloadBoundingBox(); } }
        protected string text;
        protected string[] textLines;
        protected Vector2[] textLinesOriginPoints;
        protected float yLineDifference;

        public BitmapTextSprite(string fontName, string text, Vector2 position)
            : base("", position)
        {
            this.text = text;

            //Finds the font from the current game's resources
            if (fontName != null)
            {
                Font = EdgeGame.GetBitmapFont(fontName);
                reloadOriginPoint();
            }
        }

        public BitmapTextSprite(string fontName, string text, Vector2 position, Color color, Vector2 scale, float rotation = 0)
            : this(fontName, text, position)
        {
            Color = color;
            Rotation = rotation;
            Scale = scale;
        }

        //Reloads the origin point based on font and text
        protected override void reloadOriginPoint()
        {
            if (Font != null && text != null)
            {
                textLines = text.Split("\n".ToArray());
                yLineDifference = font.MeasureString(text.Split("\n".ToArray())[0]).Y;
                textLinesOriginPoints = new Vector2[textLines.Length];
                for (int i = 0; i < textLines.Length; i++)
                {
                    if (CenterAsOrigin)
                    {
                        textLinesOriginPoints[i] = font.MeasureString(textLines[i]) / 2;
                    }
                    else
                    {
                        textLinesOriginPoints[i] = Vector2.Zero;
                    }
                }

                if (CenterAsOrigin)
                {
                    OriginPoint = font.MeasureString(text) / 2;
                }
                else
                {
                    OriginPoint = Vector2.Zero;
                }
            }
        }

        //Draws the textsprite to the spritebatch
        public override void Draw(GameTime gameTime)
        {
            RestartSpriteBatch();

            for (int i = 0; i < textLines.Length; i++)
            {
                Font.DrawString(textLines[i], Position + new Vector2(0, yLineDifference * i) - (textLines.Length > 1 ? new Vector2(0, OriginPoint.Y / 2) : Vector2.Zero), Color, Rotation, Scale, SpriteEffects, 0, EdgeGame.Game.SpriteBatch);
            }

            RestartSpriteBatch();
        }

        public override object Clone()
        {
            BitmapTextSprite clone = (BitmapTextSprite)base.Clone();
            clone.Font = font;
            return clone;
        }
    }
}
