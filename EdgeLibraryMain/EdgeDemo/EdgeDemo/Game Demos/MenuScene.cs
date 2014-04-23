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
using EdgeLibrary;

namespace EdgeDemo
{
    public class SwitchEventArgs : EventArgs
    {
        //An enum could be used here, but it doesn't really matter.......
        //0 - Menu
        //1 - Space Game
        public int SwitchCode;

        public SwitchEventArgs(int value)
        {
            SwitchCode = value;
        }
    }

    public class MenuScene : Scene
    {
        public event EventHandler<SwitchEventArgs> OnSwitch = delegate { };

        public MenuScene() : base("MenuScene")
        {
            AddAndSwitch();

            Sprite OptionsButton = new Sprite("ShadedDark33", new Vector2(40));
            OptionsButton.OnMouseOver += new Sprite.ButtonEvent(OptionsButton_OnMouseOver);
            OptionsButton.OnClick += new Sprite.ButtonEvent(OptionsButton_OnClick);
            OptionsButton.AddToGame();

            Sprite SpaceButton = new Sprite("ShadedDark12", EdgeGame.WindowSize / 2);
            SpaceButton.OnMouseOver += new Sprite.ButtonEvent(SpaceButton_OnMouseOver);
            SpaceButton.OnClick += new Sprite.ButtonEvent(SpaceButton_OnClick);
            SpaceButton.AddToGame();
        }

        void SpaceButton_OnClick(Sprite sender, Vector2 mousePosition, GameTime gameTime) { OnSwitch(this, new SwitchEventArgs(1)); }

        void OptionsButton_OnClick(Sprite sender, Vector2 mousePosition, GameTime gameTime) { }

        void SpaceButton_OnMouseOver(Sprite sender, Vector2 mousePosition, GameTime gameTime) { }

        void OptionsButton_OnMouseOver(Sprite sender, Vector2 mousePosition, GameTime gameTime) { }
    }
}
