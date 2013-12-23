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
using EdgeLibrary.Basic;

namespace EdgeLibrary.Menu
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

        protected bool HasReleasedMouseClick;
        protected bool HasChangedMouseOver;
        protected bool HasChangedMouseOff;

        public EButton ButtonOn { get; set; }
        public EButton ButtonOff { get; set; }

        public delegate void ButtonToggleEvent(ButtonToggleEventArgs e);
        public event ButtonToggleEvent Click;
        public event ButtonToggleEvent MouseOver;
        public event ButtonToggleEvent MouseOff;

        public EButtonToggle(EButton eButtonOn, EButton eButtonOff) : base(eButtonOn.Data, eButtonOn.Position, (int)eButtonOn.Width, (int)eButtonOn.Height)
        {
            On = true;
            ButtonOn = eButtonOn;
            ButtonOn.IsVisible = true;
            ButtonOn.IsActive = true;
            ButtonOff = eButtonOff;
            ButtonOff.IsVisible = false;
            ButtonOff.IsActive = false;
            HasReleasedMouseClick = false;
            HasChangedMouseOff = false;
            HasChangedMouseOver = false;
        }

        public override void OnAddToScene(EScene scene)
        {
            ButtonOn.Texture = scene.edgeData.getTexture(ButtonOn.Data);
            ButtonOff.Texture = scene.edgeData.getTexture(ButtonOff.Data);
        }

        public override void updateElement(EUpdateArgs updateArgs)
        {
            if (updateArgs.mouseState.LeftButton == ButtonState.Released) { HasReleasedMouseClick = true; }

            if (BoundingBox.Contains(new Rectangle(updateArgs.mouseState.X, updateArgs.mouseState.Y, 1, 1)))
            {
                if (updateArgs.mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (HasReleasedMouseClick)
                    {
                        ButtonToggleEventArgs e = new ButtonToggleEventArgs();
                        e.button = this;
                        e.clickPosition = new Vector2(updateArgs.mouseState.X, updateArgs.mouseState.Y);

                        if (On)
                        {
                            ButtonOn.IsActive = false;
                            ButtonOn.IsVisible = false;
                            ButtonOff.IsActive = true;
                            ButtonOff.IsVisible = true;
                            On = false;
                            e.isOn = On;
                            if (Click != null)
                            {
                                Click(e);
                            }
                        }
                        else
                        {
                            ButtonOn.IsActive = true;
                            ButtonOn.IsVisible = true;
                            ButtonOff.IsActive = false;
                            ButtonOff.IsVisible = false;
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
                        e.clickPosition = new Vector2(updateArgs.mouseState.X, updateArgs.mouseState.Y);
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
                    e.clickPosition = new Vector2(updateArgs.mouseState.X, updateArgs.mouseState.Y);
                    if (MouseOff != null)
                    {
                        MouseOff(e);
                    }
                    HasChangedMouseOff = true;
                    HasChangedMouseOver = false;
                }
            }
        }

        public override void drawElement(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsVisible)
            {
                if (On)
                {
                    spriteBatch.Draw(ButtonOn.Texture, ButtonOn.BoundingBox, null, ButtonOn.Color, (float)EMath.DegreesToRadians(ButtonOn.Rotation), Vector2.Zero, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(ButtonOff.Texture, ButtonOff.BoundingBox, null, ButtonOff.Color, (float)EMath.DegreesToRadians(ButtonOff.Rotation), Vector2.Zero, SpriteEffects.None, 0);
                }
            }
        }
    }
}
