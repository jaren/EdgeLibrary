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

    public static class EdgeGame
    {
        private static GraphicsDevice graphicsDevice;
        private static GraphicsDeviceManager graphics;
        private static SpriteBatch spriteBatch;

        private static RenderTarget2D ScreenTarget;

        public static Color ClearColor;
        public static bool AutomaticallyAddElementsToGame;
        public static Scene SceneToAddTo;

        public static List<Effect> Effects;

        public static List<Scene> Scenes { get; private set; }
        public static string selectedScene;

        public static void Init(ContentManager c, GraphicsDevice gd, GraphicsDeviceManager gdm, SpriteBatch sb)
        {
            graphicsDevice = gd;
            graphics = gdm;
            spriteBatch = sb;

            Effects = new List<Effect>();

            AutomaticallyAddElementsToGame = true;

            ScreenTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            Scenes = new List<Scene>();
            Scenes.Add(new Scene("Main"));
            selectedScene = mainScene().ID;

            ResourceManager.Init(c);
            SoundManager.Init(c);
            Camera.UpdateWithGame();
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

        public static Vector2 WindowSize()
        {
            return new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        public static void AutoAdd(Element e)
        {
            if (AutomaticallyAddElementsToGame)
            {
                Scene(selectedScene).AddElement(e);
            }
        }

        public static void SetWindowSize(Vector2 size)
        {
            graphics.PreferredBackBufferWidth = (int)size.X;
            graphics.PreferredBackBufferHeight = (int)size.Y;
            graphics.ApplyChanges();

            ScreenTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            Camera.UpdateWithGame();
        }

        public static void Update(GameTime gameTime)
        {
            InputManager.Update();

            Camera.Update(gameTime);

            Scene(selectedScene).Update(gameTime);
        }

        public static void Draw(GameTime gameTime)
        {
            graphicsDevice.SetRenderTarget(ScreenTarget);
            graphicsDevice.Clear(ClearColor);
            graphicsDevice.Viewport = new Viewport((int)Camera.Position.X - (int)WindowSize().X / 2, (int)Camera.Position.Y - (int)WindowSize().Y / 2, (int)WindowSize().X, (int)WindowSize().Y);

            spriteBatch.Begin();
            Scene(selectedScene).Draw(gameTime);
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(ClearColor);
            Texture2D screen = (Texture2D)ScreenTarget;
            for (int i = 0; i < Effects.Count; i++)
            {
                Effects[i].ApplyEffect(screen);
            }
            spriteBatch.Begin();
            drawTexture(screen, new Rectangle(0, 0, (int)WindowSize().X, (int)WindowSize().Y), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        public static void drawTexture(Texture2D texture, Rectangle destRect, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects spriteEffects, float layerDepth)
        {
            spriteBatch.Draw(texture, destRect, sourceRectangle, color, rotation, origin, spriteEffects, layerDepth);
        }

        public static void drawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects, float layerDepth)
        {
            spriteBatch.DrawString(font, text, position, color, rotation, origin, scale, spriteEffects, layerDepth);
        }
    }
}
