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
    //Holds all the calculation and simple drawing functions
    public static class MathTools
    {
        public static void Init() { }

        public static float circlePointStep = 8;
        public static float outerCirclePointStep = 1;

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

        //Returns the average of the first number/second number with an "influence (percentage/100)" of number 2
        public static int SpecialAverage(int number1, int number2, int influence)
        {
            return (100 * number1 + influence * 100 * number2) / (100 + influence * 100);
        }

        public static float CenterXString(Label label)
        {
            return (EdgeGame.WindowSize.X - label.Font.MeasureString(label.Text).X ) / 2;
        }

        //Used for a string such as: 'Planet/Country/State/City/Street/House' - this would return House
        public static string LastPortionOfPath(string path)
        {
            string[] splitParts = path.Split('/');
            return splitParts[splitParts.Length - 1];
        }

        public static List<Vector2> GetCirclePoints(Vector2 centerPosition, float radius)
        {
            List<Vector2> points = new List<Vector2>();
            float actualStep = circlePointStep / radius;

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

        public static List<Vector2> GetOuterCirclePoints(Vector2 centerPosition, float radius)
        {
            List<Vector2> points = new List<Vector2>();
            float actualStep = outerCirclePointStep / radius;

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
    }
}
