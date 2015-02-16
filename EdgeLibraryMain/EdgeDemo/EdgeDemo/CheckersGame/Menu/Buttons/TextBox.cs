using EdgeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    public class TextBox : Button
    {
        public bool Focused;
        public bool TextSpriteBlank;
        public string DefaultText;
        public Color OnTextColor;
        public Color OffTextColor;
        public TextSprite TextSprite;
        public Vector2 TextOffset
        {
            get { return textOffset; }
            set { textOffset = value; moveTextSprite(); }
        }

        private Vector2 textOffset;
        public bool ReplaceExtra;

        public TextBox(string texture, string font, Vector2 position)
            : base(texture, position)
        {
            Input.OnClick += Input_OnClick;
            Input.OnKeyRelease += Input_OnKeyRelease;

            textOffset = Vector2.One * 10;
            ReplaceExtra = true;
            TextSpriteBlank = true;
            Focused = false;

            DefaultText = "Enter Text";

            TextSprite = new TextSprite(font, DefaultText, position - new Vector2(BoundingBox.X, BoundingBox.Y) / 2 + TextOffset) { CenterAsOrigin = false };
        }

        private void moveTextSprite()
        {
            TextSprite.Position = Position - new Vector2(BoundingBox.X, BoundingBox.Y) / 2 + TextOffset;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            TextSprite.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            TextSprite.Draw(gameTime);
        }

        void Input_OnClick(Vector2 mousePosition, Vector2 previousMousePosition)
        {
            if (BoundingBox.Contains(new Point((int)Input.MousePosition.X, (int)Input.MousePosition.Y)))
            {
                Focused = true;
                TextSprite.Color = OnTextColor;
                if (TextSpriteBlank)
                {
                    TextSprite.Text = "";
                }
            }
            else
            {
                Focused = false;
                TextSprite.Color = OffTextColor;
                if (TextSpriteBlank)
                {
                    TextSprite.Text = DefaultText;
                }
            }
        }

        void Input_OnKeyRelease(Keys key)
        {
            TextSprite.Text += key.ToCorrectString((Input.IsKeyDown(Keys.LeftShift) || Input.IsKeyDown(Keys.RightShift)));

            if (key == Keys.Back)
            {
                /*
                if (!TextSpriteBlank)
                {
                    TextSprite.Text = TextSprite.Text.Remove(TextSprite.Text.Length - 1, 1);
                }
                 */

                TextSprite.Text = "";
            }

            if (TextSprite.Text.Length == 0)
            {
                TextSpriteBlank = true;
            }
            else
            {
                TextSpriteBlank = false;
            }
        }
    }
}
