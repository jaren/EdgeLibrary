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
    public class CollisionInfo
    {
        public PhysicsBody BodyA;
        public PhysicsBody BodyB;
        public float Depth;
        public Vector2 Normal;
        public Vector2[] Contacts;
        public int ContactNumber;
        public float MixedRestitution;
        public float MixedDynamicFriction;
        public float MixedStaticFriction;

        public CollisionInfo(PhysicsBody a, PhysicsBody b)
        {
            BodyA = a;
            BodyB = b;

            Contacts = new Vector2[2];
        }

        public void Init()
        {
            MixedRestitution = Math.Min(BodyA.Restitution, BodyB.Restitution);
            MixedStaticFriction = (float)Math.Sqrt(BodyA.StaticFriction * BodyB.StaticFriction);
            MixedDynamicFriction = (float)Math.Sqrt(BodyA.DynamicFriction * BodyB.DynamicFriction);

            for (int i = 0; i < ContactNumber; i++)
            {
                Vector2 radiusA = Contacts[i] - BodyA.Position;
                Vector2 radiusB = Contacts[i] - BodyB.Position;

                Vector2 relativeVelocity = BodyB.Velocity + BodyB.AngularVelocity.CrossProduct(radiusB) - BodyA.Velocity - BodyA.AngularVelocity.CrossProduct(radiusA);

                if (relativeVelocity.LengthSquared() < PhysicsWorld.Gravity.LengthSquared()) { MixedRestitution = 0; }
            }
        }

        public void Solve()
        {
            CollisionResolver.Solve(this, BodyA, BodyB);
        }

        public void ApplyImpulse()
        {
            if (BodyA.InvMass + BodyB.InvMass == 0)
            {
                InfiniteMassCorrection();
                return;
            }

            for (int i = 0; i < ContactNumber; i++)
            {
                Vector2 radiusA = Contacts[i] - BodyA.Position;
                Vector2 radiusB = Contacts[i] - BodyB.Position;

                Vector2 relativeVelocity = BodyB.Velocity + BodyB.AngularVelocity.CrossProduct(radiusB) - BodyA.Velocity - BodyA.AngularVelocity.CrossProduct(radiusA);

                float contactVelocity = relativeVelocity.CrossProduct(Normal);

                //Do not resolve if velocities are separating
                if (contactVelocity > 0) { return; }

                float radiusACross = radiusA.CrossProduct(Normal);
                float radiusBCross = radiusB.CrossProduct(Normal);
                float invMassSum = BodyA.InvMass + BodyB.InvMass + (radiusACross * radiusACross * BodyA.InvInertia) + (radiusBCross * radiusBCross * BodyB.InvInertia);

                float scalar = -(1 + MixedRestitution) * contactVelocity;
                scalar /= invMassSum;
                scalar /= (float)ContactNumber;

                Vector2 impulse = Normal * scalar;
                BodyA.ApplyImpulse(impulse, radiusA);
                BodyB.ApplyImpulse(impulse, radiusB);

                relativeVelocity = BodyB.Velocity + BodyB.AngularVelocity.CrossProduct(radiusB) - BodyA.Velocity - BodyA.AngularVelocity.CrossProduct(radiusA);

                Vector2 t = relativeVelocity - (Normal * relativeVelocity.DotProduct(Normal));
                t.Normalize();

                float scalarT = -relativeVelocity.DotProduct(t);
                scalarT /= invMassSum;
                scalarT /= (float)ContactNumber;

                if (scalarT == 0) { return; }

                Vector2 tangentImpulse;
                if (Math.Abs(scalarT) < scalar * MixedStaticFriction)
                {
                    tangentImpulse = t * scalarT;
                }
                else
                {
                    tangentImpulse = t * -scalar * MixedDynamicFriction;
                }

                BodyA.ApplyImpulse(tangentImpulse, radiusA);
                BodyB.ApplyImpulse(tangentImpulse, radiusB);
            }

        }

        public void PositionalCorrection()
        {
           float slop = 0.05f; // Penetration allowance
           float percent = 0.4f; // Penetration percentage to correct

           Vector2 correction = (Math.Max(Depth - slop, 0) / (BodyA.InvMass + BodyB.InvMass)) * Normal * percent;
           BodyA.Position -= correction * BodyA.InvMass;
           BodyB.Position += correction * BodyB.InvMass;
        }

        public void InfiniteMassCorrection()
        {
            BodyA.Velocity = Vector2.Zero;
            BodyB.Velocity = Vector2.Zero;
        }
    }
}
