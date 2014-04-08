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
using EdgeLibrary;


namespace EdgePhysics
{
    public class PolygonShape : Shape
    {
        public int VertexCount;
        public List<Vector2> Vertices;
        public List<Vector2> Normals;

        public override void SetMassInertia(PhysicsBody body, float density)
        {
            Vector2 centroid = Vector2.Zero;
            float inertia = 0;
            float area = 0;

            for (int vertexNumber = 0; vertexNumber < VertexCount; vertexNumber++)
            {
                Vector2 v1 = Vertices[vertexNumber];
                int vertexNumber2 = vertexNumber + 1 < VertexCount ? vertexNumber + 1: 0;
                Vector2 v2 = Vertices[vertexNumber2];

                float Cross = MathTools.CrossProduct(v1, v2);
                float triangleArea = Cross / 2f;
                area += triangleArea;

                centroid += triangleArea / 3f * (v1 + v2);

                float IFactor1 = v1.X * v1.X + v2.X * v2.X + v1.X * v2.X;
                float IFactor2 = v1.Y * v1.Y + v2.Y * v2.Y + v1.Y * v2.Y;
                inertia += (Cross / 12f) * (IFactor1 + IFactor2);
            }

            centroid *= area;

            for (int i = 0; i < VertexCount; i++)
            {
                Vertices[i] -= centroid;
            }

            body.Mass = density * area;
            body.Inertia = density * inertia;
        }

        public override void Draw(SpriteBatch spriteBatch, Color color)
        {
            base.Draw(spriteBatch, color);
        }

        public override void SetRotation(float radians)
        {
            Matrix.Set(radians);
        }

        public void SetBox()
        {
        }

        public void Set()
        {
        }

        public override Shape Clone()
        {
            PolygonShape poly = new PolygonShape();
            poly.Matrix = Matrix;
            for (int i = 0; i < VertexCount; i++)
            {
                poly.Vertices.Add(Vertices[i]);
                poly.Normals.Add(Normals[i]);
            }
            poly.VertexCount = VertexCount;
            return poly;
        }

        public override ShapeType GetShapeType()
        {
            return ShapeType.Polygon;
        }
    }
}
