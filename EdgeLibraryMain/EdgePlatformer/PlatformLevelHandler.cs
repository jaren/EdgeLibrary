using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeLibrary.Platform
{
    public class PlatformLevelHandler
    {
        public List<PlatformLevel> Levels;

        public PlatformLevelHandler()
        {
            Levels = new List<PlatformLevel>();
        }

        public void LevelFromImage(string levelID, string path)
        {
            //Load a level from an image
        }
    }

    public class PlatformLevel
    {
        string ID;
    }
}
