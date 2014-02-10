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
    public class TextSprite : Element
    {
        public Rectangle BoundingBox { get; set; }
        public override Vector2 Position { get { return _position; } set { _position = value; reloadBoundingBox(); } }
        public Vector2 Scale { get { return _scale; } set { _scale = value; reloadBoundingBox(); } }
        public SpriteFont Font { get; set; }
        public string Text { get; set; }
        public SpriteEffects spriteEffects;
        public float ScaledDrawScale;
        protected Vector2 _position;
        protected Vector2 _scale;

        //Extra
        public float Rotation;
        public Color Color;

        public TextSprite(string eFontName, string eText, Vector2 ePosition, Color eColor) : base(true)
        {
            Font = ResourceManager.getFont(eFontName);

            spriteEffects = SpriteEffects.None;
            ScaledDrawScale = 1f;
            _position = ePosition;

            Text = eText;

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

        public void reloadBoundingBox()
        {
            Vector2 Measured = Font.MeasureString(Text);
            BoundingBox = new Rectangle((int)(Position.X - Measured.X / 2), (int)(Position.Y - Measured.Y / 2), (int)Measured.X, (int)Measured.Y);
        }

        public override void drawElement(GameTime gameTime)
        {
            base.DrawString(Font, Text, Color, Rotation, Scale, spriteEffects); 
        }
    }
}
