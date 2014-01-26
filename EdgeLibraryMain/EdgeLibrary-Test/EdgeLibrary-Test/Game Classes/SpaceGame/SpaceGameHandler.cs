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
using EdgeLibrary.Basic;
using EdgeLibrary.Menu;
using EdgeLibrary.Effects;

namespace EdgeLibrary_Test
{
    public class SpaceGameHandler : EElement
    {
        ELayer layer;

        Random random;
        ERange asteroidSpeedRange;
        ERange asteroidSizeRange;

        PlayerShip player;
        ERandomTicker ticker;

        public override void FillTexture()
        {
            InitSpaceGame();
        }

        public void InitSpaceGame()
        {
            layer = EdgeGame.GetLayerFromObject(this);

            random = new Random();
            asteroidSpeedRange = new ERange(7,8);
            asteroidSizeRange = new ERange(50, 150);

            ticker = new ERandomTicker(new ERange(1000, 3000));
            ticker.Tick += new ERandomTicker.ETickerEventHandler(asteroidCreate);
            layer.addElement(ticker);

            player = new PlayerShip();
            layer.addElement(player);
        }

        void asteroidCreate(ETickerEventArgs e)
        {
            Asteroid asteroid = new Asteroid((int)asteroidSpeedRange.GetRandom(random), (int)asteroidSizeRange.GetRandom(random), (int)random.Next((int)EdgeGame.WindowSize.X));
            layer.addElement(asteroid);
        }

        public override void updateElement(EUpdateArgs updateArgs)
        {
        }
    }
}
