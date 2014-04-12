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

namespace EdgeLibrary
{
    public class SceneHandler
    {
        public List<Scene> Scenes { get; protected set; }
        public Scene CurrentScene { get { return selectedScene; } }
        private Scene selectedScene;
        public Color DebugDrawColor;

        public SceneHandler()
        {
            Scenes = new List<Scene>();
        }

        //Adds a scene
        public void AddScene(Scene scene)
        {
            DebugLogger.LogAdd("Scene Added", "ID: " + scene.ID);
            Scenes.Add(scene);
        }

        //Gets the scene with the given ID
        public Scene GetScene(string id)
        {
            foreach (Scene scene in Scenes)
            {
                if (scene.ID == id)
                {
                    return scene;
                }
            }
            return null;
        }

        //Switches the scene and adds it to the game if it isn't already added
        public void SwitchScene(Scene scene)
        {
            if (Scenes.Contains(scene))
            {
                selectedScene = scene;
                scene.WhenSwitched();
                EdgeGame.Instance.Camera.Position = scene.InitialCameraPosition;
            }
            else
            {
                Scenes.Add(scene);
                SwitchScene(scene);
            }
        }
        public void SwitchScene(string id)
        {
            SwitchScene(GetScene(id));
        }

        //Deletes a specific scene
        public bool RemoveScene(Scene scene)
        {
            DebugLogger.LogRemove("Scene Removed", "ID: " + scene.ID);
            return Scenes.Remove(scene);
        }
        public bool RemoveScene(string id)
        {
            return Scenes.Remove(GetScene(id));
        }
        public void RemoveScene(int index)
        {
            Scenes.RemoveAt(index);
        }

        //Updates the selected scene
        public void Update(GameTime gameTime)
        {
            if (selectedScene == null) { return; }
            selectedScene.Update(gameTime);
        }

        //Draws the selected scene based on draw state
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, DrawState drawState)
        {
            if (selectedScene == null) { return; }
            switch (drawState)
            {
                case DrawState.Normal:
                    selectedScene.Draw(gameTime, spriteBatch);
                    break;
                case DrawState.Debug:
                    selectedScene.DrawDebug(gameTime, spriteBatch, DebugDrawColor);
                    break;
                case DrawState.Hybrid:
                    selectedScene.Draw(gameTime, spriteBatch);
                    selectedScene.DrawDebug(gameTime, spriteBatch, DebugDrawColor);
                    break;
            }
        }
    }
}
