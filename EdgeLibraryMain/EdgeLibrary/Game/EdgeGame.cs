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
    public class EdgeGame : Game
    {
        private SpriteBatch SpriteBatch;
        private GraphicsDeviceManager Graphics;

        public ContentLoader Resources;
        public SoundLoader Sounds;
        public SceneHandler SceneHandler;

        public Color ClearColor = Color.DarkKhaki;

        public delegate void EdgeGameEvent(EdgeGame game);
        public event EdgeGameEvent OnInit = delegate { };
        public event EdgeGameEvent OnLoadContent = delegate { };
        public event EdgeGameEvent OnUnloadContent = delegate { };
        public delegate void EdgeGameUpdateEvent(GameTime gameTime, EdgeGame game);
        public event EdgeGameUpdateEvent OnUpdate = delegate { };
        public event EdgeGameUpdateEvent OnDraw = delegate { };

        public EdgeGame()
        {
            Resources = new ContentLoader(Content);
            Sounds = new SoundLoader(Content);
            SceneHandler = new SceneHandler();

            Graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            OnInit(this);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            OnLoadContent(this);
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            OnUnloadContent(this);
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            OnUpdate(gameTime, this);
            SceneHandler.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            OnDraw(gameTime, this);
            GraphicsDevice.Clear(ClearColor);
            SceneHandler.Draw(gameTime, SpriteBatch);
            base.Draw(gameTime);
        }
    }
}
