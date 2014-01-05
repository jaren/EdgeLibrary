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

    public class EdgeGame
    {
        #region VARIABLES
        private ContentManager Content;
        private SpriteBatch spriteBatch;
        private GraphicsDeviceManager graphics;
        private GraphicsDevice graphicsDevice;

        private Color clearColor = Color.Black;
        public Color DebugDrawColor = Color.White;

        private List<EScene> scenes;
        private int selectedSceneIndex;

        public EData edgeData;

        public EdgeGameDrawTypes DrawType;

        private MouseState previousMouseState;
        public delegate void EMouseEvent(EUpdateArgs e);
        public event EMouseEvent MouseClick;
        public event EMouseEvent MouseRelease;
        public event EMouseEvent MouseMove;

        private ESettingsHandler settingsLoader;

        private Texture2D pixel;
        #endregion

        public EdgeGame(ContentManager eContent, SpriteBatch eSpriteBatch, GraphicsDeviceManager eGraphics, GraphicsDevice eGraphicsDevice)
        {
            DrawType = EdgeGameDrawTypes.Normal;

            Content = eContent;
            spriteBatch = eSpriteBatch;
            graphics = eGraphics;
            graphicsDevice = eGraphicsDevice;

            previousMouseState = Mouse.GetState();

            scenes = new List<EScene>();

            edgeData = new EData();

            selectedSceneIndex = 0;
        }

        #region INIT
        public void Init()
        {
            EMath.Init(graphicsDevice);
        }

        public void InitWithXML(string xmlPath)
        {
            settingsLoader = new ESettingsHandler(xmlPath);
            Init();
        }

        public void setWindowWidth(int width) { graphics.PreferredBackBufferWidth = width; graphics.ApplyChanges(); }
        public void setWindowHeight(int height) { graphics.PreferredBackBufferHeight = height; graphics.ApplyChanges(); }
        #endregion

        #region LOAD
        public void LoadContent(Texture2D pixelTexture)
        {
            pixel = pixelTexture;
        }

        public void LoadTexture(string texturePath, string textureName)
        {
            edgeData.addTexture(textureName, Content.Load<Texture2D>(texturePath));
        }

        public void LoadFont(string fontPath, string fontName)
        {
            edgeData.addFont(fontName, Content.Load<SpriteFont>(fontPath));
        }

        public void LoadSong(string songPath, string songName)
        {
            edgeData.addSong(songName, Content.Load<Song>(songPath));
        }

        public void LoadSound(string soundPath, string soundName)
        {
            edgeData.addSound(soundName, Content.Load<SoundEffect>(soundPath));
        }

        //Currently Unused
        public void UnloadContent() { }
        #endregion

        #region UPDATE
        public void Update(EUpdateArgs updateArgs)
        {
            try
            {
                scenes[selectedSceneIndex].Update(updateArgs);
                }
                catch{}

            if ((updateArgs.mouseState.X != previousMouseState.X || updateArgs.mouseState.Y != previousMouseState.Y) && MouseMove != null) { MouseMove(updateArgs); }
            if ((updateArgs.mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released) && MouseClick != null) { MouseClick(updateArgs); }
            else if ((updateArgs.mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed) && MouseRelease != null) { MouseRelease(updateArgs); }

            previousMouseState = updateArgs.mouseState;
        }

        //NOTE: For these, the element is not actually set to "null". You must do it manually.
        public void RemoveElement(EElement eElement)
        {
            foreach (EScene scene in scenes)
            {
                scene.RemoveElement(eElement);
            }
        }

        //NOTE: For these, the element is not actually set to "null". You must do it manually.
        public void RemoveObject(EObject eObject)
        {
            foreach (EScene scene in scenes)
            {
                scene.RemoveObject(eObject);
            }
        }

        public string getSetting(string settingName)
        {
            return settingsLoader.getSetting(settingName);
        }

        public void playSong(string songName)
        {
            edgeData.playSong(songName);
        }

        public void playSound(string soundName)
        {
            edgeData.playSound(soundName);
        }

        public void addScene(EScene scene)
        {
            scene.setEData(edgeData);
            scene.mainGame = this;
            scenes.Add(scene);
        }

        public bool switchScene(string sceneName)
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
        #endregion

        #region DRAW
        public void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(clearColor);
            spriteBatch.Begin();
            try
            {
                scenes[selectedSceneIndex].DrawType = DrawType;
                scenes[selectedSceneIndex].DebugDrawColor = DebugDrawColor;
                scenes[selectedSceneIndex].Draw(spriteBatch, gameTime);
            }
            catch { }
            spriteBatch.End();
        }
        #endregion

    }
}
