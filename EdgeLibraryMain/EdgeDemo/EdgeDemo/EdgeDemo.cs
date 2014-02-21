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

        TextSprite FPSSprite;
        TextSprite MousePosSprite;

        public EdgeDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            EdgeGame.Init(Content, GraphicsDevice, graphics, spriteBatch);
            EdgeGame.GameDrawState = GameDrawState.Debug;
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


            Sprite s1 = new Sprite("enemyUFO", new Vector2(250, 400));
            s1.CollisionBodyType = ShapeTypes.circle;

            Sprite s2 = new Sprite("meteorSmall", new Vector2(350, 400));
            s2.CollisionBodyType = ShapeTypes.circle;
            s2.AddCapability(new AdvancedMovementCapability());
            ((AdvancedMovementCapability)s2.Capability("AdvancedMovement")).RotateElementAroundPoint(s1.Position);

            TextSprite textSprite = new TextSprite("SmallFont", "This is a TextSprite", new Vector2(300, 100), Color.Green);

            ParticleEmitter emitter = new ParticleEmitter("fire", Vector2.Zero);
            ((ClampCapability)emitter.Capability("Clamp")).ClampElement = InputManager.MouseSprite;
            emitter.EmitWait = 1000;

            FPSSprite = new TextSprite("SmallFont", "FPS: 0", new Vector2(400, 650), Color.White);
            MousePosSprite = new TextSprite("SmallFont", "MouseX: 0, MouseY: 0", new Vector2(400, 600), Color.White);
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            EdgeGame.Update(gameTime);

            FPSSprite.Text = string.Format("FPS: {0}", FPSCounter.FPS);
            MousePosSprite.Text = string.Format("MouseX: {0}, MouseY: {1}", InputManager.MousePos().X, InputManager.MousePos().Y);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            EdgeGame.Draw(gameTime);
        }
    }
}
