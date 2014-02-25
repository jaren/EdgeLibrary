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

namespace EdgeLibrary
{
    public class LabelButton : Button
    {
        public SpriteFont Font;
        public string Text;

        public virtual event ButtonEventHandler Click;
        public virtual event ButtonEventHandler MouseOver;
        public virtual event ButtonEventHandler MouseOff;

        public LabelButton(string id, string eFontName, string eText, Vector2 ePosition, Color eClickColor) : base(id, "", ePosition, eClickColor)
        {
            Font = ResourceManager.getFont(eFontName);
            Text = eText;
            reloadBoundingBox();
        }

        public override void  reloadBoundingBox()
        { 
            if (Font != null)
            {
                Vector2 Measured = Font.MeasureString(Text);
                BoundingBox = new Rectangle((int)(Position.X - Measured.X / 2), (int)(Position.Y - Measured.Y / 2), (int)Measured.X, (int)Measured.Y);
                _width = BoundingBox.Width;
                _height = BoundingBox.Height;
            }
        }

        protected override void  drawElement(GameTime gameTime)
        {
            EdgeGame.drawString(Font, Text, new Vector2(Position.X-Font.MeasureString(Text).X/2, Position.Y-Font.MeasureString(Text).Y / 2), Style.Color, Style.Rotation, OriginPoint, Scale, Style.Effects);
        }

        protected override void updateElement(GameTime gameTime)
        {
            base.UpdateSpritePortion(gameTime);

            Vector2 mousePosition = InputManager.MousePos();

            if (BoundingBox.Contains(new Rectangle((int)mousePosition.X, (int)mousePosition.Y, 1, 1)))
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
