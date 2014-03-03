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

        FakeSprite fireChanger;
        FakeSprite smokeChanger;

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

            fireChanger = new FakeSprite();
            fireChanger.StyleChanger.ColorChange(MathTools.RandomColor(Color.OrangeRed, Color.DarkOrange), MathTools.RandomColor(Color.OrangeRed, Color.DarkOrange), 1000);
            fireChanger.StyleChanger.FinishedColorChange += new StyleCapability.StyleColorEvent(fireChangerFinished);

            smokeChanger = new FakeSprite();
            smokeChanger.StyleChanger.ColorChange(MathTools.RandomColor(Color.Gray, Color.DarkGray), MathTools.RandomColor(Color.Gray, Color.DarkGray), 1000);
            smokeChanger.StyleChanger.FinishedColorChange += new StyleCapability.StyleColorEvent(smokeChangerFinished);

            EdgeGame.MainScene().Background = ResourceManager.textureFromString("Wood Background");

            DebugPanel panel = new DebugPanel("SmallFont", Vector2.Zero, Color.Goldenrod);

            Sprite torch = new Sprite("Pixel", new Vector2(400, 450));
            torch.Style.Color = MathTools.ColorFromHex("1A0805");
            torch.Width = 20;
            torch.Height = 100;
            torch.Movement.ClampTo(InputManager.MouseSprite);

            ParticleEmitter fireEmitter = new ParticleEmitter("fire", new Vector2(400, 400));
            fireEmitter.update += new Element.ElementUpdateEvent(fireEmitterUpdate);

            fireEmitter.MinColorIndex = new ColorChangeIndex(1000, Color.Orange, Color.Gray, Color.Transparent);
            fireEmitter.MinColorIndex.SetTime(0, 1750);
            fireEmitter.MinColorIndex.SetTime(1, 250);
            fireEmitter.MinColorIndex.SetTime(2, 0);

            fireEmitter.MaxColorIndex = fireEmitter.MinColorIndex;

            fireEmitter.SetLife(5000);
            fireEmitter.MinVelocity = new Vector2(-0.25f, -2.5f);
            fireEmitter.MaxVelocity = new Vector2(0.25f, -2.5f);
            fireEmitter.SetSize(new Vector2(60));
            fireEmitter.Movement.ClampTo(torch, new Vector2(0, -torch.Height / 2));
            fireEmitter.SetEmitArea(0, 0);
             
        }

        void fireEmitterUpdate(Element e, GameTime g)
        {
            ((ParticleEmitter)e).MinColorIndex = new ColorChangeIndex(1000, fireChanger.Style.Color, smokeChanger.Style.Color, Color.Transparent);
            ((ParticleEmitter)e).MinColorIndex.SetTime(0, 1750);
            ((ParticleEmitter)e).MinColorIndex.SetTime(1, 250);
            ((ParticleEmitter)e).MaxColorIndex.SetTime(2, 0);

            ((ParticleEmitter)e).MaxColorIndex = ((ParticleEmitter)e).MinColorIndex;
        }

        void fireChangerFinished(StyleCapability capability, Color finishColor)
        {
            capability.ColorChange(finishColor, MathTools.RandomColor(Color.OrangeRed, Color.Orange), 1000);
        }

        void smokeChangerFinished(StyleCapability capability, Color finishColor)
        {
            capability.ColorChange(finishColor, MathTools.RandomColor(new Color(100, 100, 100), new Color(100, 100, 100)), 1000);
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
