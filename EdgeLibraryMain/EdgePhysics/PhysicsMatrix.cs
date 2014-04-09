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
    //A small (2x2) matrix used for polygons
    public struct PhysicsMatrix
    {
        public float M00; public float M01;
        public float M10; public float M11;

        public PhysicsMatrix(float radians)
        {
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);

            M00 = cos; M01 = -sin;
            M10 = sin; M11 = cos;
        }

        public PhysicsMatrix(float a, float b, float a2, float b2)
        {
            M00 = a; M01 = b;
            M10 = a2; M11 = b2;
        }

        public void Set(float radians)
        {
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);

            M00 = cos; M01 = -sin;
            M10 = sin; M11 = cos;
        }

        public PhysicsMatrix Abs()
        {
            return new PhysicsMatrix(Math.Abs(M00), Math.Abs(M01), Math.Abs(M10), Math.Abs(M11));
        }

        public Vector2 AxisX()
        {
            return new Vector2(M00, M10);
        }

        public Vector2 AxisY()
        {
            return new Vector2(M01, M11);
        }

        public PhysicsMatrix Transpose()
        {
            return new PhysicsMatrix(M00, M10, M01, M11);
        }

        public static Vector2 operator *(Vector2 vector, PhysicsMatrix matrix)
        {
            return new Vector2(matrix.M00 * vector.X + matrix.M01 * vector.Y, matrix.M10 * vector.X + matrix.M11 * vector.Y);
        }

        public static PhysicsMatrix operator *(PhysicsMatrix matrix, PhysicsMatrix matrix2)
        {
            // [00 01]  [00 01]
            // [10 11]  [10 11]

            return new PhysicsMatrix(
              matrix.M00 * matrix2.M00 + matrix.M01 * matrix2.M10,
              matrix.M00 * matrix2.M01 + matrix.M01 * matrix2.M11,
              matrix.M10 * matrix2.M00 + matrix.M11 * matrix2.M10,
              matrix.M10 * matrix2.M01 + matrix.M11 * matrix2.M10
            );
        }
    }
}
