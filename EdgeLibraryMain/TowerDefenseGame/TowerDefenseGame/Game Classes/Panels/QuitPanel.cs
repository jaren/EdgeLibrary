using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public class QuitPanel : Scene
    {
        public Sprite BackPanel;
        public TextSprite RewardText;
        public TextSprite ContinueText;
        public Button ContinueButton;
        public TextSprite QuitText;
        public Button QuitButton;
        public QuitPanel()
        {
            BackPanel = new Sprite("grey_panel", new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.5f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * 0.6f)) { Color = new Color(20, 20, 20, 225), Scale = new Vector2(5f, 2.5f) };
            base.Components.Add(BackPanel);

            RewardText = new TextSprite(Config.StatusFont, "Congratulations! You won the game!", EdgeGame.WindowSize * Config.CommonRatio / 2) { Color = Color.DarkGoldenrod };
            base.Components.Add(RewardText);

            ContinueButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.35f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * 0.65f)) { Color = Config.MenuButtonColor };
            ContinueButton.Style = new Style(EdgeGame.GetTexture(Config.ButtonMouseOverTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonNormalTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonClickTexture), Config.MenuButtonColor);
            base.Components.Add(ContinueButton);

            ContinueText = new TextSprite(Config.MenuButtonTextFont, "Continue", ContinueButton.Position);
            base.Components.Add(ContinueText);

            QuitButton = new Button(Config.ButtonNormalTexture, new Vector2(EdgeGame.WindowSize.X * Config.CommonRatio.X * 0.65f, EdgeGame.WindowSize.Y * Config.CommonRatio.Y * 0.65f)) { Color = Config.MenuButtonColor };
            QuitButton.Style = new Style(EdgeGame.GetTexture(Config.ButtonMouseOverTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonNormalTexture), Config.MenuButtonColor, EdgeGame.GetTexture(Config.ButtonClickTexture), Config.MenuButtonColor);
            base.Components.Add(QuitButton);
            
            QuitText = new TextSprite(Config.MenuButtonTextFont, "Quit", QuitButton.Position);
            base.Components.Add(QuitText);
        }
    }
}
