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

        PlatformCharacter sprite;

        public EdgeDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            EdgeGame.Init(Content, GraphicsDevice, graphics, spriteBatch);
            EdgeGame.GameDrawState = GameDrawState.Hybrid;
            IsMouseVisible = true;

            EdgeGame.WindowSize = new Vector2(700, 700);
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

            PlatformLevel level = new PlatformLevel("LEVEL", new Vector2(0, -9.8f));
            EdgeGame.AddScene(level);
            EdgeGame.SelectedScene = level;
            sprite = new PlatformCharacter("S", "Pixel", new Vector2(500, 500));
            sprite.Style.Color = Color.Black;
            sprite.Scale = new Vector2(30);
            level.AddSprite(sprite);

            DebugPanel panel = new DebugPanel("SmallFont", new Vector2(0), Color.Gold);

            level.CreateScreenBox();

            PlatformStatic sprite2 = new PlatformStatic("S2", "Pixel", new Vector2(300, 500));
            sprite2.Style.Color = Color.White;
            sprite2.Scale = new Vector2(40, 100);
            level.AddSprite(sprite2);
             
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            EdgeGame.Update(gameTime);
            float speed = 0.1f;

            if (InputManager.IsKeyDown(Keys.Left))
            {
                sprite.ApplyImpulse(new Vector2(-speed, 0));
            }
            if (InputManager.IsKeyDown(Keys.Right))
            {
                sprite.ApplyImpulse(new Vector2(speed, 0));
            }
            if (InputManager.IsKeyDown(Keys.Up))
            {
                sprite.ApplyImpulse(new Vector2(0, speed));
            }
            if (InputManager.IsKeyDown(Keys.Down))
            {
                sprite.ApplyImpulse(new Vector2(0, -speed));
            }
            if (InputManager.IsKeyDown(Keys.Space))
            {
                sprite.Shoot(InputManager.MousePos(), 3);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            EdgeGame.Draw(gameTime);
        }
    }
}
