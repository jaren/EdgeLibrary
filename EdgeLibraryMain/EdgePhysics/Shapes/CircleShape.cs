using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgePhysics
{
    public class CircleShape : Shape
    {
        public CircleShape(float radius)
        {
            Radius = radius;
        }

        public override float GetMass(float density)
        {
            //Area of the circle (PI * r^2) multiplied by density
            return (float)Math.PI * Radius * Radius * density;
        }

        public override float GetInertia(float density)
        {
            return GetMass(density) * Radius * Radius;
        }

        public override void SetRotation(float degrees)
        {
            //Circles don't have rotation
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
