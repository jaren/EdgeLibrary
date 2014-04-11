using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EdgePhysics
{
    public static class PhysicsExtensions
    {
        /// <summary>
        /// Cross product of vectors
        /// </summary>
        public static float CrossProduct(this Vector2 a, Vector2 b)
        {
            return (a.X * b.Y) - (a.Y * b.X);
        }
        public static Vector2 CrossProduct(this Vector2 vector, float cross)
        {
            return new Vector2(cross * vector.Y, -cross * vector.X);
        }
        public static Vector2 CrossProduct(this float cross, Vector2 vector)
        {
            return new Vector2(-cross * vector.Y, cross * vector.X);
        }

        /// <summary>
        /// Dot product of vectors
        /// </summary>
        public static float DotProduct(this Vector2 a, Vector2 b)
        {
            return (a.X * b.X) + (a.Y * b.Y);
        }

    }
}
