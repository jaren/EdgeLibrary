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
        public float Speed;
        private bool HasFinished;

        public SimpleMovementCapability() : base("SimpleMovement")
        {
            HasFinished = true;
        }

        public override void Update(GameTime gameTime, Element element)
        {
            if (!HasFinished)
            {
                Vector2 moveVector = new Vector2(TargetPos.X - element.Position.X, TargetPos.Y - element.Position.Y);
                moveVector.Normalize();
                element.Position += moveVector * Speed;

                HasFinished = checkIfEnd(moveVector, element);
            }
        }

        public override Capability NewInstance()
        {
            return new SimpleMovementCapability();
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
            HasFinished = false;
        }
    }

    public class ClampCapability : Capability
    {
        public Element ClampElement;

        public ClampCapability() : base("Clamp") { }

        public override void Update(GameTime gameTime, Element element)
        {
            if (ClampElement != null)
            {
                element.Position = ClampElement.Position;
            }
        }

        public override Capability NewInstance()
        {
            return new ClampCapability();
        }
    }
}
