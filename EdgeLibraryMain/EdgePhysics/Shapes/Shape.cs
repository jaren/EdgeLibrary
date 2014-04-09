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

namespace EdgePhysics
{
    public enum ShapeType
    {
        Circle, Polygon, Count, Null
    }

    public class Shape
    {
        //For circle
        public float Radius;

        //For polygon
        public Mat2 Matrix;

        public virtual void Init() { } //Possibly remove
        public virtual Shape Clone() { return null; }
        public virtual void SetMassInertia(PhysicsBody body, float density) { }
        public virtual void Draw(SpriteBatch spriteBatch, Color color) { }
        public virtual void SetRotation(float radians) { }
        public virtual ShapeType GetShapeType() { return ShapeType.Null; }
    }
}
