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
    //The different collision layers that a sprite could use
    [Flags]
    public enum CollisionLayers : int
    {
        A = 1,
        B = 2,
        C = 4,
        D = 8,
        E = 16,
        F = 32,
        G = 64,
        H = 128,
        I = 256,
        J = 512,
        K = 1024,
        L = 2048,
        M = 4096,
        N = 8192,
        O = 16384,
        P = 32768,
        Q = 65536,
        R = 131072,
        S = 262144,
        T = 524288,
        U = 1048576,
        V = 2097152,
        W = 4194304,
        X = 8388608,
        Y = 16777216,
        Z = 33554432,
        All = 67108863,
    }

    //Serves as the collision body for an element, can check if it is colliding with another element
    public class CollisionBody
    {
        //The layers which the collision body exists in
        public CollisionLayers CollisionLayers;

        public Vector2 Position { get { return Shape.CenterPosition; } set { Shape.CenterPosition = value; } }
        //The "actual collision checker"
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

        public static CollisionBody BodyWithSprite(Sprite sprite, CollisionLayers layers)
        {
            switch (sprite.CollisionBodyType)
            {
                case ShapeTypes.circle:
                    return new CollisionBody(new ShapeCircle(sprite.Position, (sprite.Width + sprite.Height) / 4 * ((sprite.Scale.X + sprite.Scale.Y) / 2)), layers); //It's the average over 2, because the average of width+height is the diameter and this is the radius
                case ShapeTypes.rectangle:
                    return new CollisionBody(new ShapeRectangle(sprite.Position, sprite.Width * sprite.Scale.X, sprite.Height * sprite.Scale.Y), layers);
            }
            return null;
        }

        public void ScaleWith(Sprite sprite)
        {
            Shape = BodyWithSprite(sprite, CollisionLayers).Shape;
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
