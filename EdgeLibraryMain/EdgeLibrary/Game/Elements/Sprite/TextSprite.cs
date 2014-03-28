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

        //The width/height of the measured text
        public override float Width { get { return Font == null ? 0 : Font.MeasureString(Text).X; } }
        public override float Height { get { return Font == null ? 0 : Font.MeasureString(Text).Y; } }

        //The text to display on the screen
        public string Text { get { return _text; } set { _text = value; reloadOriginPoint(); } }
        protected string _text;

        public TextSprite(string fontName, string text, Vector2 position) : base("", position)
        {
            _text = text;

            //Finds the font from the current game's resources
            if (fontName != null)
            {
                Font = EdgeGame.GetCurrentGame().Resources.GetFont(fontName);
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
                if (_centerAsOrigin)
                {
                    Vector2 Measured = Font.MeasureString(_text);
                    OriginPoint = new Vector2(Measured.X / 2f, Measured.Y / 2f);
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
            spriteBatch.DrawString(_font, _text, Position, Color, Rotation, OriginPoint, Scale, SpriteEffects, 0);
        }
    }
}
