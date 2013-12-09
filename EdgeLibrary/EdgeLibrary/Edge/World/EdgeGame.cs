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

namespace EdgeLibrary.Edge
{
    public class EdgeGame
    {
        #region VARIABLES
        private string normalXML = "Settings.xml";

        private ContentManager Content;
        private SpriteBatch spriteBatch;
        private GraphicsDeviceManager graphics;
        private GraphicsDevice graphicsDevice;

        private Color clearColor = Color.Black;

        private List<EScene> scenes;
        private int selectedSceneIndex;

        public EData edgeData;

        private MouseState previousMouseState;
        public delegate void EMouseEvent(EUpdateArgs e);
        public event EMouseEvent MouseClick;
        public event EMouseEvent MouseRelease;
        public event EMouseEvent MouseMove;

        private ESettingsHandler settingsLoader;
        #endregion

        public EdgeGame(ContentManager eContent, SpriteBatch eSpriteBatch, GraphicsDeviceManager eGraphics, GraphicsDevice eGraphicsDevice)
        {
            Content = eContent;
            spriteBatch = eSpriteBatch;
            graphics = eGraphics;
            graphicsDevice = eGraphicsDevice;

            previousMouseState = Mouse.GetState();

            scenes = new List<EScene>();

            edgeData = new EData();

            selectedSceneIndex = 0;

            MouseClick += new EMouseEvent(nullHandler);
            MouseRelease += new EMouseEvent(nullHandler);
            MouseMove += new EMouseEvent(nullHandler);
        }

        #region INIT
        public void Init()
        {
            settingsLoader = new ESettingsHandler(normalXML);
        }

        public void Init(string xmlPath)
        {
            settingsLoader = new ESettingsHandler(xmlPath);
        }

        public void setWindowWidth(int width) { graphics.PreferredBackBufferWidth = width; graphics.ApplyChanges(); }
        public void setWindowHeight(int height) { graphics.PreferredBackBufferHeight = height; graphics.ApplyChanges(); }
        private void nullHandler(EUpdateArgs e) { }
        #endregion

        #region LOAD
        public void LoadContent()
        {
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
            scenes[selectedSceneIndex].Update(updateArgs);

            if (updateArgs.mouseState.X != previousMouseState.X || updateArgs.mouseState.Y != previousMouseState.Y) { MouseMove(updateArgs); }
            if (updateArgs.mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released) { MouseClick(updateArgs); }
            else if (updateArgs.mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed) { MouseRelease(updateArgs); }

            previousMouseState = updateArgs.mouseState;
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
            scenes[selectedSceneIndex].Draw(spriteBatch, gameTime);
            spriteBatch.End();
        }
        #endregion

        #region EXTRAS
        private Color getColorFromString(string colorString)
        {
            var typeProperty = typeof(Color).GetProperty(colorString);
            if (typeProperty != null)
            {
                return (Color)typeProperty.GetValue(null, null);
            }
            else
            {
                return Color.Black;
            }
        }

        public static double RadiansToDegrees(float radians) { return radians * (180 / Math.PI); }
        public static double DegreesToRadians(float degrees) { return degrees / (180 / Math.PI); }

        #endregion
    }
}
