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
        public bool collidesWithAll { get; set; }
        public List<string> CollidesWithIDs { get; set; }
        public Vector2 Position { get { return Shape.CenterPosition; } set { Shape.CenterPosition = value; } }
        public EShape Shape { get; set; }

        public ECollisionBody(EShape shape, string collisionID)
        {
            Shape = shape;
            ID = collisionID;
            collidesWithAll = true;
        }

        public ECollisionBody(EShape shape, string collisionID, List<string> collidesWithIDs) : this(shape, collisionID)
        {
            CollidesWithIDs = new List<string>(collidesWithIDs);
            collidesWithAll = false;
        }

        public ECollisionBody(EShape shape, string collisionID, params string[] collidesWithIDs) : this(shape, collisionID)
        {
            CollidesWithIDs = new List<string>(collidesWithIDs);
            collidesWithAll = false;
        }

        public bool CheckForCollide(ECollisionBody body)
        {
            if (CollidesWithIDs.Contains(body.ID) || collidesWithAll)
            {
                return Shape.CollidesWith(body.Shape);
            }
            return false;
        }
    }
}
