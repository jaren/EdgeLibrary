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

        public Capability(string id)
        {
            ID = id;
        }

        public abstract void Update(GameTime gameTime, Element element);

        //This is so each element can have a new list of capabilities, not just references to the old ones
        public abstract Capability NewInstance(Element e);
    }
}
