using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EdgePhysics
{
    public class CircleShape : Shape
    {
        public CircleShape(float radius)
        {
            Radius = radius;
        }

        public override void SetMassInertia(PhysicsBody body, float density)
        {
            //Area of the circle (PI * r^2) multiplied by density
            body.Mass = (float)Math.PI * Radius * Radius * density;
            body.Inertia = body.Mass*Radius * Radius;
        }

        public override void SetRotation(float degrees)
        {
            //Circles don't have rotation
        }

        public override void Draw(SpriteBatch spriteBatch, Color color)
        {
            base.Draw(spriteBatch, color);
        }

        public override Shape Clone()
        {
            return new CircleShape(Radius);
        }

        public override ShapeType GetShapeType()
        {
            return ShapeType.Circle;
        }
    }
}
