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

    public class SimpleMovementCapability : Capability
    {
        public Vector2 TargetPos;
        public Element ConstantTarget;
        private bool ConstantUpdateTarget;
        public float Speed;

        public SimpleMovementCapability() : base("SimpleMovement") { STOPPED = true; ConstantUpdateTarget = false; }

        public override void updateCapability(GameTime gameTime, Element element)
        {
            if (ConstantUpdateTarget)
            {
            }
                Vector2 moveVector = new Vector2(TargetPos.X - element.Position.X, TargetPos.Y - element.Position.Y);
                moveVector.Normalize();
                element.Position += moveVector * Speed;

                STOPPED = checkIfEnd(moveVector, element);
        }

        private bool checkIfEnd(Vector2 moveVector, Element element)
        {
            if ((moveVector.X < 0) && (element.Position.X < TargetPos.X)) { return true; }
            else if ((moveVector.X > 0) && (element.Position.X > TargetPos.X)) { return true; }

            if ((moveVector.Y < 0) && (element.Position.Y < TargetPos.Y)) { return true; }
            else if ((moveVector.Y > 0) && (element.Position.Y > TargetPos.Y)) { return true; }

            return false;
        }


        public void MoveElementTo(Vector2 target, float speed)
        {
            TargetPos = target;
            Speed = speed;
            STOPPED = false;
        }
    }

    public class FollowCapability : Capability
    {
        public Element FollowElement;
        public float Speed;

        public FollowCapability() : base("Follow") { STOPPED = true; }

        public override void updateCapability(GameTime gameTime, Element element)
        {
            if (FollowElement != null)
            {
                Vector2 moveVector = new Vector2(FollowElement.Position.X - element.Position.X, FollowElement.Position.Y - element.Position.Y);
                moveVector.Normalize();
                element.Position += moveVector * Speed;
            }
        }

        public void Follow(Element target, float speed)
        {
            FollowElement = target;
            Speed = speed;
            STOPPED = false;
        }
    }

    public class ClampCapability : Capability
    {
        public Element ClampElement;

        public ClampCapability() : base("Clamp") { STOPPED = true; }

        public override void updateCapability(GameTime gameTime, Element element)
        {
            if (ClampElement != null)
            {
                element.Position = ClampElement.Position;
            }
        }

        public void ClampTo(Element e)
        {
            ClampElement = e;
            STOPPED = false;
        }
    }
}
