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
    /* License:
    Copyright (c) 2013 Randy Gaul http://RandyGaul.net

    This software is provided 'as-is', without any express or implied
    warranty. In no event will the authors be held liable for any damages
    arising from the use of this software.

    Permission is granted to anyone to use this software for any purpose,
    including commercial applications, and to alter it and redistribute it
    freely, subject to the following restrictions:
      1. The origin of this software must not be misrepresented; you must not
         claim that you wrote the original software. If you use this software
         in a product, an acknowledgment in the product documentation would be
         appreciated but is not required.
      2. Altered source versions must be plainly marked as such, and must not be
         misrepresented as being the original software.
      3. This notice may not be removed or altered from any source distribution.
     */

    public class PhysicsBody
    {
        public Vector2 Position;
        public Vector2 Velocity;

        public float Mass { get { return Shape.GetMass(Density); } set { } }
        public float InvMass { get { return Mass != 0 ? 1 / Mass : 0; } set { if (value != 0) { Mass = 1 / value; } else { Mass = 0; } } }

        public float Inertia { get { return Shape.GetInertia(Density); } set { } }
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
        }

        public void ApplyForce(Vector2 force)
        {
            Forces += force;
        }

        public void ApplyImpulse(Vector2 impulse, Vector2 contact)
        {
            Velocity += InvMass * impulse;
            AngularVelocity += InvInertia * MathTools.CrossProduct(contact, impulse);
        }

        public void SetStatic()
        {
            Mass = 0;
            Inertia = 0;
        }

        public void SetRotation(float degrees)
        {
            float radians = MathHelper.ToRadians(degrees);
            Rotation = radians;
            Shape.SetRotation(radians);
        }
    }
}
