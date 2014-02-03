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

namespace EdgeLibrary
{
    public enum EdgeGameDrawTypes
    {
        Normal,
        Debug,
        Hybrid
    }

    public static class EdgeGame
    {
        #region VARIABLES
        public static ContentManager Content;
        public static SpriteBatch spriteBatch;
        public static GraphicsDeviceManager graphics;
        public static GraphicsDevice graphicsDevice;

        public static string ContentRootDirectory;
        public static EdgeGameDrawTypes DrawType;
        public static Color ClearColor;
        public static Color DebugDrawColor;


        public static Vector2 WindowSize
        {
            get { return new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight); }
            set {  }
        }
        

        private static List<Scene> scenes;
        private static int selectedSceneIndex;

        private static MouseState previousMouseState;
        public delegate void EMouseEvent(UpdateArgs e);
        public static event EMouseEvent MouseClick;
        public static event EMouseEvent MouseRelease;
        public static event EMouseEvent MouseMove;

        public delegate void EdgeGameUpdateEvent(UpdateArgs e);
        public static event EdgeGameUpdateEvent UpdateEvent;
        #endregion

        public static void Init(ContentManager eContent, SpriteBatch SpriteBatch, GraphicsDeviceManager eGraphics, GraphicsDevice eGraphicsDevice)
        {
            Content = eContent;
            spriteBatch = SpriteBatch;
            graphics = eGraphics;
            graphicsDevice = eGraphicsDevice;

            previousMouseState = Mouse.GetState();

            scenes = new List<Scene>();

            selectedSceneIndex = 0;

            ClearColor = Color.Black;
            DebugDrawColor = Color.White;
            DrawType = EdgeGameDrawTypes.Normal;

            MathGenerator.Init();
            ResourceData.Init();
            ContentRootDirectory = Content.RootDirectory;
        }

        public static void setWindowWidth(int width) { graphics.PreferredBackBufferWidth = width; graphics.ApplyChanges(); }
        public static void setWindowHeight(int height) { graphics.PreferredBackBufferHeight = height; graphics.ApplyChanges(); }

        public static Layer GetLayerFromObject(Object Object)
        {
            return getScene(Object.SceneID).getLayer(Object.LayerID);
        }

        #region LOAD
        //Currently unused
        public static void LoadContent() { }

        public static void LoadTexture(string texturePath, string textureName)
        {
            ResourceData.addTexture(textureName, Content.Load<Texture2D>(texturePath));
        }

        public static void LoadTexture(string path)
        {
            ResourceData.addTexture(MathGenerator.LastPortionOfPath(path), Content.Load<Texture2D>(path));
        }

        public static void LoadTextureFromSpritesheet(string spritesheetpath, string xmlpath)
        {
            ResourceData.addTexture(spritesheetpath, Content.Load<Texture2D>(spritesheetpath));
            Dictionary<string, Texture2D> textures = TextureGenerator.SplitSpritesheet(spritesheetpath, xmlpath);

            foreach (KeyValuePair<string, Texture2D> texture in textures)
            {
                ResourceData.addTexture(texture.Key, texture.Value);
            }
        }

        public static void LoadFont(string fontPath, string fontName)
        {
            ResourceData.addFont(fontName, Content.Load<SpriteFont>(fontPath));
        }

        public static void LoadFont(string path)
        {
            ResourceData.addFont(MathGenerator.LastPortionOfPath(path), Content.Load<SpriteFont>(path));
        }

        public static void LoadSong(string songPath, string songName)
        {
            ResourceData.addSong(songName, Content.Load<Song>(songPath));
        }

        public static void LoadSong(string path)
        {
            ResourceData.addSong(MathGenerator.LastPortionOfPath(path), Content.Load<Song>(path));
        }

        public static void LoadSound(string soundPath, string soundName)
        {
            ResourceData.addSound(soundName, Content.Load<SoundEffect>(soundPath));
        }

        public static void LoadSound(string path)
        {
            ResourceData.addSound(MathGenerator.LastPortionOfPath(path), Content.Load<SoundEffect>(path));
        }

        //Currently Unused
        public static void UnloadContent() { }
        #endregion

        #region UPDATE
        public static void Update(GameTime gameTime)
        {
            UpdateArgs updateArgs = new UpdateArgs(gameTime, Keyboard.GetState(), Mouse.GetState());

            if (scenes.Count != 0)
            {
                scenes[selectedSceneIndex].Update(updateArgs);
                }

            if ((updateArgs.mouseState.X != previousMouseState.X || updateArgs.mouseState.Y != previousMouseState.Y) && MouseMove != null) { MouseMove(updateArgs); }
            if ((updateArgs.mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released) && MouseClick != null) { MouseClick(updateArgs); }
            else if ((updateArgs.mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed) && MouseRelease != null) { MouseRelease(updateArgs); }

            previousMouseState = updateArgs.mouseState;

            if (UpdateEvent != null)
            {
                UpdateEvent(updateArgs);
            }
        }

        //NOTE: For these, the element is not actually set to "null". You must do it manually.
        public static void RemovElement(Element Element)
        {
            foreach (Scene scene in scenes)
            {
                scene.RemovElement(Element);
            }
        }

        public static Texture2D GetTexture(string texture)
        {
            return ResourceData.getTexture(texture);
        }

        //NOTE: For these, the element is not actually set to "null". You must do it manually.
        public static void RemovObject(Object Object)
        {
            foreach (Scene scene in scenes)
            {
                scene.RemovObject(Object);
            }
        }

        public static void playSong(string songName)
        {
            ResourceData.playSong(songName);
        }

        public static void playSound(string soundName)
        {
            ResourceData.playSound(soundName);
        }

        public static void addScene(Scene scene)
        {
            scenes.Add(scene);
        }

        public static bool switchScene(string sceneName)
        {
            for (int i = 0; i < scenes.Count; i++)
            {
                if (scenes[i].ID == sceneName)
                {
                    selectedSceneIndex = i;
                    return true;
                }
            }
            return false;
        }

        public static Scene getScene(string sceneName)
        {
            foreach (Scene scene in scenes)
            {
                if (scene.ID == sceneName)
                {
                    return scene;
                }
            }
            return null;
        }
        #endregion

        #region DRAW
        public static void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(ClearColor);
            spriteBatch.Begin();
            if (scenes.Count != 0)
            {
                scenes[selectedSceneIndex].Draw(spriteBatch, gameTime);
            }
            spriteBatch.End();
        }
        #endregion

    }
}
