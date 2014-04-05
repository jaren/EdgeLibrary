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
    //The type of drawing the game will use
    public enum DrawState
    {
        //Normal game drawing
        Normal,
        //Draws only collision bodies
        Debug,
        //Draws collision bodies and normal game drawing
        Hybrid
    }

    //The main game type
    public class EdgeGame : Game
    {
        //Game components
        private SpriteBatch SpriteBatch;
        private GraphicsDeviceManager Graphics;
        public SceneHandler SceneHandler;

        //Used for drawing the game
        public Camera Camera;

        //Gets the current running game
        public static EdgeGame Instance { get; private set; }

        //Gets the FPS
        public int FPS;

        //The color the graphicsdevice will clear each frame
        public Color ClearColor = Color.Black;
        //The color that debug draw will color in
        public Color DebugDrawColor { get { return SceneHandler.DebugDrawColor; } set { SceneHandler.DebugDrawColor = value; } }

        //The draw state which the game will draw
        public DrawState DrawState = DrawState.Normal;

        //Gets/Sets the graphics preferred buffer size
        public Vector2 WindowSize
        { 
            get 
            { 
                return new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
            }
            set 
            { 
                Graphics.PreferredBackBufferWidth = (int)value.X;
                Graphics.PreferredBackBufferHeight = (int)value.Y;
                Graphics.ApplyChanges();

                Camera = new Camera(WindowSize / 2, GraphicsDevice);
            }
        }

        //The events for changing the game initialization outside of the game
        public delegate void EdgeGameEvent(EdgeGame game);
        public event EdgeGameEvent OnInit = delegate { };
        public event EdgeGameEvent OnLoadContent = delegate { };
        public event EdgeGameEvent OnUnloadContent = delegate { };

        //The events for changing the game update outside of the game
        public delegate void EdgeGameUpdateEvent(GameTime gameTime, EdgeGame game);
        public event EdgeGameUpdateEvent OnUpdate = delegate { };
        public event EdgeGameUpdateEvent OnDraw = delegate { };

        /// <summary>
        /// Creates a new game with the given scene ID
        /// </summary>
        /// <param name="sceneID">The ID of the original scene</param>
        public EdgeGame(string sceneID)
        {
            Content.RootDirectory = "Content";
            SceneHandler = new SceneHandler(sceneID);

            Graphics = new GraphicsDeviceManager(this);

            IsMouseVisible = true;
        }
        public EdgeGame() : this("Main") { }

        //Initializes the game
        protected override void Initialize()
        {
            DebugLogger.Init();
            base.Initialize();
            OnInit(this);
            DebugLogger.Log('~', "EdgeGame Successfully Initialized");
        }

        //Loads all the game content
        protected override void LoadContent()
        {
            Camera = new Camera(WindowSize / 2, GraphicsDevice);
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Resources.Init(Content);
            Sounds.Init(Content);
            Input.Init();

            base.LoadContent();
            OnLoadContent(this);
            DebugLogger.Log('~', "EdgeGame Successfully Loaded Content"); 
        }

        //Unloads all the game content
        protected override void UnloadContent()
        {
            base.UnloadContent();
            OnUnloadContent(this);
            DebugLogger.Log('~', "EdgeGame Successfully Unloaded Content"); 
        }

        //Updates the game
        protected override void Update(GameTime gameTime)
        {
            SceneHandler.Update(gameTime);
            Input.Update(gameTime);
            Camera.Update(gameTime);

            FPS = (int)(1000f / gameTime.ElapsedGameTime.TotalMilliseconds);

            base.Update(gameTime);
            OnUpdate(gameTime, this);
        }

        //Draws the game
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(Camera.Target);

            GraphicsDevice.Clear(ClearColor);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Camera.GetTransform());
            SceneHandler.Draw(gameTime, SpriteBatch, DrawState);
            Input.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(ClearColor);
            Camera.Draw(SpriteBatch);

            base.Draw(gameTime);
            OnDraw(gameTime, this);
        }

        //Starts the game
        public new void Run()
        {
            Instance = this;
            base.Run();
        }

        //Creates a new Texture2D from the graphics device
        public static Texture2D CreateNewTexture(int width, int height)
        {
            return new Texture2D(Instance.GraphicsDevice, width, height);
        }
    }
}
