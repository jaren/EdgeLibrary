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
        public string Text { get { return _text; } set { _text = value; reloadDimensions(); } }
        public bool CenterText { get; set; }
        protected string _text;

        public TextSprite(string eFontName, string eText, Vector2 ePosition, Color eColor) : this(MathTools.RandomID(), eFontName, eText, ePosition, eColor) { }

        public TextSprite(string id, string eFontName, string eText, Vector2 ePosition, Color eColor) : base(id, "", ePosition)
        {
            CenterText = true;

            Font = ResourceManager.getFont(eFontName);

            Style.Effects = SpriteEffects.None;
            Position = ePosition;

            _text = eText;

            Style.Color = eColor;
            Style.Rotation = 0;
            Scale = Vector2.One;

            if (eFontName != null)
            {
                Font = ResourceManager.getFont(eFontName);
            }

            reloadDimensions();
        }

        public TextSprite(string id, string eFontName, string eText, Vector2 ePosition, Color eColor, float eRotation, Vector2 eScale): this(id, eFontName, eText, ePosition, eColor)
        {
            Style.Rotation = eRotation;
            Scale = eScale;
        }

        public override void reloadDimensions()
        {
            if (Font != null)
            {
                Vector2 Measured = Font.MeasureString(_text);
                _width = Measured.X;
                _height = Measured.Y;

                reloadOriginPoint();
            }
        }

        protected override void reloadOriginPoint()
        {
            if (Font != null)
            {
                Vector2 Measured = Font.MeasureString(_text);
                originPoint = new Vector2(Measured.X / 2, Measured.Y / 2);
            }
        }

        protected override void reloadActualScale()
        {
            if (Font != null)
            {
                Vector2 measured = Font.MeasureString(Text);
                actualScale = new Vector2(_width / measured.X, _height / measured.Y);
                actualScale *= Scale;
            }
        }


        protected override void UpdateCollision(GameTime gameTime)
        {
            if (EdgeGame.CollisionsInTextSprites)
            {
                base.UpdateCollision(gameTime);
            }
        }

        /*
        protected Vector2 MeasuredPosition()
        {
            if (Font != null)
            {
                if (!CenterText)
                {
                    return new Vector2(Position.X + originPoint.X, Position.Y - Font.MeasureString(_text).Y / 2 + originPoint.Y); 
                }
                    return new Vector2(Position.X - Font.MeasureString(_text).X / 2 + originPoint.X, Position.Y - Font.MeasureString(_text).Y / 2 + originPoint.Y);
            }
            return Position;
        }
         */

        protected override void drawElement(GameTime gameTime)
        {
            EdgeGame.drawString(Font, Text, Position, Style.Color, Style.Rotation, originPoint, Scale, Style.Effects);
        }
    }
}
