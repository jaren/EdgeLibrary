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
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;

        ContentLoader Resources;
        SceneHandler SceneHandler;

        public delegate void EdgeGameEvent(EdgeGame game);
        public event EdgeGameEvent OnInit = delegate { };
        public event EdgeGameEvent OnLoadContent = delegate { };
        public event EdgeGameEvent OnUnloadContent = delegate { };
        public event EdgeGameEvent OnUpdate = delegate { };
        public event EdgeGameEvent OnDraw = delegate { };

        public EdgeGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Resources = new ContentLoader();
            SceneHandler = new SceneHandler();
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
        }

        protected override void UnloadContent()
        {
            OnUnloadContent(this);
        }

        protected override void Update(GameTime gameTime)
        {
            OnUpdate(this);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            OnDraw(this);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
