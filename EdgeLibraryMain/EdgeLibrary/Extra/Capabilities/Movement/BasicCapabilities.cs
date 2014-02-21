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
            Vector2 moveVector = Vector2.Zero;
            if (ConstantUpdateTarget)
            {
                moveVector = new Vector2(ConstantTarget.Position.X - element.Position.X, ConstantTarget.Position.Y - element.Position.Y);
            }
            else
            {
                moveVector = new Vector2(TargetPos.X - element.Position.X, TargetPos.Y - element.Position.Y);
            }
            moveVector.Normalize();

            if (!ConstantUpdateTarget)
            {
                STOPPED = checkIfEnd(moveVector, TargetPos, element.Position + moveVector*Speed);
            }
            else
            {
                if (checkIfEnd(moveVector, ConstantTarget.Position, element.Position + moveVector*Speed))
                {
                    return;
                }
            }

            element.Position += moveVector * Speed;
        }

        private bool checkIfEnd(Vector2 moveVector, Vector2 target, Vector2 position)
        {
            if ((moveVector.X < 0) && (position.X < target.X)) { return true; }
            else if ((moveVector.X > 0) && (position.X > target.X)) { return true; }

            if ((moveVector.Y < 0) && (position.Y < target.Y)) { return true; }
            else if ((moveVector.Y > 0) && (position.Y > target.Y)) { return true; }

            return false;
        }


        public void MoveElementTo(Vector2 target, float speed)
        {
            TargetPos = target;
            Speed = speed;
            ConstantUpdateTarget = false;
            STOPPED = false;
        }

        public void Follow(Element target, float speed)
        {
            ConstantTarget = target;
            Speed = speed;
            ConstantUpdateTarget = true;
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
