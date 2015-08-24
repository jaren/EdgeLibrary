using EdgeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class TowerPanel : Scene
    {
        public TextSprite TowerName;
        public TextSprite TowerDescription;
        public Button CloseButton;
        public Button SellButton;
        public TextSprite SellSprite;
        public Sprite TowerPicture;

        public TowerPanel() : base(new List<Microsoft.Xna.Framework.GameComponent>())
        {
        }
    }
}
