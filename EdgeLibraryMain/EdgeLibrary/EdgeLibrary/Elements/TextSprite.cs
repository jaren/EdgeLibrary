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
    //Provides a base game object
    public class TextSprite : Sprite
    {
        public SpriteFont Font { get; set; }
        public string Text { get { return _text; } set { _text = value; reloadBoundingBox(); } }
        private string _text;
        public bool CenterText;

        public TextSprite(string eFontName, string eText, Vector2 ePosition, Color eColor) : base("", ePosition)
        {
            Font = ResourceManager.getFont(eFontName);

            CenterText = true;

            spriteEffects = SpriteEffects.None;
            ScaledDrawScale = 1f;
            _position = ePosition;

            _text = eText;

            Color = eColor;
            Rotation = 0;
            Scale = Vector2.One;

            if (eFontName != null)
            {
                Font = ResourceManager.getFont(eFontName);
            }

            reloadBoundingBox();
        }

        public TextSprite(string eFontName, string eText, Vector2 ePosition, Color eColor, float eRotation, Vector2 eScale): this(eFontName, eText, ePosition, eColor)
        {
            Rotation = eRotation;
            Scale = eScale;
        }

        public override void reloadBoundingBox()
        {
            if (Font != null)
            {
                Vector2 Measured = Font.MeasureString(_text);
                BoundingBox = new Rectangle((int)(Position.X - Measured.X / 2), (int)(Position.Y - Measured.Y / 2), (int)Measured.X, (int)Measured.Y);
                _width = BoundingBox.Width;
                _height = BoundingBox.Height;
            }
        }

        public Vector2 MeasuredPosition()
        {
            if (CenterText && Font != null)
            {
                return new Vector2(Position.X - Font.MeasureString(_text).X / 2, Position.Y - Font.MeasureString(_text).Y / 2);
            }
            return Position;
        }

        public override void drawElement(GameTime gameTime)
        {
            EdgeGame.drawString(Font, _text, MeasuredPosition(), Color, Rotation, OriginPoint, Scale, spriteEffects, LayerDepth); 
        }
    }
}
