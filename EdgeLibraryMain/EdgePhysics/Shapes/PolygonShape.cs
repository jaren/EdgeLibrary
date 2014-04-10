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
        public List<Vector2> Vertices;
        public List<Vector2> Normals;

        public override void SetMassInertia(PhysicsBody body, float density)
        {
            Vector2 centroid = Vector2.Zero;
            float inertia = 0;
            float area = 0;

            for (int vertexNumber = 0; vertexNumber < Vertices.Count; vertexNumber++)
            {
                Vector2 v1 = Vertices[vertexNumber];
                int vertexNumber2 = vertexNumber + 1 < Vertices.Count ? vertexNumber + 1: 0;
                Vector2 v2 = Vertices[vertexNumber2];

                float Cross = v1.CrossProduct(v2);
                float triangleArea = Cross / 2f;
                area += triangleArea;

                centroid += triangleArea / 3f * (v1 + v2);

                float IFactor1 = v1.X * v1.X + v2.X * v2.X + v1.X * v2.X;
                float IFactor2 = v1.Y * v1.Y + v2.Y * v2.Y + v1.Y * v2.Y;
                inertia += (Cross / 12f) * (IFactor1 + IFactor2);
            }

            centroid *= area;

            for (int i = 0; i < Vertices.Count; i++)
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

        //Creates a box
        public void SetBox(float halfWidth, float halfHeight)
        {
            Vertices.Clear();
            Normals.Clear();
            
            Vertices.Add(new Vector2(-halfWidth, -halfHeight));
            Normals.Add(new Vector2(0, -1));

            Vertices.Add(new Vector2(halfWidth, -halfHeight));
            Normals.Add(new Vector2(1, 0));

            Vertices.Add(new Vector2(halfWidth, halfHeight));
            Normals.Add(new Vector2(0, 1));

            Vertices.Add(new Vector2(-halfWidth, halfHeight));
            Normals.Add(new Vector2(-1, 0));
        }

        public void Set(params Vector2[] vertices)
        {
            Set(new List<Vector2>(vertices));
        }
        public void Set(List<Vector2> vertices)
        {
            if (vertices.Count < 2) { throw new Exception("There must be three or more vertices."); }

            Vertices.Clear();
            Normals.Clear();

            //Find the farthest right point of the vertices
            int rightMost = 0;
            float highestXCoord = vertices[0].X;
            for (int i = 0; i < vertices.Count; i++)
            {
                float x = vertices[i].X;
                if (x > highestXCoord)
                { 
                    highestXCoord = x; 
                    rightMost = i; 
                }

                //If they have the same X-Coordinate, take the lowest one
                else if ((x == highestXCoord) && (vertices[i].Y < vertices[rightMost].Y))
                {
                    rightMost = i;
                }
            }

            int[] hull = new int[vertices.Count];
            int outCount = 0;
            int indexHull = rightMost;

            while (true)
            {
                hull[outCount] = indexHull;

                // Search for next index that wraps around the hull
                // by computing cross products to find the most counter-clockwise
                // vertex in the set, given the previos hull index
                int nextHullIndex = 0;

                for (int i = 1; i < vertices.Count; i++)
                {
                    // Skip if same coordinate as we need three unique
                    // points in the set to perform a cross product
                    if (nextHullIndex == indexHull)
                    {
                        nextHullIndex = i;
                        continue;
                    }

                    // Cross every set of three unique vertices
                    // Record each counter clockwise third vertex and add
                    // to the output hull
                    Vector2 e1 = vertices[nextHullIndex] - vertices[hull[outCount]];
                    Vector2 e2 = vertices[i] - vertices[hull[outCount]];
                    float Crossed = e1.CrossProduct(e2);

                    // Cross product is zero then e vectors are on same line
                    // therefor want to record vertex farthest along that line
                    if (Crossed < 0) { nextHullIndex = i; }
                    if (Crossed == 0 && e2.LengthSquared() > e1.LengthSquared()) { nextHullIndex = i; }
                }

                outCount++;
                indexHull = nextHullIndex;

                // Conclude algorithm upon wrap-around
                if (nextHullIndex == rightMost)
                {
                    break;
                }
            }

            // Copy vertices into shape's vertices
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices[i] = vertices[hull[i]];
            }

            // Compute face normals
            for (int i = 0; i < vertices.Count; i++)
            {
                int i2 = i + 1 < Vertices.Count ? i + 1 : 0;
                Vector2 face = Vertices[i2] - Vertices[i];

                if (face.LengthSquared() == 0)
                {
                    throw new Exception("Length was zero.");
                }

                // Calculate normal with 2D cross product between vector and scalar
                Vector2 normal = new Vector2(face.Y, -face.X);
                normal.Normalize();
                Normals.Add(normal);
            }
        }

        //The extreme point along a direction in a polygon
        public Vector2 GetSupport(Vector2 direction)
        {
            float bestProjection = -float.MaxValue;
            Vector2 bestVertex = Vector2.Zero;

            foreach (Vector2 vertex in Vertices)
            {
                float projection = vertex.DotProduct(direction);

                if (projection > bestProjection)
                {
                    bestVertex = vertex;
                    bestProjection = projection;
                }
            }

            return bestVertex;
        }

        public override Shape Clone()
        {
            PolygonShape poly = new PolygonShape();
            poly.Matrix = Matrix;
            for (int i = 0; i < Vertices.Count; i++)
            {
                poly.Vertices.Add(Vertices[i]);
                poly.Normals.Add(Normals[i]);
            }
            return poly;
        }

        public override ShapeType GetShapeType()
        {
            return ShapeType.Polygon;
        }
    }
}
