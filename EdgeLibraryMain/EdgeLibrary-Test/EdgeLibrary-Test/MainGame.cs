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
    ///     -For some reason, in the asteroid game, if the player ship is a rectangle and the asteroids circles, collision occurs even when the collisions don't appear to occur
    /// -Menu
    ///     -More Menu Items
    ///         -Label button
    /// </summary>
    /// 

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

            EdgeGame.LoadTextureFromSpritesheet("SpaceSheet", "SpaceSheet.xml");
            EdgeGame.LoadTextureFromSpritesheet("ButtonSheet", "ButtonSheet.xml");

            EdgeGame.LoadTexture("Particle Textures/fire");
        }

        //Sets up the game window
        private void initializeGameWindow()
        {
            EdgeGame.DrawType = EdgeGameDrawTypes.Normal;
            EdgeGame.playSong("battleThemeA");
            EdgeGame.setWindowHeight(700);
            EdgeGame.setWindowWidth(700);
            IsMouseVisible = true;

            //Asteroid collision bodies EXTREMELY laggy
            EMath.circlePointStep = 50;
            EMath.outerCirclePointStep = 50;
        }

        private void initializeScenes()
        {
            MenuScene menuScene = new MenuScene();
            SpaceGameScene spaceGame = new SpaceGameScene();
            EdgeGame.addScene(spaceGame);
        }
    }
}
