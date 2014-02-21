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
        /// <summary>
        /// Problems:
        ///     -EXTREMELY laggy when adding debug panels or particle emitters
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
            IsMouseVisible = false;

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

            DebugPanel debug = new DebugPanel("SmallFont", Color.White);

            
            Sprite s1 = new Sprite("SSERWEHWWEWE1", "enemyUFO", new Vector2(250, 400));
            s1.CollisionBodyType = ShapeTypes.circle;

            Sprite s2 = new Sprite("SASDGRGFDGWEFWRGWEEEWWE2", "meteorSmall", new Vector2(350, 400));
            s2.CollisionBodyType = ShapeTypes.circle;
            s2.AddCapability(new PointRotationCapability());
            s2.PointRotation.RotateElementAroundPoint(s1.Position);
            TextSprite textSprite = new TextSprite("TS1A:LFDK:LSKDJEIFDEFED", "SmallFont", "This is a TextSprite", new Vector2(300, 100), Color.Green);
            

            ParticleEmitter emitter = new ParticleEmitter("P1", "fire", new Vector2(500, 500));
            emitter.Clamp.ClampElement = InputManager.MouseSprite;
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
