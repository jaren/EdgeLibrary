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

        public ButtonRound(string eTextureName, Vector2 ePosition, int radius, Color eClickColor): base(eTextureName, ePosition, radius * 2, radius * 2, eClickColor)
        {
            Radius = radius;
        }

        public ButtonRound(string eTextureName, Vector2 ePosition, int radius, Color eClickColor, Color eColor, float eRotation, Vector2 eScale): base(eTextureName, ePosition, radius * 2, radius * 2, eClickColor, eColor, eRotation, eScale)
        {
            Radius = radius;
        }

        public override void updatElement(UpdateArgs updateArgs)
        {
            base.UpdatSpritePortion(updateArgs);

            Vector2 mousePosition = new Vector2(updateArgs.mouseState.X, updateArgs.mouseState.Y);

            if (updateArgs.mouseState.LeftButton == ButtonState.Released)
            {
                Color = Color.White;
            }

            if (MathTools.DistanceBetween(Position, mousePosition) <= Radius)
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
                    MouseOff(clickArgs);
                    launchedMouseOver = false;
                    launchedMouseOff = true;
                }
            }
        }
    }
}
