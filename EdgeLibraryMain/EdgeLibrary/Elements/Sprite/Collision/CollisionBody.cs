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
    [Flags]
    public enum CollisionLayers : byte
    {
        A = 1,
        B = 2,
        C = 4,
        D = 8,
        E = 16,
        F = 32,
        G = 64,
        All = 127
    }


    //Serves as the collision body for an element, can check if it is colliding with another element
    public class CollisionBody
    {
        public CollisionLayers CollisionLayers;
        public Vector2 Position { get { return Shape.CenterPosition; } set { Shape.CenterPosition = value; } }
        public Shape Shape { get; set; }

        public CollisionBody(Shape shape)
        {
            Shape = shape;
            CollisionLayers = CollisionLayers.All;
        }

        public CollisionBody(Shape shape, CollisionLayers layers) : this(shape)
        {
            CollisionLayers = layers;
        }

        public static Rectangle Intersect(CollisionBody body1, CollisionBody body2)
        {
            return Rectangle.Intersect(body1.Shape.AsRectangle(), body2.Shape.AsRectangle());
        }

        public static CollisionBody BodyWithSprite(ShapeTypes shapeType, Sprite sprite, CollisionLayers layers)
        {
            switch (shapeType)
            {
                case ShapeTypes.circle:
                    return new CollisionBody(new ShapeCircle(sprite.Position, (sprite.Width + sprite.Height) / 4 * ((sprite.Scale.X + sprite.Scale.Y) / 2)), layers); //It's the average over 2, because the average of width+height is the diameter and this is the radius
                    break;
                case ShapeTypes.rectangle:
                    return new CollisionBody(new ShapeRectangle(sprite.Position, sprite.Width * (int)sprite.Scale.X, sprite.Height * (int)sprite.Scale.Y), layers);
                    break;
            }
            return null;
        }

        public void ScaleWith(Sprite sprite, ShapeTypes shapeType)
        {
            Shape = BodyWithSprite(shapeType, sprite, CollisionLayers).Shape;
        }

        public bool CheckForCollide(CollisionBody body)
        {
            if ((CollisionLayers & body.CollisionLayers) != 0)
            {
                return Shape.CollidesWith(body.Shape);
            }
            return false;
        }
    }
}
