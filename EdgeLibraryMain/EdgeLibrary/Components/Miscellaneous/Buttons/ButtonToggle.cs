using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeLibrary
{
    public class ButtonToggle : Button
    {
        public bool On = false;

        public Style OnStyle;
        public Style OffStyle;

        public ButtonToggle(string texture, Vector2 position) : base(texture, position)
        {
            OnStyle = Style;
            OffStyle = Style;

            OnClick += new ButtonEvent(ButtonToggle_OnClick);
        }

        void ButtonToggle_OnClick(Button sender, GameTime gameTime)
        {
            if (On)
            {
                On = false;
                Style = OffStyle;
            }
            else
            {
                On = true;
                Style = OnStyle;
            }
        }
    }
}
