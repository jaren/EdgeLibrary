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
    public static class MathExtensions
    {  
        private static Dictionary<Type, int> GivenIDs = new Dictionary<Type, int>();

        /// <summary>
        /// Generates a random ID based on the element's type and the number of previously given IDs of that type
        /// </summary>
        public static string GenerateID(this object obj)
        {
            //If no other elements of this type exist, then create an index for it
            if (!GivenIDs.ContainsKey(obj.GetType()))
            {
                GivenIDs.Add(obj.GetType(), 0);
            }
            GivenIDs[obj.GetType()]++;
            return obj.GetType().Name + GivenIDs[obj.GetType()].ToString();
        }

        /// <summary>
        /// Cross product of vectors
        /// </summary>
        public static float CrossProduct(this Vector2 a, Vector2 b)
        {
            return (a.X * b.Y) - (a.Y * b.X);
        }
        public static Vector2 CrossProduct(this Vector2 vector, float cross)
        {
            return new Vector2(cross * vector.Y, -cross * vector.X);
        }
        public static Vector2 CrossProduct(this float cross, Vector2 vector)
        {
            return new Vector2(-cross * vector.Y, cross * vector.X);
        }

        /// <summary>
        /// Dot product of vectors
        /// </summary>
        public static float DotProduct(this Vector2 a, Vector2 b)
        {
            return (a.X * b.X) + (a.Y * b.Y);
        }

        /// <summary>
        /// Returns the rotation for a sprite
        /// </summary>
        public static float GetRotationTowards(this Vector2 elementPos, Vector2 targetPos)
        {
            return MathHelper.ToDegrees((float)Math.Atan2(targetPos.Y - elementPos.Y, targetPos.X - elementPos.X));
        }

        /// <summary>
        /// Returns the position a certain distance relative to another at an angle
        /// </summary>
        public static Vector2 GetRelativePosition(this Vector2 target, float distance, float degrees)
        {
            return new Vector2((float)Math.Cos(MathHelper.ToRadians(degrees)) * distance + target.X, (float)Math.Sin(MathHelper.ToRadians(degrees)) * distance + target.Y);
        }

        /// <summary>
        /// Retrieves a color by its name.
        /// As this method uses a (expensive) reflection call, it should only be invoked at load time.
        /// If the color is known at compile time, a static property on the <see cref="Color"/> class should be used instead.
        /// </summary>
        public static Color ToColor(this string colorString)
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
        /// Sets a vector's values to be positive
        /// </summary>
        public static Vector2 Abs(this Vector2 vector)
        {
            return new Vector2(Math.Abs(vector.X), Math.Abs(vector.Y));
        }

        /// <summary>
        /// Gets a color from a hex string
        /// </summary>
        public static Color ToHex(this string hexString)
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
        public static Color Add(this Color color, int number)
        {
            color.R = color.R + number >= 0 ? color.R + number >= 256 ? (byte)255 : (byte)(color.R + number) : (byte)0;
            color.G = color.G + number >= 0 ? color.G + number >= 256 ? (byte)255 : (byte)(color.G + number) : (byte)0;
            color.B = color.B + number >= 0 ? color.B + number >= 256 ? (byte)255 : (byte)(color.B + number) : (byte)0;
            color.A = color.A + number >= 0 ? color.A + number >= 256 ? (byte)255 : (byte)(color.A + number) : (byte)0;
            return color;
        }

        /// <summary>
        /// Subtracts the specified amount from each component of the specified vector.
        /// Guarantees the sign of vector components (positive or negative) will remain the same after decrease.
        /// </summary>
        public static Vector2 Decrease(this Vector2 vector, float amount)
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
        public static Rectangle ResolveNegative(this Rectangle rectangle)
        {
            if (rectangle.Width < 0)
            {
                rectangle.Width = Math.Abs(rectangle.Width);
                rectangle.X -= rectangle.Width;
            }
            if (rectangle.Height < 0)
            {
                rectangle.Height = Math.Abs(rectangle.Height);
                rectangle.Y -= rectangle.Height;
            }
            return rectangle;
        }

        /// <summary>
        /// Returns the midpoint of a line segment drawn from one point to the other.
        /// </summary>
        public static Vector2 MidPoint(this Vector2 point1, Vector2 point2)
        {
            float diffX = point1.X - point2.X;
            float diffY = point1.Y - point2.Y;
            return new Vector2(point2.X + diffX / 2, point2.Y + diffY / 2);
        }

        //Used for a string such as: 'Planet/Country/State/City/Street/House' - this would return House
        public static string LastPortionOfPath(this string path)
        {
            return LastPortionOfPath(path, '/');
        }
        public static string LastPortionOfPath(this string path, char splitter)
        {
            string[] splitParts = path.Split(splitter);
            return splitParts[splitParts.Length - 1];
        }
    }
}
