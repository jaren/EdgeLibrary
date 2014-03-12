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

        public void initEdgeGame()
        {
            PlatformLevel level = new PlatformLevel("level", new Vector2(0, -5));
            EdgeGame.SelectedScene = level;
            level.Background = ResourceManager.textureFromString("Wood Background");
            level.CreateScreenBox();

            DebugPanel panel = new DebugPanel("SmallFont", Vector2.Zero, Color.Goldenrod);

            PlatformCharacter sprite = new PlatformCharacter("Pixel", new Vector2(200));
            sprite.StyleChanger.ColorChange(MathTools.RandomGrayscaleColor(Color.White, Color.Black), MathTools.RandomGrayscaleColor(Color.White, Color.Black), 1000);
            sprite.StyleChanger.FinishedColorChange += new StyleCapability.StyleColorEvent(StyleChanger_FinishedColorChange);
            sprite.update += new Element.ElementUpdateEvent(updateSprite);
            sprite.ShootDelay = 100;
            sprite.Scale = new Vector2(50);

            sprite.ProjectileTexture = "laserGreen";
            sprite.ProjectileRotationAdd = 90;
            sprite.ProjectileSpeed = 8;
        }


        void updateSprite(Element e, GameTime gameTime)
        {
            PlatformCharacter sprite = (PlatformCharacter)e;
            float speed = 2;
            float decel = 0.25f;

            if (InputManager.IsKeyDown(Keys.Left))
            {
                sprite.ApplyImpulse(new Vector2(-speed, 0), decel);
            }
            if (InputManager.IsKeyDown(Keys.Right))
            {
                sprite.ApplyImpulse(new Vector2(speed, 0), decel);
            }
            if (InputManager.IsKeyDown(Keys.Down))
            {
                sprite.ApplyImpulse(new Vector2(0, -speed), decel);
            }
            if (InputManager.IsKeyDown(Keys.Up))
            {
                sprite.ApplyImpulse(new Vector2(0, speed), decel);
            }
            if (InputManager.IsKeyDown(Keys.Space))
            {
                sprite.Shoot(InputManager.MousePosition, 0.01f);
            }
        }

        void StyleChanger_FinishedColorChange(StyleCapability capability, Color finishColor)
        {
            capability.ColorChange(finishColor, MathTools.RandomGrayscaleColor(Color.White, Color.Black), 1000);
        }

        #region UNUSED

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

            EdgeGame.WindowSize = new Vector2(1300, 700);
            EdgeGame.ClearColor = Color.White;

            base.Initialize();

            initEdgeGame();
        }

        protected override void LoadContent()
        {
            ResourceManager.LoadTexturesInSpritesheet("SpaceSheet.xml", "SpaceSheet");
            ResourceManager.LoadTexturesInSpritesheet("ParticleSheet.xml", "ParticleSheet");
            ResourceManager.LoadFont("SmallFont");
            ResourceManager.LoadFont("MediumFont");
            ResourceManager.LoadFont("LargeFont");
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
        #endregion
    }
}
