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

namespace EdgeLibrary.Edge
{
    public class ButtonEventArgs : EventArgs
    {
        public EButton button;
        public Vector2 clickPosition;
    }

    public class EButton : ESprite
    {
        //Not yet implemented
        protected ELabel label;
        protected Color clickColor;

        protected bool launchedMouseOver;
        protected bool launchedMouseOff;
        
        public delegate void ButtonEventHandler(ButtonEventArgs e);
        public event ButtonEventHandler Click;
        public event ButtonEventHandler MouseOver;
        public event ButtonEventHandler MouseOff;

        public EButton(string eTextureName, Vector2 ePosition, int eWidth, int eHeight, Color eClickColor) : base(eTextureName, ePosition, eWidth, eHeight)
        {
            IsActive = true;
            clickColor = eClickColor;
            init();
        }

        public EButton(string eTextureName, Vector2 ePosition, int eWidth, int eHeight, Color eClickColor, Color eColor, float eRotation, Vector2 eScale) : base(eTextureName, ePosition, eWidth, eHeight, eColor, eRotation, eScale)
        {
            IsActive = true;
            clickColor = eClickColor;
            init();
        }

        protected void init()
        {
            Click += new ButtonEventHandler(nullHandler);
            MouseOver += new ButtonEventHandler(nullHandler);
            MouseOff += new ButtonEventHandler(nullHandler);

            launchedMouseOver = false;
            launchedMouseOff = false;
        }

        protected void nullHandler(ButtonEventArgs e) { }

        public override void updateElement(EUpdateArgs updateArgs)
        {
            base.updateElement(updateArgs);

            Vector2 mousePosition = new Vector2(updateArgs.mouseState.X, updateArgs.mouseState.Y);

            if (updateArgs.mouseState.LeftButton == ButtonState.Released)
            {
                Color = Color.White;
            }

            if (BoundingBox.Contains(new Rectangle((int)mousePosition.X, (int)mousePosition.Y, 1, 1)))
            {
                Color = clickColor;

                ButtonEventArgs clickArgs = new ButtonEventArgs();
                clickArgs.button = this;
                clickArgs.clickPosition = mousePosition;

                if (!launchedMouseOver)
                {
                    MouseOver(clickArgs);
                    launchedMouseOver = true;
                    launchedMouseOff = false;
                }

                if (updateArgs.mouseState.LeftButton == ButtonState.Pressed)
                {
                    Click(clickArgs);
                }
            }
            else
            {
                if (!launchedMouseOff)
                {
                    ButtonEventArgs clickArgs = new ButtonEventArgs();
                    clickArgs.button = this;
                    clickArgs.clickPosition = Vector2.Zero;
                    MouseOff(clickArgs);
                    launchedMouseOver = false;
                    launchedMouseOff = true;
                }
            }
        } 
    }
}
