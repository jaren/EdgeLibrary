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
    public enum EShapeTypes
    {
        circle,
        rectangle
    };

    public class EShape
    {
        //Check for collision here
        public static bool ShapeCollidesWith(Vector2 position1, EShape shape1, Vector2 position2, EShape shape2)
        {
            return false;
        }
    }
}
