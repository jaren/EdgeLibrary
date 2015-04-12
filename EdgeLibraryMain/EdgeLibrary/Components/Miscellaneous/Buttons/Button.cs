using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeLibrary
{
    public enum ButtonClickState
    {
        MousedOver,
        Clicked,
        Normal
    }

    public class Button : Sprite
    {
        public delegate void ButtonEvent(Button sender, GameTime gameTime);
        public virtual event ButtonEvent OnClick;
        public virtual event ButtonEvent OnRelease;
        public virtual event ButtonEvent OnMouseOver;
        public virtual event ButtonEvent OnMouseOff;

        public bool LeftClick;

        protected ButtonClickState ButtonState;

        protected bool wasContained;

        public Style Style;

        public Button(string texture, Vector2 position)
            : base(texture, position)
        {
            LeftClick = true;

            Style = new Style(Texture, Color, Texture, Color, Texture, Color);

            ButtonState = ButtonClickState.Normal;
        }

        protected virtual void ChangeState(ButtonClickState state)
        {
            ButtonState = state;

            switch (ButtonState)
            {
                case ButtonClickState.Normal:
                    Texture = Style.NormalTexture;
                    Color = Style.NormalColor;
                    break;
                case ButtonClickState.Clicked:
                    Texture = Style.ClickTexture;
                    Color = Style.ClickColor;
                    break;
                case ButtonClickState.MousedOver:
                    Texture = Style.MouseOverTexture;
                    Color = Style.MouseOverColor;
                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (BoundingBox.Contains(new Point((int)Input.MousePosition.X, (int)Input.MousePosition.Y)))
            {
                if (LeftClick ? Input.JustLeftClicked() : Input.JustRightClicked() )
                {
                    ChangeState(ButtonClickState.Clicked);
                    if (OnClick != null)
                    {
                        OnClick(this, gameTime);
                    }
                }
                else if (LeftClick ? Input.JustReleasedLeftClick() : Input.JustReleasedRightClick())
                {
                    ChangeState(ButtonClickState.MousedOver);
                    if (OnRelease != null)
                    {
                        OnRelease(this, gameTime);
                    }
                }
                else if (LeftClick ? !Input.IsLeftClicking() : !Input.IsLeftClicking())
                {
                    ChangeState(ButtonClickState.MousedOver);
                    if (OnMouseOver != null)
                    {
                        OnMouseOff(this, gameTime);
                    }
                }

                wasContained = true;
            }
            else if (wasContained)
            {
                ChangeState(ButtonClickState.Normal);
                if (OnMouseOff != null)
                {
                    OnMouseOff(this, gameTime);
                }
                wasContained = false;
            }

            base.Update(gameTime);
        }
    }
}
