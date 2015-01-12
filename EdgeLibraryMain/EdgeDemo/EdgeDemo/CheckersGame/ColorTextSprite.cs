using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    //A sprite which runs an 'AColorChange' action on demand
    public class ColorTextSprite : TextSprite
    {
        public ColorChangeIndex Colors;

        public ColorTextSprite(string fontName, Vector2 position, ColorChangeIndex colors) : base(fontName, "", position)
        {
            Colors = colors;
        }

        public void Display(string text)
        {
            Text = text;
            AddAction(new AColorChange(Colors));
        }
    }
}
