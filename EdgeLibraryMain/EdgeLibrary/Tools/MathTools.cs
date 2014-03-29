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
    public static class MathTools
    {  
        public static float circlePointStep = 8;
        public static float outerCirclePointStep = 1;

        private static Dictionary<Type, int> GivenIDs = new Dictionary<Type, int>();

        /// <summary>
        /// Generates a random ID based on the element's type and the number of previously given IDs of that type
        /// </summary>
        public static string GenerateID(Element element)
        {
            //If no other elements of this type exist, then create an index for it
            if (!GivenIDs.ContainsKey(element.GetType()))
            {
                GivenIDs.Add(element.GetType(), 0);
            }
            GivenIDs[element.GetType()]++;
            return element.GetType().Name + GivenIDs[element.GetType()].ToString();
        }

        /// <summary>
        /// Retrieves a color by its name.
        /// As this method uses a (expensive) reflection call, it should only be invoked at load time.
        /// If the color is known at compile time, a static property on the <see cref="Color"/> class should be used instead.
        /// </summary>
        public static Color ColorFromString(string colorString)
        {
            var typeProperty = typeof(Color).GetProperty(colorString);
            if (typeProperty != null)
            {
                return (Color)typeProperty.GetValue(null, null);
            }
            else
            {
                return Color.Black;
            }
        }

        /// <summary>
        /// Gets a color from a hex string
        /// </summary>
        public static Color ColorFromHex(string hexString)
        {
            if (!hexString.Contains('#'))
            {
                hexString = "#" + hexString;
            }
            System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(hexString);
            return new Color(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Uniformly adds the specified value to the alpha, red, green, and blue components of the specified color.
        /// </summary>
        public static Color AddToColor(Color color, int number)
        {
            color.R = color.R + number >= 0 ? color.R + number >= 256 ? (byte)255 : (byte)(color.R + number) : (byte)0;
            color.G = color.G + number >= 0 ? color.G + number >= 256 ? (byte)255 : (byte)(color.G + number) : (byte)0;
            color.B = color.B + number >= 0 ? color.B + number >= 256 ? (byte)255 : (byte)(color.B + number) : (byte)0;
            color.A = color.A + number >= 0 ? color.A + number >= 256 ? (byte)255 : (byte)(color.A + number) : (byte)0;
            return color;
        }

        /// <summary>
        /// Generates a color with random alpha, red, green, and blue channel values.
        /// </summary>
        public static Color RandomColor()
        {
            return RandomColor(Color.Black, Color.White);
        }

        /// <summary>
        /// Generates a random color bounded by the ARGB values of each color.
        /// </summary>
        /// <remarks>
        /// "min" and "max" really mean nothing in this case, as Math.Min and Math.Max are used in the function.
        /// </remarks>
        public static Color RandomColor(Color min, Color max)
        {
            return new Color(RandomTools.RandomInt(Math.Min(min.R, max.R), Math.Max(min.R, max.R)), RandomTools.RandomInt(Math.Min(min.G, max.G), Math.Max(min.G, max.G)), RandomTools.RandomInt(Math.Min(min.B, max.B), Math.Max(min.B, max.B)), RandomTools.RandomInt(Math.Min(min.A, max.A), Math.Max(min.A, max.A)));
        }
        /// <summary>
        /// Generates a random color which may not be between the ARGB values of each color because it adds the same random number to the R, G, B, and A
        /// </summary>
        public static Color RandomUniformColor(Color min, Color max)
        {
            int random = RandomTools.RandomInt(((max.R - min.R) + (max.G - min.G) + (max.B - min.G)) / 3);
            return new Color(min.R + random, min.G + random, min.B + random, min.A + random);
        }

        /// <summary>
        /// Subtracts the specified amount from each component of the specified vector.
        /// Guarantees the sign of vector components (positive or negative) will remain the same after decrease.
        /// </summary>
        public static Vector2 DecreaseVector(Vector2 vector, float amount)
        {
            if (vector.X >= 0)
            {
                vector = new Vector2(vector.X - amount, vector.Y);
                if (vector.X < 0)
                {
                    vector = new Vector2(0, vector.Y);
                }
            }
            else
            {
                vector = new Vector2(vector.X + amount, vector.Y);
                if (vector.X > 0)
                {
                    vector = new Vector2(0, vector.Y);
                }
            }

            if (vector.Y >= 0)
            {
                vector = new Vector2(vector.X, vector.Y - amount);
                if (vector.Y < 0)
                {
                    vector = new Vector2(vector.X, 0);
                }
            }
            else
            {
                vector = new Vector2(vector.X, vector.Y + amount);
                if (vector.Y > 0)
                {
                    vector = new Vector2(vector.X, 0);
                }
            }

            return vector;
        }

        /// <summary>
        /// Flips the rectangle if its width and height are negative
        /// </summary>
        public static Rectangle ResolveNegativeRectangle(Rectangle rectangle)
        {
            if (rectangle.Width < 0)
            {
                // If value is negative use primitive int multiply, don't bother with absolute value
                rectangle.Width *= -1;
                rectangle.X -= rectangle.Width;
            }
            if (rectangle.Height < 0)
            {
                // If value is negative use primitive int multiply, don't bother with absolute value
                rectangle.Height *= -1;
                rectangle.Y -= rectangle.Height;
            }
            return rectangle;
        }

        /// <summary>
        /// Returns the midpoint of a line segment drawn from one point to the other.
        /// </summary>
        public static Vector2 MidPoint(Vector2 point1, Vector2 point2)
        {
            float diffX = point1.X - point2.X;
            float diffY = point1.Y - point2.Y;
            return new Vector2(point2.X + diffX / 2, point2.Y + diffY / 2);
        }

        //Used for a string such as: 'Planet/Country/State/City/Street/House' - this would return House
        public static string LastPortionOfPath(string path)
        {
            return LastPortionOfPath(path, '/');
        }
        public static string LastPortionOfPath(string path, char splitter)
        {
            string[] splitParts = path.Split(splitter);
            return splitParts[splitParts.Length - 1];
        }

        //Returns all the points of the given circle
        public static List<Vector2> GetCirclePoints(Vector2 centerPosition, float radius, float step)
        {
            List<Vector2> points = new List<Vector2>();
            float actualStep = step / radius;

            for (float currentRadius = radius; currentRadius > 0; currentRadius -= actualStep)
            {
                for (float x = centerPosition.X - currentRadius; x <= centerPosition.X + currentRadius; x += actualStep)
                {
                    /* Solve for y based on: x^2 + y^2 = r^2 at center 0, 0
                                             (x-centerX)^2 + (y-centerY)^2 = r^2
                                             y = SqRt(r^2 - (x-centerX)^2) + centerY  */

                    //First point's y coordinate - bottom half
                    float y = (float)(Math.Sqrt(Math.Pow(currentRadius, 2) - Math.Pow(x - centerPosition.X, 2)) + centerPosition.Y);

                    //Second point's y coordinate - top half
                    float y1 = -(y - centerPosition.Y) + centerPosition.Y;

                    points.Add(new Vector2(x, y));
                    points.Add(new Vector2(x, y1));
                }
            }

            return points;
        }
        public static List<Vector2> GetCirclePoints(Vector2 centerPosition, float radius)
        {
            return GetCirclePoints(centerPosition, radius, circlePointStep);
        }

        //Returns all the points on the outside of the given circle
        public static List<Vector2> GetOuterCirclePoints(Vector2 centerPosition, float radius, float step)
        {
            List<Vector2> points = new List<Vector2>();
            float actualStep = step / radius;

            for (float x = centerPosition.X - radius; x <= centerPosition.X + radius; x += actualStep)
            {
                /* Solve for y based on: x^2 + y^2 = r^2 at center 0, 0
                                         (x-centerX)^2 + (y-centerY)^2 = r^2
                                         y = SqRt(r^2 - (x-centerX)^2) + centerY  */

                //First point's y coordinate - bottom half
                float y = (float)(Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(x - centerPosition.X, 2)) + centerPosition.Y);

                //Second point's y coordinate - top half
                float y1 = -(y - centerPosition.Y) + centerPosition.Y;

                points.Add(new Vector2(x, y));
                points.Add(new Vector2(x, y1));
            }

            return points;
        }
        public static List<Vector2> GetOuterCirclePoints(Vector2 centerPosition, float radius)
        {
            return GetOuterCirclePoints(centerPosition, radius, outerCirclePointStep);
        }
    }
}
