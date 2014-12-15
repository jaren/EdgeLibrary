using EdgeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
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

        protected ButtonClickState ButtonState;

        protected bool wasContained;

        public Texture2D MouseOverTexture;
        public Color MouseOverColor;
        public Texture2D NormalTexture;
        public Color NormalColor;
        public Texture2D ClickTexture;
        public Color ClickColor;

        public Button(string texture, Vector2 position)
            : base(texture, position)
        {
            MouseOverTexture = Texture;
            NormalTexture = Texture;
            ClickTexture = Texture;

            MouseOverColor = Color;
            NormalColor = Color;
            ClickColor = Color;

            ButtonState = ButtonClickState.Normal;
        }

        protected virtual void ChangeState(ButtonClickState state)
        {
            ButtonState = state;

            switch (ButtonState)
            {
                case ButtonClickState.Normal:
                    Texture = NormalTexture;
                    Color = NormalColor;
                    break;
                case ButtonClickState.Clicked:
                    Texture = ClickTexture;
                    Color = ClickColor;
                    break;
                case ButtonClickState.MousedOver:
                    Texture = MouseOverTexture;
                    Color = MouseOverColor;
                    break;
            }
        }

        public virtual void SetTextures(Texture2D texture)
        {
            NormalTexture = texture;
            MouseOverTexture = texture;
            ClickTexture = texture;
        }

        public virtual void SetColors(Color color)
        {
            NormalColor = color;
            MouseOverColor = color;
            ClickColor = color;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (BoundingBox.Contains(new Point((int)Input.MousePosition.X, (int)Input.MousePosition.Y)))
            {
                if (Input.JustLeftClicked())
                {
                    ChangeState(ButtonClickState.Clicked);
                    if (OnClick != null)
                    {
                        OnClick(this, gameTime);
                    }
                }
                else if (Input.JustReleasedLeftClick())
                {
                    ChangeState(ButtonClickState.MousedOver);
                    if (OnRelease != null)
                    {
                        OnRelease(this, gameTime);
                    }
                }
                else if (!Input.IsLeftClicking())
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
