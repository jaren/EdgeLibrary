using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EdgeLibrary
{
    //Serves as the collision body for an element, can check if it is colliding with another element
    public class CollisionBody
    {
        public bool collidesWithAll { get; set; }
        public List<string> CollidesWithIDs { get; set; }
        public Vector2 Position { get { return CollisionShape.CenterPosition; } set { CollisionShape.CenterPosition = value; } }
        public Shape CollisionShape { get; set; }
        public string ID { get; set; }

        public CollisionBody(Shape shape, string collisionID)
        {
            CollisionShape = shape;
            ID = collisionID;
            collidesWithAll = true;
        }

        public CollisionBody(Shape shape, string collisionID, List<string> collidesWithIDs)
            : this(shape, collisionID)
        {
            CollidesWithIDs = new List<string>(collidesWithIDs);
            collidesWithAll = false;
        }

        public CollisionBody(Shape shape, string collisionID, params string[] collidesWithIDs)
            : this(shape, collisionID)
        {
            CollidesWithIDs = new List<string>(collidesWithIDs);
            collidesWithAll = false;
        }

        public static CollisionBody BodyWithSpriteAndIDs(ShapeTypes shapeType, Sprite sprite, string collisionID, List<string> collidesWithIDs)
        {
            switch (shapeType)
            {
                case ShapeTypes.circle:
                    return new CollisionBody(new ShapeCircle(sprite.Position, (sprite.Width + sprite.Height) / 4), collisionID, collidesWithIDs); //It's the average over 2, because the average of width+height is the diameter and this is the radius
                    break;
                case ShapeTypes.rectangle:
                    return new CollisionBody(new ShapeRectangle(sprite.Position, sprite.Width, sprite.Height), collisionID, collidesWithIDs);
                    break;
            }
            return null;
        }

        public void ScaleWith(Sprite sprite)
        {
            CollisionShape = BodyWithSprite(CollisionShape.ShapeType, sprite, ID).CollisionShape;
            CollisionShape.CenterPosition = Position;
        }

        public static CollisionBody BodyWithSprite(ShapeTypes shapeType, Sprite sprite, string collisionID, params string[] collidesWithIDs)
        {
            return CollisionBody.BodyWithSpriteAndIDs(shapeType, sprite, collisionID, new List<string>(collidesWithIDs));
        }

        public bool CheckForCollide(CollisionBody body)
        {
            if (CollidesWithIDs.Contains(body.ID) || collidesWithAll)
            {
                return CollisionShape.CollidesWith(body.CollisionShape);
            }
            return false;
        }
    }
}
