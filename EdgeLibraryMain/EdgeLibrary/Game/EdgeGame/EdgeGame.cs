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
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace EdgeLibrary
{
    //The main game type
    public static partial class EdgeGame
    {
        //The game which is run
        public static StatefulGame Game { get; private set; }

        //Used for drawing the game
        public static Camera Camera;
        public static Camera3D Camera3D;

        //Farseer Physics
        public static World World { get; private set; }
        public static float WorldStep = 1 / 60f;

        //The common game time
        public static GameTime GameTime;

        //How quickly to run the game
        public static float GameSpeed = 1;

        //The events to change the game initialization/update
        public delegate void EdgeGameEvent();
        public delegate void EdgeGameUpdateEvent(GameTime gameTime);
        public static event EdgeGameEvent OnRun;
        public static event EdgeGameEvent OnInit;
        public static event EdgeGameEvent OnLoadContent;
        public static event EdgeGameEvent OnReset;
        public static event EdgeGameUpdateEvent OnUpdate;
        public static event EdgeGameUpdateEvent OnDraw;

        //Gets the FPS that the game is currently running at
        public static int FPS { get; private set; }

        //Sets whether the mouse is visible or not
        public static bool MouseVisible { get { return Game.IsMouseVisible; } set { Game.IsMouseVisible = value; } }

        //The color the graphicsdevice will clear each frame
        public static Color ClearColor { get { return Game.ClearColor; } set { Game.ClearColor = value; } }

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
                Camera3D.AspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
            }
        }

        //Initializes and starts the game
        public static void Start()
        {
            Game = new StatefulGame();
            InitializeResources();
            InitializeSounds();

            Camera3D = new Camera3D(new Vector3(0, 0, 20), Vector3.Zero, Vector3.Up, 1);

            WindowSize = new Vector2(800);

            //Connects all the events from the StatefulGame to the EdgeGame
            Game.OnRun += new StatefulGame.GameInitializeEvent(Game_OnRun);
            Game.OnInit += new StatefulGame.GameInitializeEvent(Game_OnInit);
            Game.OnLoadContent += new StatefulGame.GameInitializeEvent(Game_OnLoadContent);

            //OnStartUpdate and OnStartDraw must be used so the graphics device can be properly cleared
            Game.OnStartUpdate += new StatefulGame.GameEvent(Game_OnUpdate);
            Game.OnStartDraw += new StatefulGame.GameEvent(Game_OnDraw);
            Game.OnDraw += new StatefulGame.GameEvent(Game_OnFinishDraw);

            //Runs the game
            Game.Run();
        }

        //Sets the world
        public static void InitializeWorld(Vector2 gravity)
        {
            World = new World(gravity);
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
            Game.Components.Clear();
            if (OnReset != null)
            {
                OnReset();
            }
        }

        private static void Game_OnRun(StatefulGame game)
        {
            if (OnRun != null)
            {
                OnRun();
            }
        }

        private static void Game_OnLoadContent(StatefulGame game)
        {
            InitializeBasicTextures();
            if (OnLoadContent != null)
            {
                OnLoadContent();
            }
        }

        private static void Game_OnInit(StatefulGame game)
        {
            if (OnInit != null)
            {
                OnInit();
            }
        }

        //This is called before the stateful game updates
        private static void Game_OnUpdate(StatefulGame game, GameTime gameTime)
        {
            //Sets the game time
            GameTime = gameTime;

            if (World != null)
            {
                //Updates the physics
                World.Step(WorldStep);
            }

            //Updates all the game items
            Input.Update(gameTime);

            if (OnUpdate != null)
            {
                OnUpdate(gameTime);
            }
        }

        //This is called before the stateful game draws
        private static void Game_OnDraw(StatefulGame game, GameTime gameTime)
        {
            //Sets the game time
            GameTime = gameTime;

            //Prepares to render to the camera
            Game.GraphicsDevice.SetRenderTarget(Camera.Target);

            //Clears the graphics device
            Game.GraphicsDevice.Clear(ClearColor);

            //Sets the FPS in draw - it's always 60 fps in update
            FPS = (int)(1000 / gameTime.ElapsedGameTime.TotalMilliseconds + 1);

            //Starts drawing to the camera render target - it needs to restart the spritebatch with the given settings
            Game.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Camera.GetTransform());
        }

        //This is called after the stateful game draws
        private static void Game_OnFinishDraw(StatefulGame game, GameTime gameTime)
        {
            //Prepares to render to the screen
            Game.GraphicsDevice.SetRenderTarget(null);
            Game.GraphicsDevice.Clear(ClearColor);

            Game.SpriteBatch.End();

            //Draws to the screen
            Camera.Draw(Game.SpriteBatch);

            if (OnDraw != null)
            {
                OnDraw(gameTime);
            }

            if (Game.SpriteBatch.IsStarted)
            {
                Game.SpriteBatch.End();
            }
        }

        //Runs one update and draw
        public static void Tick()
        {
            Game.Tick();
        }
    }
}
