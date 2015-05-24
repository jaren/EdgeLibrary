using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public static class CollisionDetection
    {
        public static bool CircleRectangle(Vector2 center, float radius, Rectangle rectangle)
        {
            //Detects with a general rectangle-rectangle collision to save time
            if (!RectangleRectangle(new Rectangle((int)(center.X - radius), (int)(center.Y - radius), (int)(radius*2), (int)(radius*2)), rectangle))
            {
                return false;
            }

            //If the circle is not at the corners of the rectangle, then it is in the rectangle
            if ((center.X < rectangle.Right && center.X > rectangle.Left) || (center.Y > rectangle.Bottom && center.Y < rectangle.Top))
            {
                return true;
            }

            //If this is run, the circle must be on one of the corners of the rectangle. If any of the corners intersects with the radius, the circle is in the rectangle
            float distance1 = Vector2.DistanceSquared(center, new Vector2(rectangle.X, rectangle.Y));
            float distance2 = Vector2.DistanceSquared(center, new Vector2(rectangle.X + rectangle.Width, rectangle.Y));
            float distance3 = Vector2.DistanceSquared(center, new Vector2(rectangle.X, rectangle.Y + rectangle.Height));
            float distance4 = Vector2.DistanceSquared(center, new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height));

            if (distance1 < (radius*radius) || distance2 < (radius*radius) || distance3 < (radius*radius) || distance4 < (radius*radius))
            {
                return true;
            }

            return false;
        }

        public static bool CircleCircle(Vector2 center1, float radius1, Vector2 center2, float radius2)
        {
            return Vector2.DistanceSquared(center1, center2) < ((radius1 + radius2)*(radius1 + radius2));
        }

        public static bool RectangleRectangle(Rectangle rectangle1, Rectangle rectangle2)
        {
            bool result = false;
            rectangle1.Intersects(ref rectangle2, out result);
            return result;
        }
    }
}
