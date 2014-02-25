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
    public class ButtonToggleEventArgs : EventArgs
    {
        public ButtonToggle button;
        public bool isOn;
    }

    public class ButtonToggle : Button
    {
        protected bool On;

        protected bool HasReleasedMouseClick;
        protected bool HasChangedMouseOver;
        protected bool HasChangedMouseOff;

        public delegate void ButtonToggleEvent(ButtonToggleEventArgs e);
        public new event ButtonToggleEvent Click;
        public new event ButtonToggleEvent MouseOver;
        public new event ButtonToggleEvent MouseOff;

        public ButtonToggle(string id, string eTextureName, Vector2 ePosition, Color eClickColor) : base(id, eTextureName, ePosition, eClickColor)
        {
            On = false;
            HasReleasedMouseClick = false;
            HasChangedMouseOff = false;
            HasChangedMouseOver = false;
        }

        protected override void updateElement(GameTime gameTime)
        {
            base.updateElement(gameTime);

            if (!InputManager.LeftClick()) { HasReleasedMouseClick = true; }

            if (BoundingBox.Contains(new Rectangle((int)InputManager.MousePos().X, (int)InputManager.MousePos().Y, 1, 1)))
            {
                Style = MouseOverStyle;
                Scale = MouseOverScale;

                if (InputManager.LeftClick())
                {
                    if (HasReleasedMouseClick)
                    {
                        ButtonToggleEventArgs e = new ButtonToggleEventArgs();
                        e.button = this;

                        if (On)
                        {
                            Style = OnStyle;
                            Scale = OnScale;
                            On = false;
                            e.isOn = On;
                            if (Click != null)
                            {
                                Click(e);
                            }
                        }
                        else
                        {
                            Style = OffStyle;
                            Scale = OffScale;
                            On = true;
                        }

                        e.isOn = On;
                        if (Click != null)
                        {
                            Click(e);
                        }

                        HasReleasedMouseClick = false;
                    }
                }
                else
                {
                    if (!HasChangedMouseOver)
                    {
                        ButtonToggleEventArgs e = new ButtonToggleEventArgs();
                        e.isOn = On;
                        e.button = this;
                        if (MouseOver != null)
                        {
                            MouseOver(e);
                        }
                        HasChangedMouseOff = false;
                        HasChangedMouseOver = true;
                    }
                }
            }
            else
            {
                if (On)
                {
                    Style = OnStyle;
                }
                else
                {
                    Style = OffStyle;
                }

                if (!HasChangedMouseOff)
                {
                    ButtonToggleEventArgs e = new ButtonToggleEventArgs();
                    e.isOn = On;
                    e.button = this;
                    if (MouseOff != null)
                    {
                        MouseOff(e);
                    }
                    HasChangedMouseOff = true;
                    HasChangedMouseOver = false;
                }
            }
        }

        protected override void drawElement(GameTime gameTime)
        {
            if (On)
            {
                Texture = OnTexture;
            }
            else
            {
                Texture = OffTexture;
            }

            base.drawElement(gameTime);
        }
    }
}
