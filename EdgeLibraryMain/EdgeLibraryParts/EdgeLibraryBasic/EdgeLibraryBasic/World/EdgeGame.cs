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

namespace EdgeLibrary.Basic
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
        

        private static List<EScene> scenes;
        private static int selectedSceneIndex;

        private static MouseState previousMouseState;
        public delegate void EMouseEvent(EUpdateArgs e);
        public static event EMouseEvent MouseClick;
        public static event EMouseEvent MouseRelease;
        public static event EMouseEvent MouseMove;

        public delegate void EdgeGameUpdateEvent(EUpdateArgs e);
        public static event EdgeGameUpdateEvent UpdateEvent;
        #endregion

        public static void Init(ContentManager eContent, SpriteBatch eSpriteBatch, GraphicsDeviceManager eGraphics, GraphicsDevice eGraphicsDevice)
        {
            Content = eContent;
            spriteBatch = eSpriteBatch;
            graphics = eGraphics;
            graphicsDevice = eGraphicsDevice;

            previousMouseState = Mouse.GetState();

            scenes = new List<EScene>();

            selectedSceneIndex = 0;

            ClearColor = Color.Black;
            DebugDrawColor = Color.White;
            DrawType = EdgeGameDrawTypes.Normal;

            EMath.Init();
            EData.Init();
            ContentRootDirectory = Content.RootDirectory;
        }

        public static void setWindowWidth(int width) { graphics.PreferredBackBufferWidth = width; graphics.ApplyChanges(); }
        public static void setWindowHeight(int height) { graphics.PreferredBackBufferHeight = height; graphics.ApplyChanges(); }

        public static ELayer GetLayerFromObject(EObject eobject)
        {
            return getScene(eobject.SceneID).getLayer(eobject.LayerID);
        }

        #region LOAD
        //Currently unused
        public static void LoadContent() { }

        public static void LoadTexture(string texturePath, string textureName)
        {
            EData.addTexture(textureName, Content.Load<Texture2D>(texturePath));
        }

        public static void LoadTexture(string path)
        {
            EData.addTexture(EMath.LastPortionOfPath(path), Content.Load<Texture2D>(path));
        }

        public static void LoadTextureFromSpritesheet(string spritesheetpath, string xmlpath)
        {
            EData.addTexture(spritesheetpath, Content.Load<Texture2D>(spritesheetpath));
            Dictionary<string, Texture2D> textures = EMath.SplitSpritesheet(spritesheetpath, xmlpath);

            foreach (KeyValuePair<string, Texture2D> texture in textures)
            {
                EData.addTexture(texture.Key, texture.Value);
            }
        }

        public static void LoadFont(string fontPath, string fontName)
        {
            EData.addFont(fontName, Content.Load<SpriteFont>(fontPath));
        }

        public static void LoadFont(string path)
        {
            EData.addFont(EMath.LastPortionOfPath(path), Content.Load<SpriteFont>(path));
        }

        public static void LoadSong(string songPath, string songName)
        {
            EData.addSong(songName, Content.Load<Song>(songPath));
        }

        public static void LoadSong(string path)
        {
            EData.addSong(EMath.LastPortionOfPath(path), Content.Load<Song>(path));
        }

        public static void LoadSound(string soundPath, string soundName)
        {
            EData.addSound(soundName, Content.Load<SoundEffect>(soundPath));
        }

        public static void LoadSound(string path)
        {
            EData.addSound(EMath.LastPortionOfPath(path), Content.Load<SoundEffect>(path));
        }

        //Currently Unused
        public static void UnloadContent() { }
        #endregion

        #region UPDATE
        public static void Update(GameTime gameTime)
        {
            EUpdateArgs updateArgs = new EUpdateArgs(gameTime, Keyboard.GetState(), Mouse.GetState());

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
        public static void RemoveElement(EElement eElement)
        {
            foreach (EScene scene in scenes)
            {
                scene.RemoveElement(eElement);
            }
        }

        public static Texture2D GetTexture(string texture)
        {
            return EData.getTexture(texture);
        }

        //NOTE: For these, the element is not actually set to "null". You must do it manually.
        public static void RemoveObject(EObject eObject)
        {
            foreach (EScene scene in scenes)
            {
                scene.RemoveObject(eObject);
            }
        }

        public static void playSong(string songName)
        {
            EData.playSong(songName);
        }

        public static void playSound(string soundName)
        {
            EData.playSound(soundName);
        }

        public static void addScene(EScene scene)
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

        public static EScene getScene(string sceneName)
        {
            foreach (EScene scene in scenes)
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
