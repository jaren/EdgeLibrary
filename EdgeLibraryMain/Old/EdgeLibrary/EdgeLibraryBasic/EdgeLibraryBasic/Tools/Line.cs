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
        public float Slope { get; private set; }
        public float? YIntercept { get; private set; }
        public Color DrawColor;

        //Used only for vertical lines
        public float? VerticalLineXCoord = null;

        public Line(Vector2 point1, Vector2 point2)
        {
            Slope = (point1.Y - point2.Y)/(point1.X - point2.X);
            if (IsVertical(this))
            {
                YIntercept = null;
                VerticalLineXCoord = point1.X;
            }
            else
            {
                YIntercept = (Slope * -point1.X) + point1.Y;
            }
            DrawColor = Color.White;
        }

        public Line(Vector2 point, float slope) : this(point, new Vector2(point.X + 1, point.Y + slope)) { }

        public static Line ParallelToAt(Line line, Vector2 point)
        {
            return new Line(point, line.Slope);
        }

        public static Line PerpendicularToAt(Line line, Vector2 point)
        {
            if (IsVertical(line))
            {
                return new Line(point, 0);
            }
            if (IsHorizontal(line))
            {
                return new Line(point, float.PositiveInfinity);
            }
            return new Line(point, -1 / line.Slope);
        }

        public static bool IsVertical(Line line)
        {
            return Math.Abs(line.Slope) == float.PositiveInfinity;
        }
        public static bool IsHorizontal(Line line)
        {
            return line.Slope == 0;
        }

        public Vector2? Intersection(Line line)
        {
            //Vertical lines could have either "Infinity" or "-Infinity" slope
            if (line.Slope == Slope || (IsVertical(this) && IsVertical(line)))
            {
                return null;
            }

            if (IsVertical(line))
            {
                return new Vector2((float)line.SolveX(0), (float)SolveY((float)line.SolveX(0)));
            }
            else if (IsVertical(this))
            {
                return new Vector2((float)VerticalLineXCoord, (float)line.SolveY((float)VerticalLineXCoord));
            }
            else
            {

                float factor = line.Slope / Slope;
                float y = ((float)YIntercept * -factor) + (float)line.YIntercept;

                if (SolveX(y) == null)
                {
                    return null;
                }
                else
                {
                    return new Vector2((float)SolveX(y), y);
                }
            }
        }

        public float? DistanceTo(Vector2 point)
        {
            Vector2? intersection = Intersection(Line.PerpendicularToAt(this, point));

            if (intersection == null)
            {
                return null;
            }
            else
            {
                return Vector2.Distance((Vector2)intersection, point);
            }
        }

        public float? SolveY(float x)
        {
            if (IsVertical(this))
            {
                return null;
            }

            return (Slope * x) + YIntercept;
        }

        public float? SolveX(float y)
        {
            if (IsVertical(this))
            {
                return VerticalLineXCoord;
            }
            if (IsHorizontal(this))
            {
                return null;
            }
            return (y - YIntercept) / Slope;
        }

        public List<Vector2> GetPointsWithinRectangle(Rectangle rectangle)
        {
            List<Vector2> list = new List<Vector2>();

            if (IsVertical(this))
            {
                for (int y = rectangle.Y; y < rectangle.Y + rectangle.Height; y++)
                {
                    list.Add(new Vector2((float)SolveX(0), y));
                }
            }
            else
            {

                for (int x = rectangle.X; x < rectangle.X + rectangle.Width; x++)
                {
                    if ((SolveY((float)x) != null) && (SolveY((float)x) > rectangle.Y) && (SolveY((float)x) < rectangle.Y + rectangle.Height))
                    {
                        list.Add(new Vector2((float)x, (float)SolveY((float)x)));
                    }
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
