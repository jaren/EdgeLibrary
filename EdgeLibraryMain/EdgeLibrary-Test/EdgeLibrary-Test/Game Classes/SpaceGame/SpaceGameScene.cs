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
    public class SpaceGameScene : EScene
    {
        EScene GameScene;
        EScene endScene;

        Random random;
        ERange asteroidSpeedRange;
        ERange asteroidSizeRange;

        PlayerShip player;
        ERandomTicker ticker;

        public SpaceGameScene() : base("SpaceGame")
        {
            random = new Random();
            asteroidSpeedRange = new ERange(7, 8);
            asteroidSizeRange = new ERange(50, 150);

            GameScene = new EScene("game");
            endScene = new EScene("end");

            ticker = new ERandomTicker(new ERange(1000, 3000));
            ticker.Tick += new ERandomTicker.ETickerEventHandler(asteroidCreate);
            GameScene.addElement(ticker);
            player = new PlayerShip(GameScene);
            GameScene.addElement(player);

            ELabel endLabel1 = new ELabel("largeFont", new Vector2(0, 100), "YOU", Color.Red);
            endScene.addElement(endLabel1);
            endLabel1.CenterX();

            ELabel endLabel2 = new ELabel("largeFont", new Vector2(0, 200), "DIED", Color.Red);
            endScene.addElement(endLabel2);
            endLabel2.CenterX();

            EButton retryButton = new EButton("shadedDark12", new Vector2(100, EdgeGame.WindowSize.Y - 100), 200, 80, Color.White);
            retryButton.setClickTexture("transparentDark10");
            retryButton.label = new ELabel("mediumFont", Vector2.Zero, "RETRY", Color.White);
            retryButton.Click += new EButton.ButtonEventHandler(retryButton_Click);
            endScene.addElement(retryButton);

            EButton menuButton = new EButton("shadedDark12", new Vector2(600, EdgeGame.WindowSize.Y - 100), 200, 80, Color.White);
            menuButton.setClickTexture("transparentDark10");
            menuButton.label = new ELabel("mediumFont", Vector2.Zero, "MENU", Color.White);
            menuButton.Click += new EButton.ButtonEventHandler(menuButton_Click);
            endScene.addElement(menuButton);
        }

        void menuButton_Click(ButtonEventArgs e)
        {
            EdgeGame.switchScene("MenuScene");
        }

        void retryButton_Click(ButtonEventArgs e)
        {
            //Resets GameScene
            GameScene = null;
            GameScene = new EScene("game");
            ticker = new ERandomTicker(new ERange(1000, 3000));
            ticker.Tick += new ERandomTicker.ETickerEventHandler(asteroidCreate);
            GameScene.addElement(ticker);
            player = new PlayerShip(GameScene);
            GameScene.addElement(player);
        }

        void asteroidCreate(ETickerEventArgs e)
        {
            Asteroid asteroid = new Asteroid(GameScene, (int)asteroidSpeedRange.GetRandom(random), (int)asteroidSizeRange.GetRandom(random), (int)random.Next((int)EdgeGame.WindowSize.X));
            GameScene.addElement(asteroid);
        }

        public override void drawElement(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (player.dead)
            {
                endScene.drawElement(spriteBatch, gameTime);
            }
            else
            {
                GameScene.drawElement(spriteBatch, gameTime);
            }
        }

        public override void updateElement(EUpdateArgs updateArgs)
        {
            if (player.dead)
            {
                endScene.updateElement(updateArgs);
            }
            else
            {
                GameScene.updateElement(updateArgs);
            }
        }
    }
}
