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
    public class ECollisionBody : EObject
    {
        public List<string> CollidesWithIDs { get; set; }
        public Vector2 Position { get; set; }
        public EShape Shape { get; set; }

        public ECollisionBody(Vector2 position, EShape shape, string collisionID, params string[] collidesWithIDs) 
        {
            Position = position;
            Shape = shape;
            CollidesWithIDs = new List<string>(collidesWithIDs);
            ID = collisionID;
        }

        public bool CheckForCollide(ECollisionBody body)
        {
            return EShape.ShapeCollidesWith(Position, Shape, body.Position, body.Shape);
        }
    }
}
