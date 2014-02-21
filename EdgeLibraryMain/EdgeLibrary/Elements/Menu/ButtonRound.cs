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
    public class ButtonRound : Button
    {
        public int Radius;

        public override event ButtonEventHandler Click;
        public override event ButtonEventHandler MouseOver;
        public override event ButtonEventHandler MouseOff;

        public ButtonRound(string id, string eTextureName, Vector2 ePosition, int radius, Color eClickColor) : base(id, eTextureName, ePosition, radius * 2, radius * 2, eClickColor)
        {
            Radius = radius;
        }

        public ButtonRound(string id, string eTextureName, Vector2 ePosition, int radius, Color eClickColor, Color eColor, float eRotation, Vector2 eScale) : base(id, eTextureName, ePosition, radius * 2, radius * 2, eClickColor, eColor, eRotation, eScale)
        {
            Radius = radius;
        }

        protected override void updateElement(GameTime gameTime)
        {
            base.UpdateSpritePortion(gameTime);

            Vector2 mousePosition = InputManager.MousePos();

            if (!InputManager.LeftClick())
            {
                Color = onColor;
            }

            if (Vector2.Distance(Position, mousePosition) <= Radius)
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
        }
    }
}
