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
    //Serves as the collision body for an element, can check if it is colliding with another element
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

        //MUST BE CALLED AFTER SPRITE HAS BEEN ADDED TO SCENE IF THE SPRITE IS INITIALIZED WITHOUT WIDTH AND HEIGHT AND "SCALE COLLISION BODY" IS FALSE!
        public static ECollisionBody BodyWithSpriteAndIDs(EShapeTypes shapeType, ESprite sprite, string collisionID, List<string> collidesWithIDs)
        {
            switch(shapeType)
            {
                case EShapeTypes.circle:
                    return new ECollisionBody(new EShapeCircle(sprite.Position, (sprite.Width + sprite.Height) / 4), collisionID, collidesWithIDs); //It's the average over 2, because the average of width+height is the diameter and this is the radius
                    break;
                case EShapeTypes.rectangle:
                    return new ECollisionBody(new EShapeRectangle(sprite.Position, sprite.Width, sprite.Height), collisionID, collidesWithIDs);
                    break;
            }
            return null;
        }

        public static ECollisionBody BodyWithSprite(EShapeTypes shapeType, ESprite sprite, string collisionID, params string[] collidesWithIDs)
        {
            return ECollisionBody.BodyWithSpriteAndIDs(shapeType, sprite, collisionID, new List<string>(collidesWithIDs));
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
