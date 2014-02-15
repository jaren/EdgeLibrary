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

    public class ButtonToggle : Sprite
    {
        protected bool On;

        protected bool HasReleasedMouseClick;
        protected bool HasChangedMouseOver;
        protected bool HasChangedMouseOff;

        public Button ButtonOn { get; set; }
        public Button ButtonOff { get; set; }

        public delegate void ButtonToggleEvent(ButtonToggleEventArgs e);
        public event ButtonToggleEvent Click;
        public event ButtonToggleEvent MouseOver;
        public event ButtonToggleEvent MouseOff;

        public ButtonToggle(Button buttonOn, Button buttonOff) : base("", buttonOn.Position, (int)buttonOn.Width, (int)buttonOn.Height)
        {
            EdgeGame.SelectedScene.RemoveElement(buttonOn);
            EdgeGame.SelectedScene.RemoveElement(buttonOff);

            On = true;
            ButtonOn = buttonOn;
            ButtonOn.Visible = true;
            ButtonOff = buttonOff;
            ButtonOff.Visible = false;
            HasReleasedMouseClick = false;
            HasChangedMouseOff = false;
            HasChangedMouseOver = false;
        }

        public override void  updateElement(GameTime gameTime)
        {
            base.updateElement(gameTime);

            if (!InputManager.LeftClick()) { HasReleasedMouseClick = true; }

            if (BoundingBox.Contains(new Rectangle((int)InputManager.MousePos().X, (int)InputManager.MousePos().Y, 1, 1)))
            {
                if (InputManager.LeftClick())
                {
                    if (HasReleasedMouseClick)
                    {
                        ButtonToggleEventArgs e = new ButtonToggleEventArgs();
                        e.button = this;

                        if (On)
                        {
                            ButtonOn.Visible = false;
                            ButtonOff.Visible = true;
                            On = false;
                            e.isOn = On;
                            if (Click != null)
                            {
                                Click(e);
                            }
                        }
                        else
                        {
                            ButtonOn.Visible = true;
                            ButtonOff.Visible = false;
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

        public override void drawElement(GameTime gameTime)
        {
            if (On)
            {
                Texture = ButtonOn.Texture;
                Scale = ButtonOn.Scale;
                Color = ButtonOn.Color;
                Rotation = ButtonOn.Rotation;
                spriteEffects = ButtonOn.spriteEffects;
            }
            else
            {
                Texture = ButtonOff.Texture;
                Scale = ButtonOff.Scale;
                Color = ButtonOff.Color;
                Rotation = ButtonOff.Rotation;
                spriteEffects = ButtonOff.spriteEffects;
            }

            base.drawElement(gameTime);
        }
    }
}
