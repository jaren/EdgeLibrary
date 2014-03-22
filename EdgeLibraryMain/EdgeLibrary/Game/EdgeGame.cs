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

        public static float GameTimeTickRate = 100;
        private static double gameTimeElapsedTick = 0;

        private static bool IsDrawing;

        private static RenderTarget2D ScreenTarget;

        public static string DebugLoggerPath { get { return string.Empty; } set { DebugLogger.Init(value); } }

        public static GameDrawState GameDrawState;
        public static Color DebugDrawColor;

        public static Color ClearColor;
        public static Scene SelectedScene { get; private set; }

        public static Effect Effect;

        public static Vector2 WindowSize { get { return getWindowSize(); } set { SetWindowSize(value); } }

        public static List<Scene> Scenes { get; private set; }

        public delegate void UpdateEvent(GameTime gameTime);
        public static event UpdateEvent update;
        public static event UpdateEvent draw;

        public static void Init(ContentManager c, GraphicsDevice gd, GraphicsDeviceManager gdm, SpriteBatch sb)
        {
            graphicsDevice = gd;
            graphics = gdm;
            spriteBatch = sb;

            DebugDrawColor = Color.White;

            ScreenTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            Scenes = new List<Scene>();
            Scenes.Add(new Scene("Main"));
            SelectedScene = MainScene();

            ResourceManager.Init(c);
            SoundManager.Init(c);
            TextureTools.Init();
            MathTools.Init();
            RandomTools.Init();
            InputManager.Init();
            Camera.UpdateWithGame();
        }

        public static Scene MainScene()
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
                if (Scene.ID == ID)
                {
                    return Scene;
                }
            }
            return MainScene();
        }

        public static void AddScene(Scene scene)
        {
            Scenes.Add(scene);
            DebugLogger.LogAdd("Scene", "ID:" + scene.ID);
        }

        public static bool RemoveScene(string id)
        {
            foreach (Scene scene in Scenes)
            {
                if (scene.ID == id)
                {
                    Scenes.Remove(scene);
                    return true;
                }
            }
            return false;
        }

        public static bool RemoveScene(Scene scene)
        {
            if (Scenes.Remove(scene))
            {
                DebugLogger.LogRemove("Scene", "ID: " + scene.ID);
            }
            return false;
        }


        public static bool RemoveElement(Element e)
        {
            bool removed = false;
            foreach (Scene scene in Scenes)
            {
                if (scene.RemoveElement(e))
                {
                    removed = true;
                    DebugLogger.LogRemove("Element", "ID: " + e.ID, "Type: " + e.GetType());
                }
            }
            return removed;
        }

        private static Vector2 getWindowSize()
        {
            return new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        public static void SwitchScene(string id)
        {
            SwitchScene(Scene(id));
        }

        public static void SwitchScene(Scene scene)
        {
            SelectedScene = scene;
            DebugLogger.LogEvent("Switch Scene", "ID:" + scene.ID);
        }

        private static void SetWindowSize(Vector2 size)
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

            if (update != null)
            {
                update(gameTime);
            }

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
                        throw new DuplicateWaitObjectException();
                    }
                    IDs.Add(e.ID);
                }
            }
            IDs = null;

            gameTimeElapsedTick += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (gameTimeElapsedTick >= GameTimeTickRate)
            {
                DebugLogger.LogEvent("Game Running... running for " + gameTime.TotalGameTime.ToString());
                gameTimeElapsedTick = 0;
            }

            if (gameTime.IsRunningSlowly)
            {
                DebugLogger.LogError("The game is running slowly. FPS:" + FPSCounter.FPS);
            }
        }

        public static void Draw(GameTime gameTime)
        {
            FPSCounter.Update(gameTime);
            graphicsDevice.SetRenderTarget(ScreenTarget);
            graphicsDevice.Clear(ClearColor);
            graphicsDevice.Viewport = new Viewport((int)Camera.Position.X - (int)WindowSize.X / 2, (int)Camera.Position.Y - (int)WindowSize.Y / 2, (int)WindowSize.X, (int)WindowSize.Y);

            if (draw != null)
            {
                draw(gameTime);
            }

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
            spriteBatch.Begin();
            drawTexture(ScreenTarget, Vector2.Zero, null, Color.White, new Vector2(WindowSize.X/ScreenTarget.Width, WindowSize.Y/ScreenTarget.Height), 0, Vector2.Zero, SpriteEffects.None);
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

        public static void drawTexture(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, Vector2 scale, float rotation, Vector2 origin, SpriteEffects spriteEffects)
        {
            if (IsDrawing && texture != null)
            {
                spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, spriteEffects, 0);
            }
        }

        public static void drawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects)
        {
            if (IsDrawing && font != null)
            {
                spriteBatch.DrawString(font, text, position, color, rotation, origin, scale, spriteEffects, 1);
            }
        }
    }
}
