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
using EdgeLibrary.Platform;

namespace EdgeDemo
{
    public class EdgeDemo : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        PlatformSprite sprite;

        public EdgeDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            EdgeGame.Init(Content, GraphicsDevice, graphics, spriteBatch);
            EdgeGame.GameDrawState = GameDrawState.Normal;
            IsMouseVisible = true;

            EdgeGame.SetWindowSize(new Vector2(700, 700));
            EdgeGame.ClearColor = Color.Gray;

            base.Initialize();
        }

        protected override void LoadContent() 
        {
            ResourceManager.LoadTexturesInSpritesheet("SpaceSheet.xml", "SpaceSheet");
            ResourceManager.LoadTexturesInSpritesheet("ParticleSheet.xml", "ParticleSheet");
            ResourceManager.LoadFont("SmallFont");
            ResourceManager.LoadFont("MediumFont");
            ResourceManager.LoadFont("LargeFont");

            
            PlatformLevel level = new PlatformLevel("LEVEL");
            PlatformGame game = new PlatformGame("ID", level);
            EdgeGame.Scenes.Add(game);
            EdgeGame.SelectedScene = game;
            sprite = new PlatformSprite("S", "Pixel", new Vector2(500, 500));
            sprite.Style.Color = Color.Black;
            sprite.Scale = new Vector2(30);
            level.AddSprite(sprite);

            PlatformStatic bottom = new PlatformStatic("B", "Pixel", new Vector2(0, 680));
            bottom.Style.Color = Color.DarkGray;
            bottom.Scale = new Vector2(700, 20);
            level.AddSprite(bottom);

            PlatformCharacter sprite2 = new PlatformCharacter("S2", "Pixel", new Vector2(300, 500));
            sprite2.Style.Color = Color.White;
            sprite2.Scale = new Vector2(40, 100);
            level.AddSprite(sprite2);
             
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            EdgeGame.Update(gameTime);
            int speed = 2;

            if (InputManager.IsKeyDown(Keys.Left))
            {
                sprite.Position = new Vector2(sprite.Position.X - speed, sprite.Position.Y);
            }
            if (InputManager.IsKeyDown(Keys.Right))
            {
                sprite.Position = new Vector2(sprite.Position.X + speed, sprite.Position.Y);
            }
            if (InputManager.IsKeyDown(Keys.Up))
            {
                sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y - speed - 9.8f);
            }
            if (InputManager.IsKeyDown(Keys.Down))
            {
                sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y + speed);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            EdgeGame.Draw(gameTime);
        }
    }
}
