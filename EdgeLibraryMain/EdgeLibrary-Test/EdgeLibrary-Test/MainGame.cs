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
    /// - MoikMellah at OpenGameArt.org
    /// - http://millionthvector.blogspot.de/
    /// </summary>
    /// 

    public class MainGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        EdgeGame edgeGame;

        ELabel label;
        ESpriteA player;
        int collisionCount;

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
            edgeGame = new EdgeGame(Content, spriteBatch, graphics, GraphicsDevice);
            edgeGame.Init();

            initializeGame();

            base.Initialize();
        }
        protected override void LoadContent() { edgeGame.LoadContent(); }
        protected override void UnloadContent() { edgeGame.UnloadContent(); }
        //Updates the game
        protected override void Update(GameTime gameTime)
        {
            edgeGame.Update(gameTime);
            base.Update(gameTime);
        }
        //Draws the game
        protected override void Draw(GameTime gameTime)
        {
            edgeGame.Draw(gameTime);
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
            edgeGame.LoadSong("battleThemeA", "battleSong");

            edgeGame.LoadFont("font", "font");

            edgeGame.LoadTexture("Particle Textures/fire", "fire");

            edgeGame.LoadTexture("Alien/AlienBombSheet", "alienbomb");
            edgeGame.LoadTexture("Alien/AlienOrbSheet", "alienorb");
            edgeGame.LoadTexture("Alien/ShipSheet", "ship");
        }

        //Sets up the game window
        private void initializeGameWindow()
        {
            edgeGame.DrawType = EdgeGameDrawTypes.Hybrid;
            //edgeGame.playSong("battleSong");
            edgeGame.setWindowHeight(700);
            edgeGame.setWindowWidth(700);
            IsMouseVisible = true;
        }

        //A sample scene
        private void initializeMenuScene()
        {
            EScene menuScene = new EScene("menuScene");
            edgeGame.addScene(menuScene);

            edgeGame.UpdateEvent += new EdgeGame.EdgeGameUpdateEvent(EdgeGameUpdate);

            ERAnimationIndex alienBomb = new ERAnimationIndex(50, "alienorb", "AlienOrbSheetData.xml");
            alienBomb.ShouldRepeat = true;
            player = new ESpriteA(alienBomb, "alien", new Vector2(100, 100));
            player.DrawType = ESpriteDrawType.Scaled;
            player.ScaledDrawScale = 1;
            menuScene.addElement(player);


            EParticleEmitter mouseEmitter = new EParticleEmitter("fire", new Vector2(400, 400));
            mouseEmitter.ShouldEmit = true;
            mouseEmitter.DrawLayer = 3;
            mouseEmitter.EmitPositionVariance = new ERangeArray(new ERange(0), new ERange(0));
            mouseEmitter.ColorVariance = new ERangeArray(new ERange(60, 80), new ERange(30, 40), new ERange(0), new ERange(255));
            mouseEmitter.VelocityVariance = new ERangeArray(ERange.RangeWithDiffer(0, 4), ERange.RangeWithDiffer(0, 4));
            mouseEmitter.SizeVariance = new ERangeArray(ERange.RangeWithDiffer(100, 25), ERange.RangeWithDiffer(100, 25));
            mouseEmitter.GrowSpeed = 1f;
            mouseEmitter.StartRotationVariance = ERange.RangeWithDiffer(0, 0);
            mouseEmitter.RotationSpeedVariance = ERange.RangeWithDiffer(0, 0);
            mouseEmitter.LifeVariance = new ERange(500);
            mouseEmitter.EmitWait = 0;
            mouseEmitter.ClampToMouse();
            menuScene.addElement(mouseEmitter);

        }

        private void initializeGameScene()
        {
            EScene gameScene = new EScene("gameScene");

            edgeGame.addScene(gameScene);
        }

        private void SpriteCollisionStart(ESpriteCollisionArgs e)
        {
            collisionCount++;
            label.Text = string.Format("Collision Count: {0}", collisionCount);
        }

        private void EdgeGameUpdate(EUpdateArgs e)
        {
        }
    }
}
