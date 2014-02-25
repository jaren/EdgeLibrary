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
        public event ButtonToggleEvent Click;
        public event ButtonToggleEvent MouseOver;
        public event ButtonToggleEvent MouseOff;

        public ButtonToggle(string id, string eTextureName, Vector2 ePosition, Color eClickColor) : base(id, eTextureName, ePosition, eClickColor)
        {
            On = true;
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

                if (InputManager.LeftClick())
                {
                    if (HasReleasedMouseClick)
                    {
                        ButtonToggleEventArgs e = new ButtonToggleEventArgs();
                        e.button = this;

                        if (On)
                        {
                            Style = OnStyle;
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
                    if (On)
                    {
                        Style = OnStyle;
                    }
                    else
                    {
                        Style = OffStyle;
                    }

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
