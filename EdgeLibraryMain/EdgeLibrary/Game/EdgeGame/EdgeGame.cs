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
using System.Diagnostics;

namespace EdgeLibrary
{
    //The main game type
    public static partial class EdgeGame
    {
        //The game which is run
        private static StatefulGame Game;

        //Used for drawing the game
        public static Camera Camera;

        //The common game time
        public static GameTime GameTime;

        //How much to multiply the game speed by - could change from computer to computer
        public static float GameSpeedMultiplier = 1/16f;
        public static float GameSpeed = 1;

        //The events to change the game initialization/update
        public delegate void EdgeGameEvent();
        public delegate void EdgeGameUpdateEvent(GameTime gameTime);
        public delegate void EdgeGameDrawEvent(SpriteBatch spriteBatch, GameTime gameTime);
        public static event EdgeGameEvent OnRun = delegate { };
        public static event EdgeGameEvent OnInit = delegate { };
        public static event EdgeGameEvent OnLoadContent = delegate { };
        public static event EdgeGameEvent OnReset = delegate { };
        public static event EdgeGameUpdateEvent OnUpdate = delegate { };
        public static event EdgeGameDrawEvent OnDraw = delegate { };

        //Gets the FPS that the game is currently running at
        public static int FPS { get; private set; }

        //Gets the game's graphics device - used for creating new textures
        public static GraphicsDevice GraphicsDevice { get { return Game.GraphicsDevice; } }
        public static SpriteBatch SpriteBatch { get { return Game.SpriteBatch; } }

        //Sets whether the mouse is visible or not
        public static bool MouseVisible { get { return Game.IsMouseVisible; } set { Game.IsMouseVisible = value; } }

        //The color the graphicsdevice will clear each frame
        public static Color ClearColor = Color.Black;
        //The color that debug draw will color in
        public static Color DebugDrawColor = Color.White;

        //The draw state which the game will draw
        public static DrawState DrawState = DrawState.Normal;

        //Gets/Sets the graphics preferred buffer size
        public static Vector2 WindowSize
        { 
            get 
            {
                return new Vector2(Game.Graphics.PreferredBackBufferWidth, Game.Graphics.PreferredBackBufferHeight);
            }
            set 
            {
                Game.Graphics.PreferredBackBufferWidth = (int)value.X;
                Game.Graphics.PreferredBackBufferHeight = (int)value.Y;
                Game.Graphics.ApplyChanges();

                Camera = new Camera(WindowSize / 2, Game.GraphicsDevice);
            }
        }

        //Initializes and starts the game
        public static void Start()
        {
            Game = new StatefulGame();
            InitializeScenes();
            InitializeResources();
            InitializeSounds();

            //Initializes all the game items
            DebugLogger.Init();

            WindowSize = new Vector2(800);

            //Connects all the events from the StatefulGame to the EdgeGame
            Game.OnRun += new StatefulGame.GameInitializeEvent(Game_OnRun);
            Game.OnInit += new StatefulGame.GameInitializeEvent(Game_OnInit);
            Game.OnLoadContent += new StatefulGame.GameInitializeEvent(Game_OnLoadContent);
            Game.OnUpdate += new StatefulGame.GameEvent(Game_OnUpdate);
            Game.OnDraw += new StatefulGame.GameEvent(Game_OnDraw);

            //Runs the game
            Game.Run();
        }

        //Stops the game
        public static void Stop()
        {
            Game.Exit();
        }

        //Erases all of the content, scenes, etc.
        public static void Reset()
        {
            Game.Content.Unload();
            Scenes.Clear();
            OnReset();
        }

        private static void Game_OnRun(StatefulGame game)
        {
            DebugLogger.Log('~', "EdgeGame Started");
            OnRun();
        }

        private static void Game_OnLoadContent(StatefulGame game)
        {
            InitializeBasicTextures();
            Input.Init();
            DebugLogger.Log('~', "EdgeGame Successfully Loaded Content");
            OnLoadContent();
        }

        private static void Game_OnInit(StatefulGame game)
        {
            DebugLogger.Log('~', "EdgeGame Successfully Initialized");
            OnInit();
        }

        private static void Game_OnUpdate(StatefulGame game, GameTime gameTime)
        {
            GameTime = gameTime;

            if (CurrentScene != null)
            {
                Input.Update(gameTime);
                Camera.Update(gameTime);

                Update();
            }

            OnUpdate(gameTime);
        }

        private static void Game_OnDraw(StatefulGame game, GameTime gameTime)
        {
            GameTime = gameTime;

            if (CurrentScene != null)
            {
                Game.GraphicsDevice.SetRenderTarget(Camera.Target);

                Game.GraphicsDevice.Clear(ClearColor);

                FPS = (int)(1000f / gameTime.ElapsedGameTime.TotalMilliseconds);

                Game.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Camera.GetTransform());
                Draw();
                Input.Draw(gameTime, Game.SpriteBatch);
                Game.SpriteBatch.End();

                Game.GraphicsDevice.SetRenderTarget(null);
                Game.GraphicsDevice.Clear(ClearColor);
                Camera.Draw(Game.SpriteBatch);
            }

                OnDraw(game.SpriteBatch, gameTime);
        }

        //Runs one update and draw
        public static void Tick()
        {
            Game.Tick();
        }

        //Gets the amount to multiply any game process speed by
        public static double GetFrameTimeMultiplier(GameTime gameTime)
        {
            return gameTime.ElapsedGameTime.TotalMilliseconds * GameSpeed * GameSpeedMultiplier;
        }
    }
}
