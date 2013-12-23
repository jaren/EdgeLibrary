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

namespace EdgeLibrary.Basic
{
    public class EAnimatedSprite : ESprite
    {
        //Eventually will be a normal sprite with animation capability

        public EAnimatedSprite() : base("", Vector2.Zero, 0, 0) { }
    }
}
