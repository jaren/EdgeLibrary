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
        public event GameInitializeEvent OnRun;
        public event GameInitializeEvent OnInit;
        public event GameInitializeEvent OnLoadContent;
        public event GameInitializeEvent OnUnloadContent;
        public event GameEvent OnUpdate;
        public event GameEvent OnDraw;
        public event GameEvent OnStartUpdate;
        public event GameEvent OnStartDraw;

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
            if (OnRun != null)
            {
                OnRun(this);
            }
            base.Run();
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (OnInit != null)
            {
                OnInit(this);
            }
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
            if (OnLoadContent != null)
            {
                OnLoadContent(this);
            }
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            if (OnUnloadContent != null)
            {
                OnUnloadContent(this);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (OnStartUpdate != null)
            {
                OnStartUpdate(this, gameTime);
            }
            base.Update(gameTime);
            if (OnUpdate != null)
            {
                OnUpdate(this, gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearColor);

            if (OnStartDraw != null)
            {
                OnStartDraw(this, gameTime);
            }

            base.Draw(gameTime);

            if (OnDraw != null)
            {
                OnDraw(this, gameTime);
            }
        }
    }
}
