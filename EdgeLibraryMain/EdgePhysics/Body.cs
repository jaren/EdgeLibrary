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
    public class PhysicsBody
    {
        public Vector2 Position;
        public Vector2 Velocity;

        public float Mass;
        public float InvMass { get { return Mass != 0 ? 1 / Mass : 0; } set { if (value != 0) { Mass = 1 / value; } else { Mass = 0; } } }

        public float Inertia;
        public float InvInertia { get { return Inertia != 0 ? 1 / Inertia : 0; } set { if (value != 0) { Inertia = 1 / value; } else { Inertia = 0; } } }

        public float AngularVelocity;
        public float Torque;
        public float Rotation; //In radians

        public float StaticFriction;
        public float DynamicFriction;
        public float Restitution;
        public float Density;

        public Shape Shape;

        private Vector2 Forces;

        public PhysicsBody(Shape shape, Vector2 position)
        {
            Position = position;
            Shape = shape;
            Shape.Init();

            Velocity = Vector2.Zero;

            Density = 1;
            AngularVelocity = 0;
            Torque = 0;
            Rotation = 0;
            Forces = Vector2.Zero;
            StaticFriction = 0.5f;
            DynamicFriction = 0.3f;
            Restitution = 0.2f;
            Shape.SetMassInertia(this, 1);
        }

        public void ApplyForce(Vector2 force)
        {
            Forces += force;
        }

        public void ApplyImpulse(Vector2 impulse, Vector2 contact)
        {
            Velocity += InvMass * impulse;
            AngularVelocity += InvInertia * contact.CrossProduct(impulse);
        }

        public void SetStatic()
        {
            Mass = 0;
            Inertia = 0;
        }

        public void SetRotation(float radians)
        {
            Rotation = radians;
            Shape.SetRotation(radians);
        }
    }
}
