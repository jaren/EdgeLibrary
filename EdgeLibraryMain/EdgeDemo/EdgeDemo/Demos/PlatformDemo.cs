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
using EdgeLibrary.Platform;

namespace EdgeDemo
{
    public class PlatformDemo : PlatformLevel
    {
        PlatformStatic box;
        PlatformCharacter character;

        public PlatformDemo() : base("PlatformDemo", new Vector2(0, -1))
        {
            ResourceManager.LoadTexture("Wood Background");
            Background = ResourceManager.textureFromString("Wood Background");
        }

        public void Init()
        {
            DebugPanel panel = new DebugPanel("CourierNew-10", Vector2.Zero, Color.White);

            CreateScreenBox();
            box = new PlatformStatic("Pixel", new Vector2(500, 400));
            box.Scale = new Vector2(200, 50);
            box.Style.Color = Color.White;

            character = new PlatformCharacter("Pixel", new Vector2(550, 100));
            character.Scale = new Vector2(50, 50);
            character.Style.Color = Color.White;
        }
    }
}
