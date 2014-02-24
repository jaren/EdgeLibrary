﻿using System;
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
    //Any "character" such as players, enemies, bosses, etc.
    public class PlatformCharacter : PlatformSprite
    {
        public PlatformCharacter(string id, string eTextureName, Vector2 ePosition) : base(id, eTextureName, ePosition) { }

        public void Jump()
        {
        }
    }
}
