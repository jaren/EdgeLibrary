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

namespace EdgeLibrary.Basic
{
    //Unfinished class - base for everything that does not need to be updated
    /// <summary>
    /// The base class for most types in this library.
    /// </summary>
    public class EObject
    {
        public virtual string ID { get; set;}
        public string SceneID { get; set; }
        public string LayerID { get; set; }
    }
}
