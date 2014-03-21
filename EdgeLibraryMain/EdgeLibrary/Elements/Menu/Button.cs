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
using System.Xml;

namespace EdgeLibrary
{
    public class ButtonEventArgs : EventArgs
    {
        public Button button;
        public Vector2 clickPosition;
    }

    public class Button : Sprite
    {
        public TextSprite label;
        public SpriteStyle OnStyle;
        public SpriteStyle OffStyle;
        public SpriteStyle MouseOverStyle;
        public Vector2 OnScale;
        public Vector2 OffScale;
        public Vector2 MouseOverScale;

        private Vector2 previousPosition;

        protected bool launchedMouseOver;
        protected bool launchedMouseOff;

        protected Texture2D OnTexture;
        protected Texture2D OffTexture;

        public delegate void ButtonEventHandler(ButtonEventArgs e);
        public virtual event ButtonEventHandler Click;
        public virtual event ButtonEventHandler MouseOver;
        public virtual event ButtonEventHandler MouseOff;

        public Button(string eTextureName, Vector2 ePosition, Color eClickColor) : this(MathTools.RandomID(typeof(Button)), eTextureName, ePosition, eClickColor) { }

        public Button(string id, string eTextureName, Vector2 ePosition, Color eClickColor) : base(id, eTextureName, ePosition)
        {
            OnStyle = new SpriteStyle();
            OffStyle = new SpriteStyle();
            MouseOverStyle = new SpriteStyle();
            OnScale = Vector2.One;
            OffScale = OnScale;
            MouseOverScale = OnScale;

            CollisionBodyType = ShapeTypes.rectangle;
            
            label = new TextSprite(string.Format("{0}_label", id), "", "", Vector2.Zero, Color.White);
            label.REMOVE();
            previousPosition = Position;
            OnStyle.Color = eClickColor;
            OffTexture = ResourceManager.getTexture(eTextureName);
            OnTexture = OffTexture;

            Visible = true;
            OffStyle.Color = Color.White;
            launchedMouseOver = false;
            launchedMouseOff = false;

            reloadLabel();

            Texture = OffTexture;

            MouseOverStyle = OffStyle;
        }

        public void setClickTexture(string textureName)
        {
            Texture = ResourceManager.getTexture(textureName);
        }

        //Used for elements inheriting from this which need to access the sprite
        protected void UpdateSpritePortion(GameTime gameTime)
        {
            base.updateElement(gameTime);
        }

        public void SetStyle(SpriteStyle style)
        {
            OnStyle = style;
            OffStyle = style;
            MouseOverStyle = style;
        }

        public void SetScale(Vector2 scale)
        {
            OnScale = scale;
            OffScale = scale;
            MouseOverScale = scale;
        }

        protected void reloadLabel()
        {
            if (label.Font != null)
            {
                label.Position = Position - label.Font.MeasureString(label.Text) / 2;
                label.DrawLayer = this.DrawLayer - 1;
            }
        }

        protected override void updateElement(GameTime gameTime)
        {
            if (previousPosition != Position)
            {
                reloadLabel();
            }

            base.updateElement(gameTime);

            Vector2 mousePosition = InputManager.MousePosition;

            if (CollisionBody.Shape.CollidesWith(new ShapeRectangle(new Vector2((int)mousePosition.X, (int)mousePosition.Y), 1, 1)))
            {
                Style = MouseOverStyle;
                Scale = MouseOverScale;
                if (OnTexture != null)
                {
                    Texture = OnTexture;
                }

                ButtonEventArgs clickArgs = new ButtonEventArgs();
                clickArgs.button = this;
                clickArgs.clickPosition = mousePosition;

                if (!launchedMouseOver)
                {
                    if (MouseOver != null)
                    {
                        MouseOver(clickArgs);
                    }
                    launchedMouseOver = true;
                    launchedMouseOff = false;
                }

                if (InputManager.LeftClick())
                {
                    Style = OnStyle;
                    Texture = OnTexture;
                    Scale = OnScale;

                    if (Click != null)
                    {
                        Click(clickArgs);
                    }
                }
            }
            else
            {
                Style = OffStyle;
                Scale = OffScale;
                Texture = OffTexture;

                if (!launchedMouseOff)
                {
                    ButtonEventArgs clickArgs = new ButtonEventArgs();
                    clickArgs.button = this;
                    clickArgs.clickPosition = Vector2.Zero;
                    if (MouseOff != null)
                    {
                        MouseOff(clickArgs);
                    }
                    launchedMouseOver = false;
                    launchedMouseOff = true;
                }
            }

            previousPosition = Position;
        }
    }
}
