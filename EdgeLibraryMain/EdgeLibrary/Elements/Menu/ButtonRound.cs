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

        public ButtonRound(string id, string eTextureName, Vector2 ePosition, int radius, Color eClickColor) : base(id, eTextureName, ePosition, eClickColor)
        {
            Radius = radius;
            _width = radius * 2;
            _height = radius * 2;
        }

        protected override void updateElement(GameTime gameTime)
        {
            base.UpdateSpritePortion(gameTime);

            Vector2 mousePosition = InputManager.MousePos();

            if (Vector2.Distance(Position, mousePosition) <= Radius)
            {
                Style = MouseOverStyle;
                Scale = MouseOverScale;

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
        }
    }
}
