using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    public class ButtonToggle : Button
    {
        public bool On = false;

        public Texture2D OnMouseOverTexture;
        public Color OnMouseOverColor;
        public Texture2D OnNormalTexture;
        public Color OnNormalColor;
        public Texture2D OnClickTexture;
        public Color OnClickColor;

        public Texture2D OffMouseOverTexture;
        public Color OffMouseOverColor;
        public Texture2D OffNormalTexture;
        public Color OffNormalColor;
        public Texture2D OffClickTexture;
        public Color OffClickColor;

        public ButtonToggle(string texture, Vector2 position) : base(texture, position)
        {
            OnMouseOverTexture = MouseOverTexture;
            OnMouseOverColor = MouseOverColor;
            OnNormalTexture = NormalTexture;
            OnNormalColor = NormalColor;
            OnClickTexture = ClickTexture;
            OnClickColor = ClickColor;

            OffMouseOverTexture = MouseOverTexture;
            OffMouseOverColor = MouseOverColor;
            OffNormalTexture = NormalTexture;
            OffNormalColor = NormalColor;
            OffClickTexture = ClickTexture;
            OffClickColor = ClickColor;

            OnClick += new ButtonEvent(ButtonToggle_OnClick);
        }

        public void SetOnTextures(Texture2D texture)
        {
            OnNormalTexture = texture;
            OnClickTexture = texture;
            OnMouseOverTexture = texture;
        }

        public void SetOnColors(Color color)
        {
            OnNormalColor = color;
            OnClickColor = color;
            OnMouseOverColor = color;
        }

        public void SetOffTextures(Texture2D texture)
        {
            OffNormalTexture = texture;
            OffClickTexture = texture;
            OffMouseOverTexture = texture;
        }

        public void SetOffColors(Color color)
        {
            OffNormalColor = color;
            OffClickColor = color;
            OffMouseOverColor = color;
        }

        public override void SetTextures(Texture2D texture)
        {
            base.SetTextures(texture);
            SetOnTextures(texture);
            SetOffTextures(texture);
        }

        public override void SetColors(Color color)
        {
            base.SetColors(color);
            SetOnColors(color);
            SetOffColors(color);
        }

        void ButtonToggle_OnClick(Button sender, GameTime gameTime)
        {
            if (On)
            {
                On = false;

                MouseOverTexture = OffMouseOverTexture;
                MouseOverColor = OffMouseOverColor;
                NormalTexture = OffNormalTexture;
                NormalColor = OffNormalColor;
                ClickTexture = OffClickTexture;
                ClickColor = OffClickColor;
            }
            else
            {
                On = true;

                MouseOverTexture = OnMouseOverTexture;
                MouseOverColor = OnMouseOverColor;
                NormalTexture = OnNormalTexture;
                NormalColor = OnNormalColor;
                ClickTexture = OnClickTexture;
                ClickColor = OnClickColor;
            }
        }
    }
}
