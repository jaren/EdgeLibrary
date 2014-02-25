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
    /*
    public class WaypointPathCapability : Capability
    {
        public List<Vector2> Waypoints;
    }
     */

    public enum MovementType
    {
        Move,
        Follow,
        PointRotation,
        Clamp,
        None
    }

    public class MovementCapability : Capability
    {
        protected Vector2 MoveTarget;
        protected Element FollowTarget;
        protected Element ClampTarget;
        protected Vector2 RotateTarget;
        protected MovementType MovementType;
        public float Speed;

        public MovementCapability() : base("Movement") { STOPPED = true; MovementType = MovementType.None; }

        public override void updateCapability(GameTime gameTime, Element element)
        {
            Vector2 moveVector = Vector2.Zero;
            switch (MovementType)
            {
                case MovementType.Clamp:
                    element.Position = ClampTarget.Position;
                    break;
                case MovementType.Follow:
                    moveVector = new Vector2(FollowTarget.Position.X - element.Position.X, FollowTarget.Position.Y - element.Position.Y);
                    moveVector.Normalize();
                    if (!checkIfEnd(moveVector, FollowTarget.Position, element.Position + moveVector*Speed))
                    {
                        element.Position += moveVector * Speed;
                    }
                    break;
                case MovementType.Move:
                    moveVector = new Vector2(MoveTarget.X - element.Position.X, MoveTarget.Y - element.Position.Y);
                    moveVector.Normalize();
                    STOPPED = checkIfEnd(moveVector, MoveTarget, element.Position + moveVector*Speed);
                    element.Position += moveVector * Speed;
                    break;
                case MovementType.PointRotation:
                    float dist = Vector2.Distance(element.Position, RotateTarget);
                    float angleMeasure = (float)Math.Atan2(element.Position.Y - RotateTarget.Y, element.Position.X - RotateTarget.X);
                    angleMeasure += Speed/40;
                    float newDiffX = (float)Math.Cos(angleMeasure) * dist;
                    float newDiffY = (float)Math.Sin(angleMeasure) * dist;
                    element.Position = new Vector2(RotateTarget.X + newDiffX, RotateTarget.Y + newDiffY);
                    break;
            }
        }

        private bool checkIfEnd(Vector2 moveVector, Vector2 target, Vector2 position)
        {
            if ((moveVector.X < 0) && (position.X < target.X)) { return true; }
            else if ((moveVector.X > 0) && (position.X > target.X)) { return true; }

            if ((moveVector.Y < 0) && (position.Y < target.Y)) { return true; }
            else if ((moveVector.Y > 0) && (position.Y > target.Y)) { return true; }

            return false;
        }


        public void MoveTo(Vector2 target, float speed)
        {
            MoveTarget = target;
            Speed = speed;
            MovementType = MovementType.Move;
            STOPPED = false;
        }

        public void FollowElement(Element target, float speed)
        {
            FollowTarget = target;
            Speed = speed;
            MovementType = MovementType.Follow;
            STOPPED = false;
        }

        public void ClampTo(Element target)
        {
            ClampTarget = target;
            MovementType = MovementType.Clamp;
            STOPPED = false;
        }

        public void RotateAround(Vector2 target, float speed)
        {
            RotateTarget = target;
            Speed = speed;
            MovementType = MovementType.PointRotation;
            STOPPED = false;
        }
    }
}
