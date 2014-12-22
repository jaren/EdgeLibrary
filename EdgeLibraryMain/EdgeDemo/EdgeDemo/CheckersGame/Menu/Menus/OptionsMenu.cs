using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    public class OptionsMenu : MenuBase
    {
        public OptionsMenu() : base("OptionsMenu")
        {
            TextSprite title = new TextSprite(Config.MenuTitleFont, "Options Menu", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.05f)) { Color = Config.MenuTextColor };
            Components.Add(title);

            TextSprite subTitle = new TextSprite(Config.MenuSubtitleFont, "Click!", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.1f)) { Color = Config.MenuTextColor };
            Components.Add(subTitle);

            Button quitButton = new Button("grey_button00", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.7f)) { ClickTexture = EdgeGame.GetTexture("grey_button01"), MouseOverTexture = EdgeGame.GetTexture("grey_button02"), Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            quitButton.SetColors(Config.MenuButtonColor);
            quitButton.OnRelease += (x, y) => { if (System.Windows.Forms.MessageBox.Show("Are you sure you want to leave this game?", "Leave?", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK) { Config.ThisGameType = Config.GameType.Singleplayer; MenuManager.SwitchMenu("MainMenu"); } };
            Components.Add(quitButton);

            TextSprite quitButtonText = new TextSprite(Config.MenuButtonTextFont, "Main Menu", quitButton.Position);
            Components.Add(quitButtonText);

            ButtonToggle musicButton = new ButtonToggle("grey_boxCheckmark", new Microsoft.Xna.Framework.Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y * 0.5f)) { OnClickTexture = EdgeGame.GetTexture("grey_boxCheckmark"), OnMouseOverTexture = EdgeGame.GetTexture("grey_boxCheckmark"), OffNormalTexture = EdgeGame.GetTexture("grey_box"), OffClickTexture = EdgeGame.GetTexture("grey_box"), OffMouseOverTexture = EdgeGame.GetTexture("grey_box"), Color = Config.MenuButtonColor, Scale = new Vector2(1) };
            musicButton.SetColors(Config.MenuButtonColor);
            musicButton.On = true;
            Components.Add(musicButton);

            Input.OnKeyRelease += Input_OnKeyRelease;
        }

        void Input_OnKeyRelease(Microsoft.Xna.Framework.Input.Keys key)
        {
            if (MenuManager.SelectedMenu == this && key == Config.BackKey)
            {
                MenuManager.SwitchMenu(MenuManager.PreviousMenu.Name);
            }
        }
    }
}
