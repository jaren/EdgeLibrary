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

        public SceneHandler(string sceneID)
        {
            Scenes = new List<Scene>();
            selectedScene = new Scene(sceneID);
            Scenes.Add(selectedScene);
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

        //Switches the scene
        public bool SwitchScene(Scene scene)
        {
            if (Scenes.Contains(scene))
            {
                selectedScene = scene;
                return true;
            }
            return false;
        }
        public bool SwitchScene(string id)
        {
            return SwitchScene(GetScene(id));
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

        //Updates the selected scene
        public void Update(GameTime gameTime)
        {
            selectedScene.Update(gameTime);
        }

        //Draws the selected scene based on draw state
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, DrawState drawState)
        {
            spriteBatch.Begin();

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

            spriteBatch.End();
        }
    }
}
