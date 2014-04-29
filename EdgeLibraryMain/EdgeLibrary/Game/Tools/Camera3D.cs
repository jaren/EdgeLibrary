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

namespace EdgeLibrary
{
    //Used for drawing the game to the screen
    public class Camera3D
    {
        public Matrix View { get; protected set; }
        public Matrix Projection { get; protected set; }

        //Holds camera data
        public Vector3 Position { get { return _position; } set { _position = value; ReloadView(); } }
        private Vector3 _position;

        public Vector3 Target { get { return _target; } set { _target = value; ReloadView(); } }
        private Vector3 _target;

        public Vector3 UpVector { get { return _upVector; } set { _upVector = value; ReloadView(); } }
        private Vector3 _upVector;

        public float AspectRatio { get { return _aspectRatio; } set { _aspectRatio = value; ReloadView(); } }
        private float _aspectRatio;

        public float Radians { get { return _radians; } set { _radians = value; ReloadView(); } }
        private float _radians;

        public float NearPlaneDistance { get { return _nearPlaneDistance; } set { _nearPlaneDistance = value; ReloadView(); } }
        private float _nearPlaneDistance;

        public float FarPlaneDistance { get { return _farPlaneDistance; } set { _farPlaneDistance = value; ReloadView(); } }
        private float _farPlaneDistance;

        public Camera3D(Vector3 position, Vector3 target, Vector3 upVector, float aspectRatio, float radians = MathHelper.PiOver4, float nearPlaneDistance = 1, float farPlaneDistance = 5000)
        {
            Position = Vector3.Zero;
            _position = position;
            _target = target;
            _upVector = upVector;
            _aspectRatio = aspectRatio;
            _radians = radians;
            _nearPlaneDistance = nearPlaneDistance;
            _farPlaneDistance = farPlaneDistance;
            ReloadView();
            ReloadProjection();
        }

        private void ReloadView()
        {
            View = Matrix.CreateLookAt(Position, Target, UpVector);
        }

        private void ReloadProjection()
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(Radians, AspectRatio, NearPlaneDistance, FarPlaneDistance);
        }
    }
}
