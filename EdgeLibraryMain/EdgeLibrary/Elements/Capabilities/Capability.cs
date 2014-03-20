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
using System.Xml;

namespace EdgeLibrary
{
    public abstract class Capability
    {
        public string ID;
        protected bool STOPPED;

        public Capability(string id)
        {
            ID = id;
        }

        public void Stop() { STOPPED = true; }

        public void Update(GameTime gameTime, Element element) { if (!STOPPED) { updateCapability(gameTime, element); } }
        public abstract void updateCapability(GameTime gameTime, Element element);
    }
}
