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
        /// <summary>
        /// Replace all of the "TOCHANGE" in the platform library with collision checks
        /// </summary>

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
            EdgeGame.GameDrawState = GameDrawState.Normal;
            IsMouseVisible = true;

            EdgeGame.WindowSize = new Vector2(1000);
            EdgeGame.ClearColor = Color.White;

            base.Initialize();
        }

        protected override void LoadContent() 
        {
            ResourceManager.LoadTexturesInSpritesheet("SpaceSheet.xml", "SpaceSheet");
            ResourceManager.LoadTexturesInSpritesheet("ParticleSheet.xml", "ParticleSheet");
            ResourceManager.LoadFont("SmallFont");
            ResourceManager.LoadFont("MediumFont");
            ResourceManager.LoadFont("LargeFont");

            EdgeGame.CollisionsInTextSprites = true;

            EdgeGame.MainScene().Background = ResourceManager.textureFromString("Wood Background");

            DebugPanel panel = new DebugPanel("SmallFont", Vector2.Zero, Color.Goldenrod);

            Sprite torch = new Sprite("Pixel", new Vector2(400, 450));
            torch.Style.Color = MathTools.ColorFromHex("1A0805");
            torch.Width = 20;
            torch.Height = 100;
            torch.Movement.ClampTo(InputManager.MouseSprite);

            ParticleEmitter fireEmitter = new ParticleEmitter("fire", new Vector2(400, 400));

            fireEmitter.MinColorIndex = new ColorChangeIndex(Color.Orange, 1000);
            fireEmitter.MinColorIndex.Add(Color.Gray, 500);
            fireEmitter.MinColorIndex.Add(Color.Transparent, 0);

            fireEmitter.MaxColorIndex = new ColorChangeIndex(Color.OrangeRed, 1000);
            fireEmitter.MaxColorIndex.Add(Color.LightGray, 500);
            fireEmitter.MaxColorIndex.Add(Color.Transparent, 0);

            fireEmitter.SetRotationSpeed(0.1f);
            fireEmitter.SetLife(5000);
            fireEmitter.EmitWait = 0;
            fireEmitter.MinVelocity = new Vector2(-0.5f, -2.5f);
            fireEmitter.MaxVelocity = new Vector2(0.5f, -2.5f);
            fireEmitter.MinSize = new Vector2(60);
            fireEmitter.MaxSize = new Vector2(40);
            fireEmitter.SetWidthHeight(0, 0);
            fireEmitter.Movement.ClampTo(torch, new Vector2(0, -torch.Height / 2));
             
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
