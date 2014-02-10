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
using EdgeLibrary;

namespace EdgeDemo
{
    public class EdgeDemo : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public EdgeDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            EdgeGame.Init(Content, GraphicsDevice, graphics, spriteBatch);

            EdgeGame.SetWindowSize(new Vector2(700, 700));
            EdgeGame.ClearColor = Color.Gray;

            base.Initialize();
        }

        protected override void LoadContent() 
        {
            ResourceManager.LoadTexture("flatDark01");
            ResourceManager.LoadTexture("laserGreen");
            ResourceManager.LoadFont("SpriteFont1");
            Sprite s1 = new Sprite("flatDark01", new Vector2(500, 500));
            Sprite s2 = new Sprite("laserGreen", new Vector2(500, 500));
            TextSprite textSprite = new TextSprite("SpriteFont1", "This is a TextSprite", new Vector2(300, 300), Color.Green);
            //EdgeGame.Effect = new BlackWhiteEffect();
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            EdgeGame.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            EdgeGame.Draw(gameTime);
        }
    }
}
