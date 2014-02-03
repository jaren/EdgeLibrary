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
    /// <summary>
    /// MUSIC AND TEXTURES:
    /// - cynicmusic.com/pixelsphere.org
    /// - axtoncrolley at OpenGameArt.org
    /// - Kenney at kenney.nl
    /// </summary>
    /// 

    /// <summary>
    /// NOTES:
    /// -All XML Spritesheets must be set to "Content" and "Copy if Newer"
    /// </summary>

    public class MainGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //This region is not likely to be modified
        #region NOT-USED
        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            EdgeGame.Init(Content, spriteBatch, graphics, GraphicsDevice);

            initializeGame();

            base.Initialize();
        }
        protected override void LoadContent() { EdgeGame.LoadContent(); }
        protected override void UnloadContent() { EdgeGame.UnloadContent(); }
        protected override void Update(GameTime gameTime)
        {
            EdgeGame.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            EdgeGame.Draw(gameTime);
            base.Draw(gameTime);
        }
        #endregion

        //Calls all the initialize functions
        private void initializeGame()
        {
            loadResources();
            initializeGameWindow();
            initializeScenes();
        }

        //This is the "load content" function
        private void loadResources()
        {
            EdgeGame.LoadSong("battleThemeA");
            EdgeGame.LoadSong("spaceBossMusic");

            EdgeGame.LoadFont("SpriteFonts/smallFont");
            EdgeGame.LoadFont("SpriteFonts/mediumFont");
            EdgeGame.LoadFont("SpriteFonts/largeFont");
            MediaPlayer.IsRepeating = true;

            EdgeGame.LoadTexture(TextureTools.CreateVerticalGradient(300, 300, Color.White, Color.Black), "gradient");

            EdgeGame.LoadTextureFromSpritesheet("ButtonSheet", "ButtonSheet.xml");

            EdgeGame.LoadTexture("Particle Textures/fire");
        }

        //Sets up the game window
        private void initializeGameWindow()
        {
            EdgeGame.DrawType = EdgeGameDrawTypes.Normal;
            EdgeGame.ClearColor = Color.Gray;
            EdgeGame.playSong("spaceBossMusic");
            EdgeGame.setWindowHeight(700);
            EdgeGame.setWindowWidth(700);
            IsMouseVisible = true;

            //Asteroid collision bodies EXTREMELY laggy
            MathTools.circlePointStep = 50;
            MathTools.outerCirclePointStep = 50;
        }

        private void initializeScenes()
        {
            Scene mainScene = new Scene("main");
            EdgeGame.addScene(mainScene);
            Sprite sprite = new Sprite("gradient", new Vector2(500, 500));
            mainScene.addElement(sprite);
        }
    }
}