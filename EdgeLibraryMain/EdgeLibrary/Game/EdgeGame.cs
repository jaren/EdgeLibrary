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
using System.Xml;
using System.Xml.Linq;

namespace EdgeLibrary
{

    public enum GameDrawState
    {
        Normal,
        Debug,
        Hybrid
    }

    public static class EdgeGame
    {
        private static GraphicsDevice graphicsDevice;
        private static GraphicsDeviceManager graphics;
        private static SpriteBatch spriteBatch;

        private static bool IsDrawing;

        private static RenderTarget2D ScreenTarget;

        public static GameDrawState GameDrawState;
        public static Color DebugDrawColor;

        public static Color ClearColor;
        public static Scene SelectedScene;

        public static List<Capability> AutoIncludedCapabilities;

        public static Effect Effect;

        public static List<Scene> Scenes { get; private set; }

        public static void Init(ContentManager c, GraphicsDevice gd, GraphicsDeviceManager gdm, SpriteBatch sb)
        {
            graphicsDevice = gd;
            graphics = gdm;
            spriteBatch = sb;

            DebugDrawColor = Color.White;

            AutoIncludedCapabilities = new List<Capability>() { new SimpleMovementCapability(), new ClampCapability()};

            ScreenTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            Scenes = new List<Scene>();
            Scenes.Add(new Scene("Main"));
            SelectedScene = mainScene();

            ResourceManager.Init(c);
            SoundManager.Init(c);
            TextureTools.Init();
            MathTools.Init();
            InputManager.Init();
            Camera.UpdateWithGame();
        }

        public static List<Element> AllElements()
        {
            List<Element> elements = new List<Element>();
            foreach (Scene scene in Scenes)
            {
                foreach(Element e in scene.elements)
                {
                    elements.Add(e);
                }
            }
            return elements;
        }

        public static Scene mainScene()
        {
            foreach (Scene Scene in Scenes)
            {
                if (Scene.ID == "Main")
                {
                    return Scene;
                }
            }

            return null;
        }

        public static Scene Scene(string ID)
        {
            foreach (Scene Scene in Scenes)
            {
                if (Scene.ID == "Main")
                {
                    return Scene;
                }
            }
            return mainScene();
        }

        public static void RemoveElement(Element e)
        {
            foreach (Scene scene in Scenes)
            {
                scene.RemoveElement(e);
            }
        }

        public static Vector2 WindowSize()
        {
            return new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        public static void AutoAdd(Element e)
        {
                SelectedScene.AddElement(e);
        }

        public static void SwitchScene(string id)
        {
            SelectedScene = Scene(id);
        }

        public static void SetWindowSize(Vector2 size)
        {
            graphics.PreferredBackBufferWidth = (int)size.X;
            graphics.PreferredBackBufferHeight = (int)size.Y;
            graphics.ApplyChanges();

            ScreenTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            Camera.UpdateWithGame();
        }

        public static Texture2D NewTexture(int width, int height)
        {
            return new Texture2D(graphicsDevice, width, height);
        }

        public static void Update(GameTime gameTime)
        {
            InputManager.Update(gameTime);

            Camera.Update(gameTime);
            FPSCounter.Update(gameTime);

            SelectedScene.Update(gameTime);

            //Checks if two elements have the same ID
            List<string> IDs = new List<string>();
            foreach (Scene scene in Scenes)
            {
                foreach (Element e in scene.elements)
                {
                    if (IDs.Contains(e.ID))
                    {
                        //There was a duplicate ID
                        throw new Exception();
                    }
                    IDs.Add(e.ID);
                }
            }
            IDs = null;
        }

        public static void Draw(GameTime gameTime)
        {
            graphicsDevice.SetRenderTarget(ScreenTarget);
            graphicsDevice.Clear(ClearColor);
            graphicsDevice.Viewport = new Viewport((int)Camera.Position.X - (int)WindowSize().X / 2, (int)Camera.Position.Y - (int)WindowSize().Y / 2, (int)WindowSize().X, (int)WindowSize().Y);

            spriteBatch.Begin();
            IsDrawing = true;
            switch (GameDrawState)
            {
                case GameDrawState.Normal:
                    SelectedScene.Draw(gameTime);
                    break;

                case GameDrawState.Debug:
                    SelectedScene.DrawDebug(gameTime);
                    break;

                case GameDrawState.Hybrid:
                    SelectedScene.Draw(gameTime);
                    SelectedScene.DrawDebug(gameTime);
                    break;
            }
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(ClearColor);
            if (Effect != null)
            {
                Effect.ApplyEffect(ScreenTarget);
            }
            spriteBatch.Begin();
            drawTexture(ScreenTarget, new Rectangle(0, 0, (int)WindowSize().X, (int)WindowSize().Y), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            IsDrawing = false;
            spriteBatch.End();
        }

        //Used mainly for elements that require a special draw mode
        public static void RestartSpriteBatch(SpriteSortMode sortMode, BlendState blendState)
        {
            if (IsDrawing)
            {
                spriteBatch.End();
                spriteBatch.Begin(sortMode, blendState);
            }
        }
        public static void RestartSpriteBatch()
        {
            if (IsDrawing)
            {
                spriteBatch.End();
                spriteBatch.Begin();
            }
        }

        public static void drawTexture(Texture2D texture, Rectangle destRect, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects spriteEffects, float layerDepth)
        {
            if (IsDrawing && texture != null)
            {
                spriteBatch.Draw(texture, destRect, sourceRectangle, color, rotation, origin, spriteEffects, layerDepth);
            }
        }

        public static void drawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects, float layerDepth)
        {
            if (IsDrawing && font != null)
            {
                spriteBatch.DrawString(font, text, position, color, rotation, origin, scale, spriteEffects, layerDepth);
            }
        }
    }
}
