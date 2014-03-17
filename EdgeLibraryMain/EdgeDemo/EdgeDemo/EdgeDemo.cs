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

        TextSprite platformLabel;
        Sprite platformSprite;

        float speed = 15;

        public void initEdgeGame()
        {
            EdgeGame.ClearColor = new Color(20, 20, 20);
            TextSprite Header = new TextSprite("LargeFont", "EdgeDemo", new Vector2(EdgeGame.WindowSize.X/2, 50), Color.White);

            float buttonY = 400;
            float buttonXDiff = 300;

            Button PlatformButton = new Button("Pixel", new Vector2(EdgeGame.WindowSize.X/2, buttonY), Color.Transparent);
            PlatformButton.OffStyle.Color = Color.Transparent;
            PlatformButton.MouseOverStyle.Color = Color.Transparent;
            PlatformButton.DrawLayer = -1;
            PlatformButton.OffScale = new Vector2(buttonXDiff, EdgeGame.WindowSize.Y - 200);
            PlatformButton.MouseOverScale = PlatformButton.OffScale;
            PlatformButton.OnScale = PlatformButton.OffScale;
            PlatformButton.MouseOver += new Button.ButtonEventHandler(PlatformButton_MouseOver);
            PlatformButton.MouseOff += new Button.ButtonEventHandler(PlatformButton_MouseOff);

            platformLabel = new TextSprite("LargeFont", "Platform", PlatformButton.Position, Color.White);

            EdgeGame.update += new EdgeGame.UpdateEvent(EdgeGame_update);
        }

        void EdgeGame_update(GameTime gameTime)
        {
        }

        void PlatformButton_MouseOff(ButtonEventArgs e)
        {
            platformLabel.Movement.MoveTo(new Vector2(platformLabel.Position.X, 400), speed);
            platformLabel.StyleChanger.ColorChange(platformLabel.Style.Color, Color.White, 500);
        }

        void PlatformButton_MouseOver(ButtonEventArgs e)
        {
            platformLabel.Movement.MoveTo(new Vector2(platformLabel.Position.X, EdgeGame.WindowSize.Y), speed);
            platformLabel.StyleChanger.ColorChange(platformLabel.Style.Color, Color.Transparent, 500);
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
