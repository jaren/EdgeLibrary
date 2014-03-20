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

        public LabelButton(string eFontName, string eText, Vector2 ePosition, Color eClickColor) : this(MathTools.RandomID(), eFontName, eText, ePosition, eClickColor) { }

        public LabelButton(string id, string eFontName, string eText, Vector2 ePosition, Color eClickColor) : base(id, "", ePosition, eClickColor)
        {
            Font = ResourceManager.getFont(eFontName);
            Text = eText;
            reloadDimensions();
        }

        public override void reloadDimensions()
        {
            if (Font != null)
            {
                Vector2 Measured = Font.MeasureString(Text);

                _width = Measured.X;
                _height = Measured.Y;

                reloadOriginPoint();
            }
        }

        protected override void reloadOriginPoint()
        {
            if (Font != null)
            {
                Vector2 Measured = Font.MeasureString(Text);
                OriginPoint = new Vector2(Measured.X / 2, Measured.Y / 2);
            }
        }

        protected override void reloadActualScale()
        {
            if (Font != null)
            {
                Vector2 measured = Font.MeasureString(Text);
                actualScale = new Vector2(_width / measured.X, _height / measured.Y);
                actualScale *= Scale;
            }
        }

        protected override void  drawElement(GameTime gameTime)
        {
            EdgeGame.drawString(Font, Text, Position, Style.Color, Style.Rotation, OriginPoint, actualScale, Style.Effects);
        }

        protected override void updateElement(GameTime gameTime)
        {
            base.UpdateSpritePortion(gameTime);

            Vector2 mousePosition = InputManager.MousePosition;

            if (GetBoundingBox().Contains(new Point((int)InputManager.MousePosition.X, (int)InputManager.MousePosition.Y)))
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
