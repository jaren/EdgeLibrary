using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class Restriction
    {
        public Rectangle BoundingBox;
        public RestrictionType Type;

        public Restriction(Rectangle boundingBox, RestrictionType type)
        {
            BoundingBox = boundingBox;
            Type = type;
        }

        public virtual bool IntersectsWith(Rectangle rectangle)
        {
            return CollisionDetection.RectangleRectangle(BoundingBox, rectangle);
        }

        public virtual bool IntersectsWith(Vector2 center, float radius)
        {
            return CollisionDetection.CircleRectangle(center, radius, BoundingBox);
        }
    }

    public class CircleRestriction : Restriction
    {
        public float Radius;
        public Vector2 Center;

        public CircleRestriction(float radius, Vector2 center, RestrictionType type) : base(new Rectangle((int)(center.X - radius), (int)(center.Y - radius), (int)(radius*2), (int)(radius*2)), type)
        {
            Radius = radius;
            Center = center;
        }

        public override bool IntersectsWith(Rectangle rectangle)
        {
            return CollisionDetection.CircleRectangle(Center, Radius, rectangle);
        }

        public override bool IntersectsWith(Vector2 center, float radius)
        {
            return CollisionDetection.CircleCircle(Center, Radius, center, radius);
        }
    }

    public enum RestrictionType
    {
        Water,
        Path,
        Object
    }
}
