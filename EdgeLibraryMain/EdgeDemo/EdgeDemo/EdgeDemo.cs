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

        FakeSprite colorChanger;

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

            EdgeGame.WindowSize = new Vector2(800, 800);
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

            TextureTools.Colorize(ResourceManager.getTexture("player"), Color.White, 10);

            EdgeGame.CollisionsInTextSprites = true;

            DebugPanel panel = new DebugPanel("SmallFont", Vector2.Zero, Color.Goldenrod);

            colorChanger = new FakeSprite();
            colorChanger.StyleChanger.ColorChange(MathTools.RandomColor(), MathTools.RandomColor(), 1000);
            colorChanger.StyleChanger.FinishedColorChange += new StyleCapability.StyleColorEvent(StyleChanger_FinishedColorChange);

            //TextSprite tSprite = new TextSprite("SmallFont", "Test", new Vector2(500, 500), Color.Purple);

            //Sprite sprite = new Sprite("player", Vector2.One * 300);
            
            ParticleEmitter emitter = new ParticleEmitter("Pixel", Vector2.One * 400);
            //emitter.BlendState = BlendState.AlphaBlend;
            emitter.update += new Element.ElementUpdateEvent(emitterUpdate);
            emitter.SetRotationSpeed(100f);
            emitter.SetLife(10000);
            emitter.EmitWait = 0;
            emitter.SetSize(new Vector2(1));
            emitter.GrowSpeed = 1;
            emitter.SetWidthHeight(10, 10);
             
        }

        void emitterUpdate(Element e, GameTime gameTime)
        {
            ((ParticleEmitter)e).SetStartColor(colorChanger.Style.Color);
            ((ParticleEmitter)e).SetFinishColor(colorChanger.Style.Color);
        }

        void StyleChanger_FinishedColorChange(StyleCapability capability, Color finishColor)
        {
            capability.ColorChange(finishColor, MathTools.RandomColor(), 1000);
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
