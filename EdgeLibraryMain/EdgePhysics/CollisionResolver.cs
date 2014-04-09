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
    public static class CollisionResolver
    {
        public static void Solve(CollisionInfo info, PhysicsBody a, PhysicsBody b)
        {
            if (a.Shape.GetShapeType() == ShapeType.Circle)
            {
                if (b.Shape.GetShapeType() == ShapeType.Circle)
                {
                    CircleCircle(info, a, b);
                }
                else
                {
                    CirclePolygon(info, a, b);
                }
            }
            else
            {
                if (b.Shape.GetShapeType() == ShapeType.Circle)
                {
                    PolygonCircle(info, a, b);
                }
                else
                {
                    PolygonPolygon(info, a, b);
                }
            }
        }

        public static void CircleCircle(CollisionInfo info, PhysicsBody a, PhysicsBody b)
        {
            CircleShape A = (CircleShape)a.Shape;
            CircleShape B = (CircleShape)b.Shape;

            Vector2 normal = b.Position - a.Position;
            float distSqr = normal.LengthSquared();
            float distance = normal.Length();
            float radius = A.Radius + B.Radius;

            if (distSqr > radius * radius) { info.ContactNumber = 0;  return; }

            if (distance == 0)
            {
                info.Depth = A.Radius;
                info.Normal = new Vector2(1, 0);
                info.Contacts[0] = a.Position;
            }
            else
            {
                info.Depth = radius - distance;
                info.Normal = normal / distance;
                info.Contacts[0] = info.Normal * A.Radius + a.Position;
            }
        }

        public static void CirclePolygon(CollisionInfo info, PhysicsBody a, PhysicsBody b)
        {
            CircleShape A = (CircleShape)a.Shape;
            PolygonShape B = (PolygonShape)b.Shape;

        }

        public static void PolygonCircle(CollisionInfo info, PhysicsBody a, PhysicsBody b)
        {
            PolygonShape A = (PolygonShape)a.Shape;
            CircleShape B = (CircleShape)b.Shape;

        }

        public static void PolygonPolygon(CollisionInfo info, PhysicsBody a, PhysicsBody b)
        {
            PolygonShape A = (PolygonShape)a.Shape;
            PolygonShape B = (PolygonShape)b.Shape;

        }
    }
}
