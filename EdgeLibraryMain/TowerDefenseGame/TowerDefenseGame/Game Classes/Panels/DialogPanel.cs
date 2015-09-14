using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseGame
{
    public class DialogPanel : Scene
    {
        public Sprite BackPanel;
        public TextSprite DialogText;
        public TextSprite LeftText;
        public Button LeftButton;
        public TextSprite RightText;
        public Button RightButton;
        public DialogPanel(string message, string leftButtonText, string rightButtonText, System.Action leftAction, System.Action rightAction)
        {
            DialogText = new TextSprite(Config.StatusFont, message, new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.45f)) { Color = Color.DarkGoldenrod };

            BackPanel = new Sprite("Pixel", new Vector2(EdgeGame.WindowSize.X * 0.5f, EdgeGame.WindowSize.Y * 0.5f)) { Color = new Color(40,40,40), Scale = new Vector2(DialogText.Font.MeasureString(message).X * 1.1f / EdgeGame.GetTexture("Pixel").Width, 200) };
            base.Components.Add(BackPanel);
            base.Components.Add(DialogText);

            LeftButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.5f - BackPanel.Width * BackPanel.Scale.X * 0.2f, EdgeGame.WindowSize.Y * 0.55f)) { Color = Config.MenuButtonColor };
            LeftButton.Style = new Style(EdgeGame.GetTexture(Config.ButtonMouseOverTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonNormalTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonClickTexture), Config.MenuButtonColor);
            LeftButton.OnRelease += (x, y) => { Hide(); leftAction(); };
            base.Components.Add(LeftButton);

            LeftText = new TextSprite(Config.MenuButtonTextFont, leftButtonText, LeftButton.Position);
            base.Components.Add(LeftText);

            RightButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * 0.5f + BackPanel.Width * BackPanel.Scale.X * 0.2f, EdgeGame.WindowSize.Y * 0.55f)) { Color = Config.MenuButtonColor };
            RightButton.Style = new Style(EdgeGame.GetTexture(Config.ButtonMouseOverTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonNormalTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonClickTexture), Config.MenuButtonColor);
            RightButton.OnRelease += (x, y) => { Hide(); rightAction(); };
            base.Components.Add(RightButton);

            RightText = new TextSprite(Config.MenuButtonTextFont, rightButtonText, RightButton.Position);
            base.Components.Add(RightText);

            Hide();
        }

        public void Show()
        {
            Visible = true;
            Enabled = true;
        }

        public void Hide()
        {
            Visible = false;
            Enabled = false;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                RestartSpriteBatch(true);

                foreach (DrawableGameComponent component in Components.OfType<DrawableGameComponent>())
                {
                    component.Draw(gameTime);
                }
                base.Draw(gameTime);
                DrawObject(gameTime);

                RestartSpriteBatch(true);
            }
        }
    }
}
