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
    public static partial class EdgeGame
    {
        public static List<Scene> Scenes { get; private set; }
        public static Scene CurrentScene { get; private set; }

        public static void InitializeScenes()
        {
            Scenes = new List<Scene>();
        }

        //Adds a scene
        public static void AddScene(Scene scene)
        {
            DebugLogger.LogAdd("Scene Added", "ID: " + scene.ID);
            Scenes.Add(scene);
        }

        //Gets the scene with the given ID
        public static Scene GetScene(string id)
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
        public static void SwitchScene(string id)
        {
            DebugLogger.LogAdd("Scene Switched", "ID: " + id);
            Scene scene = GetScene(id);
            CurrentScene = scene;
            scene.WhenSwitched();
            Camera.Position = scene.InitialCameraPosition;
        }

        //Deletes a specific scene
        public static bool RemoveScene(string id)
        {
            DebugLogger.LogRemove("Scene Removed", "ID: " + id);
            return Scenes.Remove(GetScene(id));
        }
        public static void RemoveScene(int index)
        {
            DebugLogger.LogRemove("Scene Removed", "ID: " + Scenes[index].ID);
            Scenes.RemoveAt(index);
        }

        //Updates the selected scene
        private static void Update()
        {
            if (CurrentScene == null) { return; }
            CurrentScene.Update(GameTime);
        }

        //Draws the selected scene based on draw state
        private static void Draw()
        {
            if (CurrentScene == null) { return; }
            switch (DrawState)
            {
                case DrawState.Normal:
                    CurrentScene.Draw(GameTime, Game.SpriteBatch);
                    break;
                case DrawState.Debug:
                    CurrentScene.DrawDebug(GameTime, Game.SpriteBatch, DebugDrawColor);
                    break;
                case DrawState.Hybrid:
                    CurrentScene.Draw(GameTime, Game.SpriteBatch);
                    CurrentScene.DrawDebug(GameTime, Game.SpriteBatch, DebugDrawColor);
                    break;
            }
        }
    }
}
