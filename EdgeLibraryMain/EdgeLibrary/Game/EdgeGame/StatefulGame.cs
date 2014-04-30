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

namespace EdgeLibrary
{
    //An instance of this is created in EdgeGame to be able to use events for updating and drawing
    public class StatefulGame : Game
    {
        //Game components
        public SpriteBatch SpriteBatch { get; protected set; }
        public GraphicsDeviceManager Graphics { get; protected set; }

        //The color to clear the GraphicsDevice with
        public Color ClearColor;

        //The events for changing the game initialization and update outside of the game
        public delegate void GameEvent(StatefulGame game, GameTime gameTime);
        public delegate void GameInitializeEvent(StatefulGame game);
        public event GameInitializeEvent OnRun = delegate { };
        public event GameInitializeEvent OnInit = delegate { };
        public event GameInitializeEvent OnLoadContent = delegate { };
        public event GameInitializeEvent OnUnloadContent = delegate { };
        public event GameEvent OnUpdate = delegate { };
        public event GameEvent OnDraw = delegate { };
        public event GameEvent OnStartUpdate = delegate { };
        public event GameEvent OnStartDraw = delegate { };

        public StatefulGame()
        {
            Content.RootDirectory = "Content";
            Graphics = new GraphicsDeviceManager(this);
            ClearColor = Color.Black;
            IsMouseVisible = true;
        }

        public new void Run()
        {
            //OnRun must be called before base.Run() because base.Run() keeps going forever until the game is stopped
            OnRun(this);
            base.Run();
        }

        protected override void Initialize()
        {
            base.Initialize();
            OnInit(this);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
            OnLoadContent(this);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            OnUnloadContent(this);
        }

        protected override void Update(GameTime gameTime)
        {
            OnStartUpdate(this, gameTime);
            base.Update(gameTime);
            OnUpdate(this, gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearColor);
            SpriteBatch.Begin();
            OnStartDraw(this, gameTime);

            base.Draw(gameTime);
            
            SpriteBatch.End();

            OnDraw(this, gameTime);
        }
    }
}
