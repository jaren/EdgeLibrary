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
using System.Xml;
using System.Xml.Linq;

namespace EdgeLibrary
{
    public class Line : Element
    {
        public float Slope;
        public float YIntercept;

        public Line(Vector2 point1, Vector2 point2)
        {
            Slope = ((point1.X - point2.X) / (point1.Y - point2.Y));
            YIntercept = SolveForY(0);
        }

        public Line(Vector2 point, float slope)
        {
        }

        public Line ParallelToAt(Vector2 point, Line line)
        {
        }

        public Line PerpendicularToAt(Vector2 point, Line line)
        {
        }

        public Vector2? Intersection(Line line)
        {
        }

        public float DistanceTo(Vector2 point)
        {
        }

        public float? SolveForY(float x)
        {

        }

        public float? SolveForX(float y)
        {

        }

        public List<Vector2> GetPointsWithinRectangle(Rectangle rectangle)
        {
        }
    }
}
