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
    /// <summary>
    /// TODO:
    /// -Actions
    ///     -Fix "EActionSequence"
    ///     -Fix "EActionRotate"?
    /// -Menu
    ///     -More Menu Items
    ///         -Label button
    /// </summary>
    /// 

    /// <summary>
    /// MUSIC AND TEXTURES:
    /// - cynicmusic.com/pixelsphere.org
    /// - MoikMellah, axtoncrolley at OpenGameArt.org
    /// - Kenney at kenney.nl
    /// - http://millionthvector.blogspot.de/
    /// </summary>
    /// 

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
        //Initializes the game
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            EdgeGame.Init(Content, spriteBatch, graphics, GraphicsDevice);

            initializeGame();

            base.Initialize();
        }
        protected override void LoadContent() { EdgeGame.LoadContent(); }
        protected override void UnloadContent() { EdgeGame.UnloadContent(); }
        //Updates the game
        protected override void Update(GameTime gameTime)
        {
            EdgeGame.Update(gameTime);
            base.Update(gameTime);
        }
        //Draws the game
        protected override void Draw(GameTime gameTime)
        {
            EdgeGame.Draw(gameTime);
            base.Draw(gameTime);
        }
        #endregion

        //This calls all the loading functions
        private void initializeGame()
        {
            loadResources();
            initializeGameWindow();
            initializeMenuScene();
            initializeGameScene();
        }

        //This is the "load content" function
        private void loadResources()
        {
            EdgeGame.LoadSong("battleThemeA", "battleSong");

            EdgeGame.LoadFont("font", "font");

            EdgeGame.LoadTexture("Particle Textures/fire", "fire");

            EdgeGame.LoadTexture("SpaceSheet", "sheet");
        }

        //Sets up the game window
        private void initializeGameWindow()
        {
            EdgeGame.DrawType = EdgeGameDrawTypes.Hybrid;
            //EdgeGame.playSong("battleSong");
            EdgeGame.setWindowHeight(700);
            EdgeGame.setWindowWidth(700);
            IsMouseVisible = true;
        }

        //A sample scene
        private void initializeMenuScene()
        {
            EScene menuScene = new EScene("menuScene");
            ELayer mainLayer = new ELayer("main");
            EdgeGame.addScene(menuScene);
            menuScene.AddLayer(mainLayer);

            EdgeGame.UpdateEvent += new EdgeGame.EdgeGameUpdateEvent(EdgeGameUpdate);

            EdgeGame.LoadTextureFromSpritesheet("sheet", "SpaceSheet.xml");
        }

        private void initializeGameScene()
        {
            EScene gameScene = new EScene("gameScene");

            EdgeGame.addScene(gameScene);
            EdgeGame.playSong("battleSong");
        }

        private void EdgeGameUpdate(EUpdateArgs e)
        {
        }
    }
}
