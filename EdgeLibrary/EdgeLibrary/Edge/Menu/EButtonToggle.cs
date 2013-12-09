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

namespace EdgeLibrary.Edge
{
    public class ButtonToggleEventArgs : EventArgs
    {
        public EButtonToggle button;
        public bool isOn;
        public Vector2 clickPosition;
    }

    public class EButtonToggle : ESprite
    {
        protected bool On;

        protected bool HasBeenPressed;

        public EButton ButtonOn { get; set; }
        public EButton ButtonOff { get; set; }

        public EButtonToggle(EButton eButtonOn, EButton eButtonOff) : base(eButtonOn.Data, eButtonOn.Position, (int)eButtonOn.Width, (int)eButtonOn.Height)
        {
            On = true;
            ButtonOn = eButtonOn;
            ButtonOn.IsVisible = true;
            ButtonOn.IsActive = true;
            ButtonOff = eButtonOff;
            ButtonOff.IsVisible = false;
            ButtonOff.IsActive = false;

            ButtonOn.Click += new EButton.ButtonEventHandler(ButtonClick);
            ButtonOff.Click += new EButton.ButtonEventHandler(ButtonClick);
        }

        public void ButtonClick(ButtonEventArgs e)
        {
            HasBeenPressed = true;
        }

        public override void updateElement(EUpdateArgs updateArgs)
        {
            if (HasBeenPressed)
            {
                if (On)
                {
                    ButtonOn.IsActive = false;
                    ButtonOn.IsVisible = false;
                    ButtonOff.IsActive = true;
                    ButtonOff.IsVisible = true;
                }
                else
                {
                    ButtonOn.IsActive = true;
                    ButtonOn.IsVisible = true;
                    ButtonOff.IsActive = false;
                    ButtonOff.IsVisible = false;
                }
                HasBeenPressed = false;
            }
        }

        public override void drawElement(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsVisible)
            {
                if (On)
                {
                    spriteBatch.Draw(ButtonOn.Texture, ButtonOn.BoundingBox, null, ButtonOn.Color, (float)EdgeGame.DegreesToRadians(ButtonOn.Rotation), Vector2.Zero, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(ButtonOff.Texture, ButtonOff.BoundingBox, null, ButtonOff.Color, (float)EdgeGame.DegreesToRadians(ButtonOff.Rotation), Vector2.Zero, SpriteEffects.None, 0);
                }
            }
        }
    }
}
