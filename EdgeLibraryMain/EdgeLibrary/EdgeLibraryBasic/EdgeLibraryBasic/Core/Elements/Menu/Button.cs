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
        public Label label;
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

        public Button(string eTextureName, Vector2 ePosition, int eWidth, int eHeight, Color eClickColor) : base(eTextureName, ePosition, eWidth, eHeight)
        {
            label = new Label("", new Vector2(0, 0), "", Color.White);
            previousPosition = Position;
            onColor = eClickColor;
            offData = eTextureName;
            init();
        }

        public Button(string eTextureName, Vector2 ePosition, int eWidth, int eHeight, Color eClickColor, Color eColor, float eRotation, Vector2 eScale) : base(eTextureName, ePosition, eWidth, eHeight, eColor, eRotation, eScale)
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
            IsActive = true;
            offColor = Color.White;
            launchedMouseOver = false;
            launchedMouseOff = false;
        }

        public override void OnAddToLayer(Layer layer)
        {
            layer.addElement(label);
        }

        public override void FillTexture()
        {
            if (label != null)
            {
                label.FillTexture();
            }
            reloadLabel();

                if (onData == null)
                {
                    onTexture = ResourceData.getTexture(offData);
                }
                else
                {
                    onTexture = ResourceData.getTexture(onData);
                }
            offTexture = ResourceData.getTexture(offData);

            Texture = offTexture;
        }

        protected void UpdatSpritePortion(UpdateArgs updateArgs)
        {
            base.updatElement(updateArgs);
        }

        protected void reloadLabel()
        {
            if (label.Font != null)
            {
                label.Position = Position - label.Font.MeasureString(label.Text) / 2;
                label.DrawLayer = this.DrawLayer + 1;
            }
        }

        public override void updatElement(UpdateArgs updateArgs)
        {
            if (previousPosition != Position)
            {
                reloadLabel();
            }

            base.updatElement(updateArgs);

            Vector2 mousePosition = new Vector2(updateArgs.mouseState.X, updateArgs.mouseState.Y);

            if (updateArgs.mouseState.LeftButton == ButtonState.Released)
            {
                Color = Color.White;
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

                if (updateArgs.mouseState.LeftButton == ButtonState.Pressed)
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
