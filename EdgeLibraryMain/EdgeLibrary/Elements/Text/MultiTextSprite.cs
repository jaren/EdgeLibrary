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
    //A text sprite which splits the text into multiple lines if too long
    public class MultiTextSprite : TextSprite
    {
        public float MaxWidth;
        public float LineDifferenceFactor { get { return _lineDifferenceFactor; } set { _lineDifferenceFactor = value; reloadLineDifference(); } }
        private float _lineDifferenceFactor;
        private float lineDifference;

        public MultiTextSprite(string id, string fontName, string text, Vector2 position, Color color) : base(id, fontName, text, position, color)
        {
            MaxWidth = 300;
            _lineDifferenceFactor = 1;
            reloadLineDifference();
        }

        public MultiTextSprite(string id, string fontName, string text, Vector2 position, Color color, float maxWidth, float lineDifferenceFactor) : this(id, fontName, text, position, color)
        {
            MaxWidth = maxWidth;
            _lineDifferenceFactor = lineDifferenceFactor;
            reloadLineDifference();
        }

        //Work in Progress
        private List<string> splitText()
        {
            List<string> list = new List<string>();
            string lastLine = _text;
            while (Font.MeasureString(lastLine).X > MaxWidth)
            {
                int charsToRemove = 0;
                while(Font.MeasureString(lastLine.Remove(lastLine.Length - 1 - charsToRemove)).X > MaxWidth)
                {
                    charsToRemove++;
                }
                list.Add(lastLine.Remove(lastLine.Length - charsToRemove));
                lastLine = lastLine.Remove(0, lastLine.Length - charsToRemove);
            }
            list.Add(lastLine);
            return list;
        }

        private void reloadLineDifference()
        {
            lineDifference = Font.MeasureString("A").Y * _lineDifferenceFactor;
        }

        public override void reloadBoundingBox()
        {
            if (Font != null && MaxWidth > 0)
            {
                Vector2 Measured = Vector2.Zero;
                int maxX = 0;
                foreach (string s in splitText())
                {
                    if (Font.MeasureString(s).X > maxX)
                    {
                        maxX = (int)Font.MeasureString(s).X;
                    }
                    Measured.Y += Font.MeasureString(s).Y;
                }
                BoundingBox = new Rectangle((int)(Position.X - Measured.X / 2), (int)(Position.Y - Measured.Y / 2), (int)Measured.X, (int)Measured.Y);
                _width = BoundingBox.Width;
                _height = BoundingBox.Height;
            }
        }

        protected Vector2 MeasuredPosition(string text, int index)
        {
            if (Font != null)
            {
                return new Vector2(Position.X - Font.MeasureString(text).X / 2, Position.Y - Font.MeasureString(text).Y / 2 + lineDifference*index);
            }
            return Position;
        }
        
        public override void drawElement(GameTime gameTime)
        {
            List<string> text = splitText();
            for (int i = 0; i < text.Count; i++)
            {
                EdgeGame.drawString(Font, text[i], MeasuredPosition(text[i], i), Color, Rotation, OriginPoint, Scale, spriteEffects, LayerDepth); 
            }
        }
    }
}
