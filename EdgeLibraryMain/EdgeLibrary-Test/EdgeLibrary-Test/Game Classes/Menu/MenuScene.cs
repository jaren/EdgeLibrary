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
    public class MenuScene : EScene
    {
        ELayer playLayer;
        ELayer settingsLayer;
        ELayer creditsLayer;
        ELayer backdropLayer;

        EButton playButton;
        EButton settingsButton;
        EButton creditsButton;

        ESprite movingSmallBackdrop;

        public MenuScene() : base("MenuScene")
        {
            playLayer = new ELayer("play");
            AddLayer(playLayer);

            EButton playButton1 = new EButton("shadedDark25", new Vector2(225, 200), 50, 50, Color.White);
            playButton1.Click += new EButton.ButtonEventHandler(playButton1_Click);
            playButton1.setClickTexture("transparentDark23");
            playLayer.addElement(playButton1);

            ELabel playLabel1 = new ELabel("mediumFont", new Vector2(250, 175), "Play 'Space Game'", Color.White);
            playLayer.addElement(playLabel1);

            EButton playButton2 = new EButton("shadedDark25", new Vector2(225, 300), 50, 50, Color.White);
            playButton2.setClickTexture("transparentDark23");
            playLayer.addElement(playButton2);

            ELabel playLabel2 = new ELabel("mediumFont", new Vector2(250, 275), "Play '? Game'", Color.White);
            playLayer.addElement(playLabel2);

            EButton playButton3 = new EButton("shadedDark25", new Vector2(225, 400), 50, 50, Color.White);
            playButton3.setClickTexture("transparentDark23");
            playLayer.addElement(playButton3);

            ELabel playLabel3 = new ELabel("mediumFont", new Vector2(250, 375), "Play '? Game'", Color.White);
            playLayer.addElement(playLabel3);

            settingsLayer = new ELayer("settings");
            AddLayer(settingsLayer);

            ELabel settings1 = new ELabel("largeFont", new Vector2(150, 300), "NOTHING HERE", Color.Red);
            settingsLayer.addElement(settings1);

            creditsLayer = new ELayer("credits");
            AddLayer(creditsLayer);

            ELabel credits1 = new ELabel("mediumFont", new Vector2(300, 100), "Credits:", Color.White);
            creditsLayer.addElement(credits1);
            credits1.CenterX();

            ELabel credits2 = new ELabel("smallFont", new Vector2(150, 200), "- Made by ElevatedEdge @ Github.com", Color.White);
            creditsLayer.addElement(credits2);

            ELabel credits3 = new ELabel("smallFont", new Vector2(150, 300), "- With help from glen3b @ Github.com", Color.White);
            creditsLayer.addElement(credits3);

            ELabel credits4 = new ELabel("smallFont", new Vector2(150, 400), "- Music/Textures from OpenGameArt.org", Color.White);
            creditsLayer.addElement(credits4);

            backdropLayer = new ELayer("backdrop");
            backdropLayer.DrawLayer = 5;
            AddLayer(backdropLayer);

            ELabel backdropMenu = new ELabel("largeFont", Vector2.Zero, "Menu", Color.White);
            backdropLayer.addElement(backdropMenu);
            backdropMenu.CenterX();

            int buttonX = 100;
            int buttonStep = 150;
            int buttonYStart = 200;
            int buttonSize = 50;
            playButton = new EButton("shadedDark16", new Vector2(buttonX, buttonYStart), buttonSize, buttonSize, Color.White);
            playButton.Click += new EButton.ButtonEventHandler(playButton_Click);
            backdropLayer.addElement(playButton);
            settingsButton = new EButton("shadedDark22", new Vector2(buttonX, buttonYStart + buttonStep), buttonSize, buttonSize, Color.White);
            settingsButton.Click += new EButton.ButtonEventHandler(settingsButton_Click);
            backdropLayer.addElement(settingsButton);
            creditsButton = new EButton("shadedDark32", new Vector2(buttonX, buttonYStart + buttonStep * 2), buttonSize, buttonSize, Color.White);
            creditsButton.Click += new EButton.ButtonEventHandler(creditsButton_Click);
            backdropLayer.addElement(creditsButton);

            ESprite backdrop = new ESprite("backdrop", new Vector2(buttonX + buttonSize/2 + 275, buttonYStart - buttonSize * 2 + 250), 500, 500);
            backdropLayer.addElement(backdrop);

             movingSmallBackdrop = new ESprite("backdrop", playButton.Position, 125, (int)(buttonSize * 1.5f));
            movingSmallBackdrop.DrawLayer = -1;
            backdropLayer.addElement(movingSmallBackdrop);

            showLayer(playLayer);

        }

        void playButton1_Click(ButtonEventArgs e)
        {
            EdgeGame.switchScene("SpaceGame");
        }

        void creditsButton_Click(ButtonEventArgs e)
        {
            showLayer(creditsLayer);
            movingSmallBackdrop.runAction(new EActionMove(creditsButton.Position, 5));
        }

        void settingsButton_Click(ButtonEventArgs e)
        {
            showLayer(settingsLayer);
            movingSmallBackdrop.runAction(new EActionMove(settingsButton.Position, 5));
        }

        void playButton_Click(ButtonEventArgs e)
        {
            showLayer(playLayer);
            movingSmallBackdrop.runAction(new EActionMove(playButton.Position, 5));
        }

        private void showLayer(ELayer layer)
        {
            foreach(ELayer tempLayer in layers)
            {
                if (tempLayer.ID != backdropLayer.ID)
                {
                    layer.DrawLayer = backdropLayer.DrawLayer - 1;
                }
            }

            layer.DrawLayer = backdropLayer.DrawLayer + 1;
        }
    }
}
