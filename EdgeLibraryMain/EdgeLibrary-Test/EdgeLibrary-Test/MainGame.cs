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
    /// -General
    ///     -Fix "Remove Element/Object" function
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
    /// - IMakeGames at OpenGameArt.org
    /// </summary>
    /// 

    public class MainGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        EdgeGame edgeGame;

        #region NOT-USED
        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            edgeGame = new EdgeGame(Content, spriteBatch, graphics, GraphicsDevice);
            edgeGame.Init();

            initializeGame();

            base.Initialize();
        }
        protected override void LoadContent()
        {
            edgeGame.LoadContent();
        }
        protected override void UnloadContent() { edgeGame.UnloadContent(); }
        protected override void Update(GameTime gameTime)
        {
            edgeGame.Update(new EUpdateArgs(gameTime, Keyboard.GetState(), Mouse.GetState()));
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            edgeGame.Draw(gameTime);
            base.Draw(gameTime);
        }
        #endregion

        private void initializeGame()
        {
            loadResources();
            initializeGameWindow();
            initializeMenuScene();
            initializeGameScene();
        }

        private void loadResources()
        {
            edgeGame.LoadSong("battleThemeA", "battleSong");

            edgeGame.LoadTexture("Particle Textures/fire", "fire");
            edgeGame.LoadTexture("Particle Textures/stars", "star");
            edgeGame.LoadTexture("Particle Textures/smoke", "smoke");
            edgeGame.LoadTexture("Particle Textures/snow", "snow");

            edgeGame.LoadTexture("Platform/Entities/sprite01", "sprite01");
            edgeGame.LoadTexture("Platform/Entities/sprite02", "sprite02");
        }

        private void initializeGameWindow()
        {
            edgeGame.playSong("battleSong");
            edgeGame.setWindowHeight(1000);
            edgeGame.setWindowWidth(1000);
            IsMouseVisible = true;
        }

        private void initializeMenuScene()
        {
            EScene menuScene = new EScene("menuScene");
            edgeGame.addScene(menuScene);
        }

        private void initializeGameScene()
        {
            EScene gameScene = new EScene("gameScene");
            edgeGame.addScene(gameScene);
        }
    }
}
