using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeLibrary
{
    public class TextBox : Button
    {
        public bool Focused
        {
            get { return focused; }
            set { focused = value; CursorFlashSprite.Visible = value; }
        }
        private bool focused;
        public bool TextBlank;
        public string DefaultText;
        public Keys EnterKey;
        public string Text;

        public double TypingInputDelay;
        public double TypingInputStartDelay;
        private Ticker TypingTicker;
        private Keys CurrentKey;

        public bool ReplaceExtra;
        public string ReplaceExtraString;
        public bool ReplaceExtraFront;

        public Sprite CursorFlashSprite;
        public Vector2 CursorFlashOffset;
        public Vector2 CursorFlashScale
        {
            get { return CursorFlashSprite.Scale; }
            set { CursorFlashSprite.Scale = value; }
        }

        public int CursorFlashDelay
        {
            get { return cursorFlashDelay; }
            set { cursorFlashDelay = value; ReloadCursorFlash(); }
        }
        private int cursorFlashDelay;

        public Color CursorFlashColor
        {
            get { return cursorFlashColor; }
            set { cursorFlashColor = value; ReloadCursorFlash(); }
        }
        private Color cursorFlashColor;

        public Color OnTextColor;
        public Color OffTextColor;
        public TextSprite TextSprite;
        public Vector2 TextOffset
        {
            get { return textOffset; }
            set { textOffset = value; ChangeCentering(); }
        }
        private Vector2 textOffset;

        public bool CenterTextBox
        {
            get { return centerTextBox; }
            set { centerTextBox = value; ChangeCentering(); }
        }
        private bool centerTextBox;

        public TextBox(string texture, string font, Vector2 position)
            : base(texture, position)
        {
            Input.OnClick += Input_OnClick;
            Input.OnKeyRelease += Input_OnKeyRelease;
            Input.OnKeyPress += Input_OnKeyPress;

            OnTextColor = Color.White;
            OffTextColor = Color.LightGray;

            EnterKey = Keys.Enter;

            cursorFlashColor = Color.Black;
            cursorFlashDelay = 300;
            CursorFlashOffset = new Vector2(4, -7);
            CursorFlashSprite = new Sprite("Pixel", Vector2.Zero);
            CursorFlashScale = new Vector2(1, 40);
            CursorFlashSprite.AddAction("CursorFlashing", new ARepeat(new ASequence(new AColorChange(new ColorChangeIndex(cursorFlashDelay, Color.Transparent, cursorFlashColor)), new AColorChange(new ColorChangeIndex(cursorFlashDelay, cursorFlashColor, Color.Transparent)))));

            textOffset = Vector2.One * 10;
            ReplaceExtra = true;
            ReplaceExtraString = "..";
            ReplaceExtraFront = true;

            centerTextBox = true;
            TextBlank = true;
            Focused = false;

            TypingInputDelay = 50;
            TypingInputStartDelay = 600;
            TypingTicker = new Ticker(TypingInputStartDelay);
            TypingTicker.OnTick += TypingTicker_OnTick;
            TypingTicker.Started = false;

            DefaultText = "Enter Text";
            Text = DefaultText;

            reloadBoundingBox();

            TextSprite = new TextSprite(font, Text, position) { Color = OffTextColor };
            ReloadCursorFlashPosition();
            ChangeCentering();
        }

        private void ChangeCentering()
        {
            if (centerTextBox)
            {
                CenterAsOrigin = true;
                TextSprite.CenterAsOrigin = true;
                TextSprite.Position = Position;
            }
            else
            {
                CenterAsOrigin = false;
                TextSprite.CenterAsOrigin = false;
                TextSprite.Position = Position + TextOffset;
            }
        }

        private void ReloadCursorFlash()
        {
            CursorFlashSprite.RemoveAction("CursorFlashing");
            CursorFlashSprite.AddAction("CursorFlashing", new ARepeat(new ASequence(new AColorChange(new ColorChangeIndex(cursorFlashDelay, Color.Transparent, cursorFlashColor)), new AColorChange(new ColorChangeIndex(cursorFlashDelay, cursorFlashColor, Color.Transparent)))));
        }

        private void ReloadCursorFlashPosition()
        {
            //Finds the middle of the right edge on the text sprite
            if (TextSprite.CenterAsOrigin)
            {
                CursorFlashSprite.Position = new Vector2(TextSprite.Position.X + TextSprite.Width / 2, TextSprite.Position.Y) + CursorFlashOffset;
            }
            else
            {
                CursorFlashSprite.Position = new Vector2(TextSprite.Position.X + TextSprite.Width, TextSprite.Position.Y + TextSprite.Height / 2) + CursorFlashOffset;
            }
        }

        public override void UpdateObject(GameTime gameTime)
        {
            base.UpdateObject(gameTime);
            TextSprite.Update(gameTime);
            CursorFlashSprite.Update(gameTime);
            TypingTicker.Update(gameTime);
        }

        public override void DrawObject(GameTime gameTime)
        {
            base.DrawObject(gameTime);
            TextSprite.Draw(gameTime);
            CursorFlashSprite.Draw(gameTime);
        }

        private void TypingTicker_OnTick(GameTime gameTime)
        {
            TypingTicker.MillisecondsWait = TypingInputDelay;

            AddKeyToField(CurrentKey);
        }

        private void AddKeyToField(Keys key)
        {
            if (key != EnterKey)
            {
                Text += key.ToCorrectString((Input.IsKeyDown(Keys.LeftShift) || Input.IsKeyDown(Keys.RightShift)));

                if (key == Keys.Back)
                {
                    if (!TextBlank)
                    {
                        Text = Text.Remove(Text.Length - 1, 1);
                        TextSprite.Text = Text;
                    }
                }

                if (Text.Length == 0)
                {
                    TextBlank = true;
                }
                else
                {
                    TextBlank = false;
                }
            }
            else
            {
                Focused = false;
                TypingTicker.Started = false;
                TypingTicker.MillisecondsWait = TypingInputStartDelay;
                TextSprite.Color = OffTextColor;
                if (TextBlank)
                {
                    Text = DefaultText;
                }
            }

            TextSprite.Text = Text;

            if (ReplaceExtra)
            {
                bool removedText = false;
                while (TextSprite.BoundingBox.Width > BoundingBox.Width)
                {
                    if (ReplaceExtraFront)
                    {
                        TextSprite.Text = TextSprite.Text.Remove(0, 1);
                    }
                    else
                    {
                        TextSprite.Text = TextSprite.Text.Remove(TextSprite.Text.Length - 1, 1);
                    }
                    removedText = true;
                }
                if (removedText)
                {
                    if (ReplaceExtraFront)
                    {
                        TextSprite.Text = TextSprite.Text.Remove(0, 1);
                        TextSprite.Text = ReplaceExtraString + TextSprite.Text;
                    }
                    else
                    {
                        TextSprite.Text = TextSprite.Text.Remove(TextSprite.Text.Length - 1, 1);
                        TextSprite.Text += ReplaceExtraString;
                    }
                }
            }

            ReloadCursorFlashPosition();
        }

        private void Input_OnClick(Vector2 mousePosition, Vector2 previousMousePosition)
        {
            if (BoundingBox.Contains(new Point((int)Input.MousePosition.X, (int)Input.MousePosition.Y)))
            {
                Focused = true;
                TextSprite.Color = OnTextColor;
                if (TextBlank)
                {
                    Text = "";
                    TextSprite.Text = "";
                    CursorFlashSprite.Position = Position;
                }
            }
            else
            {
                Focused = false;
                TextSprite.Color = OffTextColor;
                if (TextBlank)
                {
                    TextSprite.Text = DefaultText;
                }
            }
        }

        private void Input_OnKeyPress(Keys key)
        {
            if (Focused)
            {
                CurrentKey = key;
                TypingTicker.Started = true;
                AddKeyToField(key);
            }
        }

        private void Input_OnKeyRelease(Keys key)
        {
            TypingTicker.Started = false;
            TypingTicker.MillisecondsWait = TypingInputStartDelay;
            TypingTicker.elapsedMilliseconds = 0;
        }
    }
}
