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
        public Color offColor;
        public Color onColor;
        public Texture2D onTexture;
        public Texture2D offTexture;
        private string onData;
        private string offData;

        private Vector2 previousPosition;

        protected bool launchedMouseOver;
        protected bool launchedMouseOff;

        public delegate void ButtonEventHandler(ButtonEventArgs e);
        public virtual event ButtonEventHandler Click;
        public virtual event ButtonEventHandler MouseOver;
        public virtual event ButtonEventHandler MouseOff;

        public Button(string id, string eTextureName, Vector2 ePosition, int eWidth, int eHeight, Color eClickColor) : base(id, eTextureName, ePosition, eWidth, eHeight)
        {
            label = new TextSprite(string.Format("{0}_label", id), "", "", Vector2.Zero, Color.White);
            label.REMOVE();
            previousPosition = Position;
            onColor = eClickColor;
            offData = eTextureName;
            init();
        }

        public Button(string id, string eTextureName, Vector2 ePosition, int eWidth, int eHeight, Color eClickColor, Color eColor, float eRotation, Vector2 eScale) : base(id, eTextureName, ePosition, eWidth, eHeight, eColor, eRotation, eScale)
        {
            onColor = eClickColor;
            offData = eTextureName;
            init();
        }

        public void setClickTexture(string textureName)
        {
            onData = textureName;
        }

        protected void init()
        {
            Visible = true;
            offColor = Color.White;
            launchedMouseOver = false;
            launchedMouseOff = false;

            reloadLabel();

            if (onData == null)
            {
                onTexture = ResourceManager.getTexture(offData);
            }
            else
            {
                onTexture = ResourceManager.getTexture(onData);
            }
            offTexture = ResourceManager.getTexture(offData);

            Texture = offTexture;
        }

        //Used for elements inheriting from this which need to access the sprite
        protected void UpdateSpritePortion(GameTime gameTime)
        {
            base.updateElement(gameTime);
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

            Vector2 mousePosition = InputManager.MousePos();

            if (!InputManager.LeftClick())
            {
                Color = offColor;
            }

            if (BoundingBox.Contains(new Rectangle((int)mousePosition.X, (int)mousePosition.Y, 1, 1)))
            {
                Color = onColor;
                Texture = onTexture;

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
                    if (Click != null)
                    {
                        Click(clickArgs);
                    }
                }
            }
            else
            {
                Color = offColor;
                Texture = offTexture;

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
