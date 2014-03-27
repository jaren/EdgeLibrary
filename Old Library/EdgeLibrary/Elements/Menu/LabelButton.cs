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
        public SpriteFont Font { get { return _font; } set { _font = value; reloadOriginPoint(); } }
        private SpriteFont _font;
        public string Text { get { return _text; } set { _text = value; reloadOriginPoint(); } }
        protected string _text;

        public override float Width { get { return Font == null ? 0 : Font.MeasureString(Text).X; } protected set { } }
        public override float Height { get { return Font == null ? 0 : Font.MeasureString(Text).Y; } protected set { } }

        public virtual event ButtonEventHandler Click;
        public virtual event ButtonEventHandler MouseOver;
        public virtual event ButtonEventHandler MouseOff;

        public LabelButton(string eFontName, string eText, Vector2 ePosition, Color eClickColor) : this(MathTools.RandomID(typeof(LabelButton)), eFontName, eText, ePosition, eClickColor) { }

        public LabelButton(string id, string eFontName, string eText, Vector2 ePosition, Color eClickColor) : base(id, "", ePosition, eClickColor)
        {
            _font = ResourceManager.getFont(eFontName);
            _text = eText;
            reloadOriginPoint();
        }

        protected override void reloadOriginPoint()
        {
            if (Font != null && Text != null)
            {
                if (_centerAsOrigin)
                {
                    Vector2 Measured = Font.MeasureString(Text);
                    OriginPoint = new Vector2(Measured.X / 2, Measured.Y / 2);
                }
                else
                {
                    OriginPoint = Vector2.Zero;
                }
            }
        }

        protected override void drawElement(GameTime gameTime)
        {
            EdgeGame.drawString(Font, Text, Position, Style.Color, Style.Rotation, OriginPoint, Scale, Style.Effects);
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
                        DebugLogger.LogEvent("Button Mouse Over", "Button: " + ID, "Button Type: " + GetType(), "GameTime: " + gameTime.TotalGameTime.ToString());
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
                        DebugLogger.LogEvent("Button Click", "Button: " + ID, "Button Type: " + GetType(), "GameTime: " + gameTime.TotalGameTime.ToString());
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
                        DebugLogger.LogEvent("Button Mouse Off", "Button: " + ID, "Button Type: " + GetType(), "GameTime: " + gameTime.TotalGameTime.ToString());
                    }
                    launchedMouseOver = false;
                    launchedMouseOff = true;
                }
            }
        }
    }
}
