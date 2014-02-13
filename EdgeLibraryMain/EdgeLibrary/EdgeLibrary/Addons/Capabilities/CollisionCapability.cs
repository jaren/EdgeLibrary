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
    public class CollisionCapability : Capability
    {
        public CollisionCapability() : base("Collision")
        {
        }

        public override void Update(GameTime gameTime, Element element)
        {
        }

        public override Capability NewInstance()
        {
            return new CollisionCapability();
        }
    }
}
