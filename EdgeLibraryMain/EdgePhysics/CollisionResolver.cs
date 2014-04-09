﻿using System;
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

            info.ContactNumber = 0;
            Vector2 center = B.Matrix.Transpose() * (a.Position - b.Position);

            float separation = -float.MaxValue;
            int faceNormal = 0;
            for (int i = 0; i < B.Vertices.Count; i++)
            {
                float s = MathTools.DotProduct(B.Normals[i], center - B.Vertices[i]);

                if (s > A.Radius) { return; }

                if (s > separation) { separation = s; faceNormal = i; }
            }

            Vector2 v1 = B.Vertices[faceNormal];
            int i2 = faceNormal + 1 < B.Vertices.Count ? faceNormal + 1 : 0;
            Vector2 v2 = B.Vertices[i2];

            if (separation <= 0)
            {
                info.ContactNumber = 1;
                info.Normal = -(B.Matrix * B.Normals[faceNormal]);
                info.Contacts[0] = info.Normal * A.Radius + a.Position;
                info.Depth = A.Radius;
                return;
            }

            float dot1 = MathTools.DotProduct(center - v1, v2 - v1);
            float dot2 = MathTools.DotProduct(center - v2, v1 - v2);
            info.Depth = A.Radius- separation;

            if (dot1 <= 0.0f)
            {
                if (Vector2.DistanceSquared(center, v1) > A.Radius * A.Radius) { return; }

                info.ContactNumber = 1;
                Vector2 n = v1 - center;
                n = B.Matrix * n;
                n.Normalize();
                info.Normal = n;
                v1 = B.Matrix * v1 + b.Position;
                info.Contacts[0] = v1;
            }
            else if (dot2 <= 0.0f)
            {
                if (Vector2.DistanceSquared(center, v2) > A.Radius * A.Radius) { return; }

                info.ContactNumber = 1;
                Vector2 n = v2 - center;
                v2 = B.Matrix * v2 + b.Position;
                info.Contacts[0] = v2;
                n = B.Matrix * n;
                n.Normalize();
                info.Normal = n;
            }
            else
            {
                Vector2 n = B.Normals[faceNormal];
                if (MathTools.DotProduct(center - v1, n) > A.Radius) { return; }

                n = B.Matrix * n;
                info.Normal = -n;
                info.Contacts[0] = info.Normal * A.Radius + a.Position;
                info.ContactNumber = 1;
            }
        }

        public static void PolygonCircle(CollisionInfo info, PhysicsBody a, PhysicsBody b)
        {
            CirclePolygon(info, a, b);
            info.Normal *= -1;
        }

        public static void PolygonPolygon(CollisionInfo info, PhysicsBody a, PhysicsBody b)
        {
            PolygonShape A = (PolygonShape)a.Shape;
            PolygonShape B = (PolygonShape)b.Shape;

        }

        public static float FindAxisLeastPenetration(ref int faceIndex, PolygonShape a, Vector2 aPosition, PolygonShape b, Vector2 bPosition)
        {
            float bestDistance = -float.MaxValue;
            int bestIndex = 0;

            for (int i = 0; i < a.Vertices.Count; ++i)
            {
                Vector2 n = a.Normals[i];
                Vector2 nw = a.Matrix * n;

                PhysicsMatrix buT = b.Matrix.Transpose();
                n = buT * nw;

                Vector2 s = b.GetSupport(-n);

                Vector2 v = a.Vertices[i];
                v = a.Matrix * v + aPosition;
                v -= bPosition;
                v = buT * v;

                float d = MathTools.DotProduct(n, s - v);

                if (d > bestDistance)
                {
                    bestDistance = d;
                    bestIndex = i;
                }
            }

            faceIndex = bestIndex;
            return bestDistance;
        }

        public static void FindIncidentFace(ref Vector2[] vector, PolygonShape refPoly, PolygonShape incPoly, Vector2 incPolyPosition, int referenceIndex)
        {
            Vector2 referenceNormal = refPoly.Normals[referenceIndex];

            referenceNormal = refPoly.Matrix * referenceNormal;
            referenceNormal = incPoly.Matrix.Transpose() * referenceNormal;

            int incidentFace = 0;
            float minDot = float.MaxValue;
            for (int i = 0; i < incPoly.Vertices.Count; ++i)
            {
                float dot = MathTools.DotProduct(referenceNormal, incPoly.Normals[i]);
                if (dot < minDot)
                {
                    minDot = dot;
                    incidentFace = i;
                }
            }

            vector[0] = incPoly.Matrix * incPoly.Vertices[incidentFace] + incPolyPosition;
            incidentFace = incidentFace + 1 >= incPoly.Vertices.Count ? 0 : incidentFace + 1;
            vector[1] = incPoly.Matrix * incPoly.Vertices[incidentFace] + incPolyPosition;
        }

        public static int Clip(Vector2 n, float c, ref Vector2[] face)
        {
            int sp = 0;
            Vector2[] outVector = face;

          float d1 = MathTools.DotProduct(n, face[0]) - c;
          float d2 = MathTools.DotProduct(n, face[1]) - c;

          if(d1 <= 0.0f) outVector[sp++] = face[0];
          if(d2 <= 0.0f) outVector[sp++] = face[1];
  
          if(d1 * d2 < 0.0f) 
          {
            float alpha = d1 / (d1 - d2);
            outVector[sp] = face[0] + alpha * (face[1] - face[0]);
            ++sp;
          }

          face[0] = outVector[0];
          face[1] = outVector[1];

          return sp;
        }
    }
}