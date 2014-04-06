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
    [Flags]
    public enum CollisionLayers : int
    {
        A = 1,
        B = 2,
        C = 4,
        D = 8,
        E = 16,
        F = 32,
        G = 64,
        H = 128,
        I = 256,
        J = 512,
        K = 1024,
        L = 2048,
        M = 4096,
        N = 8192,
        O = 16384,
        P = 32768,
        Q = 65536,
        R = 131072,
        S = 262144,
        T = 524288,
        U = 1048576,
        V = 2097152,
        W = 4194304,
        X = 8388608,
        Y = 16777216,
        Z = 33554432,
        All = 67108863,
    }

    public class Body
    {
        public Shape Shape;
        //Which layers the Body exists in
        public CollisionLayers Layers;
        //Includes Density and Restitution
        public Material Material;
        //Includes Mass and Inertia
        public MassData MassData;
        public Vector2 Velocity;
        //The total forces acting on an object
        private Vector2 Force;
        public float GravityScale;

        public void Update(GameTime gameTime)
        {
            MassData.Mass = Material.Density * Shape.Volume;

            Velocity = MassData.Inv_Mass * Force;
            Shape.Position += Velocity;

            Force = Vector2.Zero;
        }

        public void AddForce(Vector2 force)
        {
            Force += force;
        }

        public float GetVolume()
        {
            return 0;
        }

        public Box GetBox()
        {
            return new Box();
        }
    }

    public struct MassData
    {
        public float Mass;
        public float Inv_Mass 
        {
            get { return Mass != 0 ? 1 / Mass : 0; }
            set 
            {
                if (value != 0)
                { Mass = 1 / value; } else { Mass = 0; }
            }
        }

        public float Inertia;
        public float Inv_Inertia
        {
            get { return Inertia != 0 ? 1 / Inertia : 0; }
            set
            {
                if (value != 0)
                { Inertia = 1 / value; }
                else { Inertia = 0; }
            }
        }
    }

    public struct Material
    {
        public float Density;
        public float Restitution;
    }
}
