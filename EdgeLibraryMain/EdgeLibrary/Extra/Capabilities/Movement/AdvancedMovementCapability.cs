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
    public class PointRotationCapability : Capability
    {
        public float Speed;

        private Vector2 RotatingPoint;

        public PointRotationCapability() : base("PointRotation") { Speed = 1; STOPPED = true; }

        public override void updateCapability(GameTime gameTime, Element element)
        {
            float dist = Vector2.Distance(element.Position, RotatingPoint);
            float angleMeasure = (float)Math.Atan2(element.Position.Y - RotatingPoint.Y, element.Position.X - RotatingPoint.X);
            angleMeasure += Speed/40;
            float newDiffX = (float)Math.Cos(angleMeasure) * dist;
            float newDiffY = (float)Math.Sin(angleMeasure) * dist;
            element.Position = new Vector2(RotatingPoint.X + newDiffX, RotatingPoint.Y + newDiffY);
        }

        public void RotateElementAroundPoint(Vector2 point)
        {
            RotatingPoint = point;
            STOPPED = false;
        }
    }
}
