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

        TextSprite Header;
        ParticleEmitter MasterEmitter;

        protected override void LoadContent()
        {
            ResourceManager.LoadTexturesInSpritesheet("SpaceSheet.xml", "SpaceSheet");
            ResourceManager.LoadTexturesInSpritesheet("ParticleSheet.xml", "ParticleSheet");
            ResourceManager.LoadFont("Fonts/Comic Sans/ComicSans-10");
            ResourceManager.LoadFont("Fonts/Comic Sans/ComicSans-20");
            ResourceManager.LoadFont("Fonts/Comic Sans/ComicSans-30");
            ResourceManager.LoadFont("Fonts/Comic Sans/ComicSans-40");
            ResourceManager.LoadFont("Fonts/Comic Sans/ComicSans-50");
            ResourceManager.LoadFont("Fonts/Comic Sans/ComicSans-60");
            ResourceManager.LoadFont("Fonts/Courier New/CourierNew-10");
            ResourceManager.LoadFont("Fonts/Courier New/CourierNew-20");
            ResourceManager.LoadFont("Fonts/Courier New/CourierNew-30");
            ResourceManager.LoadFont("Fonts/Courier New/CourierNew-40");
            ResourceManager.LoadFont("Fonts/Courier New/CourierNew-50");
            ResourceManager.LoadFont("Fonts/Courier New/CourierNew-60");
            ResourceManager.LoadFont("Fonts/Georgia/Georgia-10");
            ResourceManager.LoadFont("Fonts/Georgia/Georgia-20");
            ResourceManager.LoadFont("Fonts/Georgia/Georgia-30");
            ResourceManager.LoadFont("Fonts/Georgia/Georgia-40");
            ResourceManager.LoadFont("Fonts/Georgia/Georgia-50");
            ResourceManager.LoadFont("Fonts/Georgia/Georgia-60");
            ResourceManager.LoadFont("Fonts/Impact/Impact-10");
            ResourceManager.LoadFont("Fonts/Impact/Impact-20");
            ResourceManager.LoadFont("Fonts/Impact/Impact-30");
            ResourceManager.LoadFont("Fonts/Impact/Impact-40");
            ResourceManager.LoadFont("Fonts/Impact/Impact-50");
            ResourceManager.LoadFont("Fonts/Impact/Impact-60");
        }

        public void initEdgeGame()
        {
            EdgeGame.GameDrawState = GameDrawState.Normal;
            IsMouseVisible = true;

            EdgeGame.WindowSize = new Vector2(1300, 700);

            EdgeGame.ClearColor = new Color(20, 20, 20);

            Header = new TextSprite("Impact-60", "EdgeDemo", new Vector2(EdgeGame.WindowSize.X/2, 50), Color.White);

            Vector2[] buttonPositions = new Vector2[2] { new Vector2(300, 300), new Vector2(1000, 300) };

            LabelButton PlatformButton = new LabelButton("Georgia-50", "Platform Demo", buttonPositions[0], Color.OrangeRed);
            PlatformButton.OffStyle.Color = Color.Gray;
            PlatformButton.MouseOverStyle.Color = Color.OrangeRed;
            PlatformButton.MouseOver += new Button.ButtonEventHandler(platform_mouseOver);
            PlatformButton.MouseOff += new Button.ButtonEventHandler(buttonMouseOff);
            PlatformButton.Click += new Button.ButtonEventHandler(platform_click);

            LabelButton GameButton = new LabelButton("Georgia-50", "Game Demo", buttonPositions[1], Color.Aquamarine);
            GameButton.OffStyle.Color = Color.Gray;
            GameButton.MouseOverStyle.Color = Color.Aquamarine;
            GameButton.MouseOver += new Button.ButtonEventHandler(game_mouseOver);
            GameButton.MouseOff += new Button.ButtonEventHandler(buttonMouseOff);
            GameButton.Click += new Button.ButtonEventHandler(game_click);

            MasterEmitter = new ParticleEmitter("fire", EdgeGame.WindowSize / 2);
            MasterEmitter.DrawLayer = -1;
            emitter_toNormal();
        }

        private void emitter_toNormal()
        {
            Header.Style.Color = Color.White;
            MasterEmitter.Position = EdgeGame.WindowSize / 2;
            MasterEmitter.Texture = ResourceManager.getTexture("fire");
            MasterEmitter.SetSize(new Vector2(50), new Vector2(100));
            MasterEmitter.BlendState = BlendState.Additive;
            MasterEmitter.GrowSpeed = 1;
            MasterEmitter.SetRotationSpeed(0);
            MasterEmitter.SetVelocity(new Vector2(-0.1f), new Vector2(0.1f));
            MasterEmitter.SetLife(3000);
            MasterEmitter.SetEmitArea(1300, 700);
            MasterEmitter.EmitWait = 10;
            ColorChangeIndex index = new ColorChangeIndex(500, Color.Purple, Color.Green, Color.DarkGoldenrod, Color.Red, Color.Turquoise, Color.Blue, Color.Transparent);
            MasterEmitter.SetColor(index);
        }

        private void emitter_toPlatform()
        {
            Header.Style.Color = Color.Orange;
            MasterEmitter.Position = EdgeGame.WindowSize / 2;
            MasterEmitter.Texture = ResourceManager.getTexture("fire");
            MasterEmitter.SetSize(new Vector2(100), new Vector2(200));
            MasterEmitter.SetVelocity(new Vector2(-5), new Vector2(5));
            MasterEmitter.SetRotationSpeed(-8, 8);
            MasterEmitter.BlendState = BlendState.Additive;
            MasterEmitter.SetLife(3000);
            MasterEmitter.SetEmitArea(0, 0);
            ColorChangeIndex index = new ColorChangeIndex(700, Color.Crimson, Color.OrangeRed, Color.Transparent);
            MasterEmitter.SetColor(index);
        }

        private void emitter_toGame()
        {
            Header.Style.Color = Color.CadetBlue;
            MasterEmitter.Position = EdgeGame.WindowSize / 2;
            MasterEmitter.Texture = ResourceManager.getTexture("fire");
            MasterEmitter.SetSize(new Vector2(100), new Vector2(200));
            MasterEmitter.SetVelocity(new Vector2(-5), new Vector2(5));
            MasterEmitter.SetRotationSpeed(-8, 8);
            MasterEmitter.BlendState = BlendState.Additive;
            MasterEmitter.SetLife(3000);
            MasterEmitter.SetEmitArea(0, 0);
            ColorChangeIndex index = new ColorChangeIndex(700, Color.Blue, Color.CadetBlue, Color.Transparent);
            MasterEmitter.SetColor(index);
        }

        private void platform_click(ButtonEventArgs e) { EdgeGame.AddScene(new PlatformDemo()); EdgeGame.SwitchScene("PlatformDemo"); }
        private void platform_mouseOver(ButtonEventArgs e) { emitter_toPlatform(); }
        private void game_click(ButtonEventArgs e) { EdgeGame.AddScene(new GameDemo()); EdgeGame.SwitchScene("GameDemo"); }
        private void game_mouseOver(ButtonEventArgs e) { emitter_toGame(); } 
        private void buttonMouseOff(ButtonEventArgs e) { emitter_toNormal(); } 

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
            base.Initialize();
            initEdgeGame();
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
