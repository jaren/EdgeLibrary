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
using EdgeLibrary.Edge;

namespace EdgeLibrary
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        /*
         * TODO:
         * -General
         *   -Delete function in objects?
         * -Actions
         *   -Fix EActionSequence... find out where the list is adding more than one action
         * -Menu
         *   -More menu items
         *   -Labels to buttons
         */

        //Images from OpenGameArt.org

        EdgeGame edgeGame;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random random;
        EParticleEmitter mouseEmitter;
        EParticleEmitter spiralEmitter;
        EButton button;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            edgeGame = new EdgeGame(Content, spriteBatch, graphics, GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            edgeGame.LoadContent();
      
            edgeGame.LoadTexture("Used Files/Player_All/Player/player_down_1", "image");
            edgeGame.LoadTexture("Used Files/Sprites/Dotty", "image2");
            edgeGame.LoadTexture("Particle Textures/fire", "particle");
            edgeGame.LoadTexture("Particle Textures/smoke", "particle2");
            edgeGame.LoadTexture("Particle Textures/snow", "particle3");
            edgeGame.LoadTexture("Particle Textures/stars2", "particle4");
            edgeGame.LoadFont("font", "font1");

            edgeGame.LoadSong("RPG Shop", "shop");
            edgeGame.LoadSound("button-3", "button");

            InitializeEdge();
        }

        protected void InitializeEdge()
        {
            edgeGame.Init();

            this.IsMouseVisible = true;
            initMenuScene();

            edgeGame.setWindowHeight(1000);
            edgeGame.setWindowWidth(1000);

            edgeGame.MouseMove += new EdgeGame.EMouseEvent(mouseMove);
            edgeGame.MouseClick += new EdgeGame.EMouseEvent(mouseClick);
        }

        protected void initMenuScene()
        {
            EScene menuScene = new EScene("menu");
            edgeGame.addScene(menuScene);

            random = new Random();

            EActionMove move = new EActionMove(new Vector2(100,100), 10f);
            EActionMove move1 = new EActionMove(new Vector2(900, 100), 10f);
            EActionMove move2 = new EActionMove(new Vector2(900, 900), 10f);
            EActionMove move3 = new EActionMove(new Vector2(100, 900), 10f);
            EActionSequence sequence = new EActionSequence(move, move1, move2, move3);


            EActionMove move4 = new EActionMove(new Vector2(100, 100), 10f);
            EActionMove move5 = new EActionMove(new Vector2(900, 100), 10f);
            EActionMove move6 = new EActionMove(new Vector2(900, 900), 10f);
            EActionMove move7 = new EActionMove(new Vector2(100, 900), 10f);            
            EActionSequence sequence1 = new EActionSequence(move4, move5, move6, move7);
            
            
            EActionRepeatForever repeatForever = new EActionRepeatForever(sequence);

            EActionRotate spin = new EActionRotate(1, 1);
            //EActionRepeatForever spinForever = new EActionRepeatForever(spin);

            ELabel label = new ELabel("font1", new Vector2(500, 500), "HI", Color.Gold);
            label.runAction(spin);
            menuScene.addElement(label);

            button = new EButton("image", new Vector2(100,100), 100, 100, Color.DarkGray);
            button.IsActive = true;
            button.DrawLayer = 2;
            button.runAction(sequence);
            menuScene.addElement(button);

            ESprite sprite = new ESprite("image", new Vector2(200, 200), 50, 50);
            sprite.DrawLayer = 5;
            sprite.runAction(sequence1);
            menuScene.addElement(sprite);


            button.Click += new EButton.ButtonEventHandler(buttonClicked);
            ETicker particleTicker = new ETicker(5000, new ERange(0,1));
            particleTicker.Tick += new ETicker.ETickerEventHandler(particleTickerTick);
            menuScene.addElement(particleTicker);

            #region PARTICLES
            EParticleEmitter dotsEmitter = new EParticleEmitter("particle3", new Vector2(500, 0));
            dotsEmitter.ShouldEmit = true;
            dotsEmitter.DrawLayer = 1;
            dotsEmitter.EmitPositionVariance = new ERangeArray(ERange.RangeWithDiffer(0, 900), ERange.RangeWithDiffer(0, 0));
            dotsEmitter.ColorVariance = new ERangeArray(new ERange(100), new ERange(100), new ERange(100), new ERange(255));
            dotsEmitter.VelocityVariance = new ERangeArray(ERange.RangeWithDiffer(0,0.1f), ERange.RangeWithDiffer(8,2.5f));
            dotsEmitter.SizeVariance = new ERangeArray(new ERange(15), new ERange(15));
            dotsEmitter.GrowSpeed = 0f;
            dotsEmitter.StartRotationVariance = ERange.RangeWithDiffer(0, 0);
            dotsEmitter.RotationSpeedVariance = ERange.RangeWithDiffer(0, 0);
            dotsEmitter.LifeVariance = new ERange(5000);
            dotsEmitter.EmitWait = 0;
            menuScene.addElement(dotsEmitter);

            mouseEmitter = new EParticleEmitter("particle", new Vector2(400, 400));
            mouseEmitter.ShouldEmit = true;
            mouseEmitter.DrawLayer = 3;
            mouseEmitter.EmitPositionVariance = new ERangeArray(new ERange(0), new ERange(0));
            mouseEmitter.ColorVariance = new ERangeArray(new ERange(0), new ERange(40, 80), new ERange(40, 80), new ERange(255));
            mouseEmitter.VelocityVariance = new ERangeArray(ERange.RangeWithDiffer(0,4), ERange.RangeWithDiffer(0,4));
            mouseEmitter.SizeVariance = new ERangeArray(ERange.RangeWithDiffer(100,25), ERange.RangeWithDiffer(100,25));
            mouseEmitter.GrowSpeed = 1f;
            mouseEmitter.StartRotationVariance = ERange.RangeWithDiffer(0, 0);
            mouseEmitter.RotationSpeedVariance = ERange.RangeWithDiffer(0, 0);
            mouseEmitter.LifeVariance = new ERange(1000);
            mouseEmitter.EmitWait = 0;
            menuScene.addElement(mouseEmitter);
            mouseEmitter.ClampToMouse();

            spiralEmitter = new EParticleEmitter("particle4", new Vector2(400, 400));
            spiralEmitter.ShouldEmit = false;
            spiralEmitter.DrawLayer = 5;
            spiralEmitter.EmitPositionVariance = new ERangeArray(ERange.RangeWithDiffer(0,1000), ERange.RangeWithDiffer(0,1000));
            spiralEmitter.ColorVariance = new ERangeArray(new ERange(200), new ERange(200), new ERange(200), new ERange(255));
            spiralEmitter.VelocityVariance = new ERangeArray(ERange.RangeWithDiffer(0,5), ERange.RangeWithDiffer(0,5));
            spiralEmitter.SizeVariance = new ERangeArray(new ERange(50, 60), new ERange(50, 60));
            spiralEmitter.GrowSpeed = -0.5f;
            spiralEmitter.StartRotationVariance = ERange.RangeWithDiffer(0, 0);
            spiralEmitter.RotationSpeedVariance = ERange.RangeWithDiffer(0,500);
            spiralEmitter.LifeVariance = new ERange(2000);
            spiralEmitter.EmitWait = 0;
            menuScene.addElement(spiralEmitter);
            spiralEmitter.clampTo(button);
            #endregion

            edgeGame.playSong("shop");
        }

        protected void particleTickerTick(ETickerEventArgs e)
        {
            //Put stuff here
        }

        protected void mouseClick(EUpdateArgs e)
        {
        }

        protected void mouseMove(EUpdateArgs e)
        {
            if (e.mouseState.X < graphics.PreferredBackBufferWidth/2)
            {
                if (e.mouseState.Y < graphics.PreferredBackBufferHeight / 2) { mouseEmitter.ColorVariance = new ERangeArray(new ERange(40, 100), new ERange(20, 40), new ERange(1, 10), new ERange(255)); }
                else { mouseEmitter.ColorVariance = new ERangeArray(new ERange(0), new ERange(40, 80), new ERange(40, 80), new ERange(255)); }
            }
            else
            {
                if (e.mouseState.Y < graphics.PreferredBackBufferHeight / 2) { mouseEmitter.ColorVariance = new ERangeArray(new ERange(0), new ERange(40, 80), new ERange(40, 80), new ERange(255)); }
                else { mouseEmitter.ColorVariance = new ERangeArray(new ERange(40, 100), new ERange(20, 40), new ERange(1, 10), new ERange(255)); }
            }
        }

        protected void actionActivate(EParticleEmitter sender)
        {
            edgeGame.playSound("button");
        }

        protected void buttonClicked(ButtonEventArgs e)
        {
            spiralEmitter.EmitSingleParticle();
        }

        protected override void UnloadContent()
        {
            edgeGame.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState k = Keyboard.GetState();
            MouseState m = Mouse.GetState();

            edgeGame.Update(new EUpdateArgs(gameTime, k, m));

            if (k.IsKeyDown(Keys.K))
            {
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            edgeGame.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
