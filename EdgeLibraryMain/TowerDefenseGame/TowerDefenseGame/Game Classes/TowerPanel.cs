using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class TowerPanel : Sprite
    {
        public TextSprite TowerName;
        public TextSprite TowerDescription;
        public Button CloseButton;
        public Button SellButton;
        public TextSprite SellSprite;
        public Sprite TowerPicture;

        public List<Sprite> Components;

        public TowerPanel() : base("Pixel", EdgeGame.WindowSize/2f)
        {
            Components = new List<Sprite>();
        }

        public override void DrawObject(GameTime gameTime)
        {
            base.DrawObject(gameTime);

            foreach (Sprite component in Components)
            {
                component.Draw(gameTime);
            }
        }

        public override void UpdateObject(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.UpdateObject(gameTime);

            foreach (Sprite component in Components)
            {
                component.Update(gameTime);
            }
        }

        public void ShowWithTower(Tower tower)
        {
            Visible = true;
            Enabled = true;
        }
    }
}
