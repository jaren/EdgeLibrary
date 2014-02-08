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
    //There is possibly a problem if there is a vertical or horizontal line - the slopes are infinity? and 0
    public class Line : Element
    {
        public float Slope;
        public float YIntercept;
        public Color DrawColor;

        public Line(Vector2 point1, Vector2 point2)
        {
            Slope = (point1.Y - point2.Y)/(point1.X - point2.X);
            YIntercept = (Slope * -point1.X) + point1.Y;
            DrawColor = Color.White;
        }

        public Line(Vector2 point, float slope) : this(point, new Vector2(point.X + 1, point.Y + slope)) { }

        public static Line ParallelToAt(Vector2 point, Line line)
        {
            return new Line(point, line.Slope);
        }

        public static Line PerpendicularToAt(Vector2 point, Line line)
        {
            return new Line(point, -1 / line.Slope);
        }

        public Vector2? Intersection(Line line)
        {
            if (line.Slope == Slope)
            {
                return null;
            }

            float factor = line.Slope / Slope;
            float y = (YIntercept *= -factor) + line.YIntercept;

            if (SolveForX(y) == null)
            {
                return null;
            }
            else
            {
                return new Vector2((float)SolveForX(y), y);
            }
        }

        public float? DistanceTo(Vector2 point)
        {
            Vector2? intersection = Intersection(Line.PerpendicularToAt(point, this));

            if (intersection == null)
            {
                return null;
            }
            else
            {
                return Vector2.Distance((Vector2)intersection, point);
            }
        }

        public float? SolveForY(float x)
        {
            return (Slope * x) + YIntercept;
        }

        public float? SolveForX(float y)
        {
            return (y - YIntercept) / Slope;
        }

        public List<Vector2> GetPointsWithinRectangle(Rectangle rectangle)
        {
            List<Vector2> list = new List<Vector2>();
            for (int x = rectangle.X; x < rectangle.X + rectangle.Width; x++)
            {
                if (SolveForY((float)x) == null)
                {
                    list.Add(new Vector2((float)x, (float)SolveForY((float)x)));
                }
            }
            
            return list;
        }

        public override void drawElement(GameTime gameTime)
        {
            foreach (Vector2 point in GetPointsWithinRectangle(new Rectangle(0, 0, (int)EdgeGame.WindowSize.X, (int)EdgeGame.WindowSize.Y)))
            {
                TextureTools.DrawPixelAt(point, DrawColor);
            }
        }
    }
}
