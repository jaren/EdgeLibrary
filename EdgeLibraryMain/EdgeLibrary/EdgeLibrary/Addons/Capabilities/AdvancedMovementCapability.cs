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
    public class AdvancedMovementCapability : Capability
    {
        private enum AdvancedMovementType
        {
            RotatingAroundPoint,
            None
        }

        public float Speed;

        private Vector2 RotatingPoint;

        private AdvancedMovementType movementType;

        public AdvancedMovementCapability() : base("AdvancedMovement") { Speed = 1; movementType = AdvancedMovementType.None; }

        public override void Update(GameTime gameTime, Element element)
        {
            switch (movementType)
            {
                    //Work in progress
                case AdvancedMovementType.RotatingAroundPoint:
                    float dist = Vector2.Distance(element.Position, RotatingPoint);
                    float angleMeasure = (float)Math.Atan2(element.Position.Y - RotatingPoint.Y, element.Position.X - RotatingPoint.X);
                    angleMeasure += Speed;
                    float newDiffX = (float)Math.Cos(angleMeasure) * dist;
                    float newDiffY = (float)Math.Sin(angleMeasure) * dist;
                    element.Position = new Vector2(RotatingPoint.X + newDiffX, RotatingPoint.Y + newDiffY);
                    break;
                case AdvancedMovementType.None:
                    break;
            }
        }

        public void RotateElementAroundPoint(Vector2 point)
        {
            RotatingPoint = point;
            movementType = AdvancedMovementType.RotatingAroundPoint;
        }

        public void Stop()
        {
            movementType = AdvancedMovementType.None;
        }

        public override Capability NewInstance()
        {
            return new AdvancedMovementCapability();
        }
    }
}
