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
    //The equivalent of "EdgeGame"
    public class PlatformGame : Scene
    {
        public PlatformLevelHandler LevelHandler;

        public PlatformGame(string id, PlatformLevel level) : base(id)
        {
            LevelHandler = new PlatformLevelHandler(level);
        }

        public override void AddElement(Element element) { }
        public override Element Element(string id) { return null; }
        public override bool RemoveElement(Element element) { return false; }
        public override bool RemoveElement(string id) { return false; }
        public override void DrawDebug(GameTime gameTime) { }

        public void AddLevel(PlatformLevel level)
        {
            LevelHandler.AddLevel(level);
        }

        public bool RemoveLevel(string id)
        {
            return LevelHandler.RemoveLevel(id);
        }

        public bool SelectLevel(string id)
        {
            return LevelHandler.SelectLevel(id);
        }

        public override void Update(GameTime gameTime)
        {
            LevelHandler.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            LevelHandler.Draw(gameTime);
        }
    }
}
