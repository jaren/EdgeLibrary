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

        Sprite sprite;
        ParticleEmitter emitter;

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

            EdgeGame.update += new EdgeGame.EdgeGameEvent(EdgeGame_update);

            EdgeGame.WindowSize = new Vector2(700, 700);
            EdgeGame.ClearColor = Color.Gray;

            base.Initialize();
        }

        void EdgeGame_update(GameTime gameTime)
        {
            emitter.SetStartColor(sprite.Style.Color);
            emitter.SetFinishColor(sprite.Style.Color);
            emitter.SetRotation(sprite.Style.Rotation);
        }

        protected override void LoadContent() 
        {
            ResourceManager.LoadTexturesInSpritesheet("SpaceSheet.xml", "SpaceSheet");
            ResourceManager.LoadTexturesInSpritesheet("ParticleSheet.xml", "ParticleSheet");
            ResourceManager.LoadFont("SmallFont");
            ResourceManager.LoadFont("MediumFont");
            ResourceManager.LoadFont("LargeFont");

            ResourceManager.addTexture("modifiedLaser", TextureTools.Colorize(ResourceManager.getTexture("laserGreen"), Color.White, 10));

            EdgeGame.CollisionsInTextSprites = true;

            DebugPanel panel = new DebugPanel("SmallFont", Vector2.Zero, Color.Goldenrod);

            sprite = new Sprite("modifiedLaser", Vector2.One * 500);
            sprite.CollisionBodyType = ShapeTypes.circle;
            sprite.StyleChanger.Rotate(InputManager.MouseSprite, 90);
            sprite.StyleChanger.ColorChange(MathTools.RandomColor(), MathTools.RandomColor(), 1000);
            sprite.StyleChanger.FinishedColorChange += new StyleCapability.StyleColorEvent(StyleChanger_FinishedColorChange);
            sprite.Movement.FollowElement(InputManager.MouseSprite, 3);

            TextSprite tSprite = new TextSprite("SmallFont", "Test", new Vector2(500, 500), Color.Purple);

            
            emitter = new ParticleEmitter("Pixel", Vector2.Zero);
            emitter.Movement.ClampTo(sprite);
            emitter.SetSize(new Vector2(10, 10));
             
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
