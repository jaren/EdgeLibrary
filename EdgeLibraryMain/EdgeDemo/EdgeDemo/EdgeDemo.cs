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
            EdgeGame.GameDrawState = GameDrawState.Normal;
            IsMouseVisible = false;
            InputManager.MouseSprite.Scale = new Vector2(10);
            InputManager.MouseSprite.DrawLayer = 50;

            EdgeGame.SetWindowSize(new Vector2(700, 700));
            EdgeGame.ClearColor = Color.Gray;

            base.Initialize();
        }

        protected override void LoadContent() 
        {
            ResourceManager.LoadTexturesInSpritesheet("SpaceSheet.xml", "SpaceSheet");
            ResourceManager.LoadTexturesInSpritesheet("ParticleSheet.xml", "ParticleSheet");
            ResourceManager.addTexture("gradient", TextureTools.CreateVerticalGradient((int)EdgeGame.WindowSize().X, (int)EdgeGame.WindowSize().Y, Color.Black, Color.Black));
            ResourceManager.LoadFont("SmallFont");
            ResourceManager.LoadFont("MediumFont");
            ResourceManager.LoadFont("LargeFont");

            DebugPanel debug = new DebugPanel("SmallFont", new Vector2(10, 0), Color.White);
            debug.DrawLayer = 100;

            Sprite s1 = new Sprite("S1", "gradient", new Vector2(350, 350));
            s1.DrawLayer = 0;

            Sprite s2 = new Sprite("S2", "meteorSmall", new Vector2(350, 400));
            s2.DrawLayer = 1;
            s2.CollisionBodyType = ShapeTypes.circle;
            s2.AddCapability(new PointRotationCapability());
            s2.SimpleMovement.Follow(InputManager.MouseSprite, 5);

            ParticleEmitter emitter = new ParticleEmitter("E1", "fire", new Vector2(500, 500));
            emitter.DrawLayer = s2.DrawLayer + 1;
            emitter.MaxStartColor = Color.Brown;
            emitter.MinStartColor = Color.RosyBrown;
            emitter.MaxFinishColor = Color.LightGoldenrodYellow;
            emitter.MinFinishColor = Color.Goldenrod;
            emitter.GrowSpeed = -0.2f;
            emitter.MinLife = 1000;
            emitter.MaxLife = 2000;
            emitter.Clamp.ClampTo(s2);
            emitter.EmitWait = 0;
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
