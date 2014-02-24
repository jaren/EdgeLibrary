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
using EdgeLibrary;

namespace EdgeLibrary.Platform
{
    //The equivalent of the list of scenes
    public class PlatformLevelHandler
    {
        public List<PlatformLevel> Levels;
        public PlatformLevel selectedLevel;

        public PlatformLevelHandler(PlatformLevel level)
        {
            Levels = new List<PlatformLevel>() { level };
            selectedLevel = level;
        }

        public void LevelFromImage(string levelID, string path)
        {
            //Load a level from an image
        }

        public void AddLevel(PlatformLevel level)
        {
            Levels.Add(level);
        }

        public bool RemoveLevel(string id)
        {
            foreach (PlatformLevel level in Levels)
            {
                if (level.ID == id)
                {
                    Levels.Remove(level);
                    return true;
                }
            }
            return false;
        }

        public bool SelectLevel(string id)
        {
            foreach (PlatformLevel level in Levels)
            {
                if (level.ID == id)
                {
                    selectedLevel = level;
                    return true;
                }
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            selectedLevel.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            selectedLevel.Draw(gameTime);
        }
    }
}
