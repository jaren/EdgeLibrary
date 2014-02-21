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
            EdgeGame.GameDrawState = GameDrawState.Hybrid;

            IsMouseVisible = true;

            EdgeGame.SetWindowSize(new Vector2(700, 700));
            EdgeGame.ClearColor = Color.Gray;

            base.Initialize();
        }

        protected override void LoadContent() 
        {
            ResourceManager.LoadTexture("flatDark01");
            ResourceManager.LoadTexture("laserGreen");
            ResourceManager.LoadFont("SpriteFont1");


            Sprite s1 = new Sprite("flatDark01", new Vector2(0, 400));

            Sprite s2 = new Sprite("laserGreen", new Vector2(700, 400));
            ((SimpleMovementCapability)s2.Capability("SimpleMovement")).MoveElementTo(s1.Position, 1);
            ((SimpleMovementCapability)s1.Capability("SimpleMovement")).MoveElementTo(s2.Position, 1);
            //s2.AddCapability(new AdvancedMovementCapability());
            //((AdvancedMovementCapability)s2.Capability("AdvancedMovement")).RotateElementAroundPoint(s1.Position);

            s1.CollisionStart += new Sprite.CollisionEvent(s1_CollisionStart);

            TextSprite textSprite = new TextSprite("SpriteFont1", "This is a TextSprite", new Vector2(300, 300), Color.Green);

            FPSSprite = new TextSprite("SpriteFont1", "FPS: 0", new Vector2(0, 650), Color.White);
            FPSSprite.CenterText = false;
            MousePosSprite = new TextSprite("SpriteFont1", "MouseX: 0, MouseY: 0", new Vector2(0, 600), Color.White);
            MousePosSprite.CenterText = false;
        }

        void s1_CollisionStart(CollisionEventArgs e)
        {
            throw new NotImplementedException();
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
