using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    //Used for buttons
    public struct Style
    {
        public Texture2D MouseOverTexture;
        public Color MouseOverColor;
        public Texture2D NormalTexture;
        public Color NormalColor;
        public Texture2D ClickTexture;
        public Color ClickColor;

        public Color AllColors
        {
            set
            {
                NormalColor = value;
                ClickColor = value;
                MouseOverColor = value;
            }
        }

        public Texture2D AllTextures
        {
            set
            {
                NormalTexture = value;
                ClickTexture = value;
                MouseOverTexture = value;
            }
        }

        public Style(Texture2D mouseOverTexture, Color mouseOverColor, Texture2D normalTexture, Color normalColor, Texture2D clickTexture, Color clickColor)
        {
            MouseOverTexture = mouseOverTexture;
            MouseOverColor = mouseOverColor;
            NormalTexture = normalTexture;
            NormalColor = normalColor;
            ClickTexture = clickTexture;
            ClickColor = clickColor;
        }
    }
}
