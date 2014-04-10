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
    //A "label"
    public class TextSprite : Sprite
    {
        //The font to display on the screen
        public SpriteFont Font { get { return _font; } set { _font = value; reloadOriginPoint(); } }
        private SpriteFont _font;

        //The width/height of the measured text - the Height must be generated in a different way because the line breaks cause it to be generated incorrectly
        public override float Width { get { return Font == null ? 0 : Font.MeasureString(Text).X; } }
        public override float Height { get { return Font == null ? 0 : Font.MeasureString(_text.Split("\n".ToArray())[0]).Y; } }

        //The text to display on the screen
        public string Text { get { return _text; } set { _text = value; reloadOriginPoint(); } }
        protected string _text;
        protected string[] textLines;
        protected Vector2[] textLinesOriginPoints;
        protected float yLineDifference;

        public TextSprite(string fontName, string text, Vector2 position) : base("", position)
        {
            _text = text;

            //Finds the font from the current game's resources
            if (fontName != null)
            {
                Font = Resources.GetFont(fontName);
            }
        }

        public TextSprite(string fontName, string text, Vector2 position, Color color, Vector2 scale, float rotation = 0) : this(fontName, text, position)
        {
            Color = color;
            Rotation = rotation;
            Scale = scale;
        }

        //Reloads the origin point based on font and text
        protected override void reloadOriginPoint()
        {
            if (Font != null && _text != null)
            {
                textLines = _text.Split("\n".ToArray());
                yLineDifference = _font.MeasureString(_text.Split("\n".ToArray())[0]).Y;
                textLinesOriginPoints = new Vector2[textLines.Length];
                for (int i = 0; i < textLines.Length; i++)
                {
                    if (_centerAsOrigin)
                    {
                        textLinesOriginPoints[i] = _font.MeasureString(textLines[i]) / 2;
                    }
                    else
                    {
                        textLinesOriginPoints[i] = Vector2.Zero;
                    }
                }

                if (_centerAsOrigin)
                {
                    OriginPoint = _font.MeasureString(_text)/2;
                }
                else
                {
                    OriginPoint = Vector2.Zero;
                }
            }
        }

        //Draws the textsprite to the spritebatch
        protected override void  DrawObject(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < textLines.Length; i++)
            {
                spriteBatch.DrawString(_font, textLines[i], Position + new Vector2(0, yLineDifference * i) - (textLines.Length > 1 ? new Vector2(0, OriginPoint.Y/2): Vector2.Zero),
                Color, MathHelper.ToRadians(Rotation), textLinesOriginPoints[i], Scale, SpriteEffects, 0);
            }
        }
    }
}
