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
using FarseerPhysics;
using System.Collections;

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
        /// Shuffles a list randomly
        /// </summary>
        public static List<T> Shuffle<T>(this List<T> list)
        {
            List<T> newList = new List<T>();

            //Generates a list of all the numbers in the list's count
            List<int> availableIndices = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                availableIndices.Add(i);
            }

            //Adds the objects randomly to the new list
            for (int i = 0; i < list.Count; i++)
            {
                int chosenIndex = RandomTools.RandomInt(0, availableIndices.Count);
                newList.Add(list[availableIndices[chosenIndex]]);
                availableIndices.Remove(availableIndices[chosenIndex]);
            }

            return newList;
        }

        /// <summary>
        /// Returns false if an object equals any of the other ones
        /// </summary>
        public static bool EqualsAnyOf(this Object obj, params Object[] objects)
        {
            foreach(Object obj2 in objects)
            {
                if (obj == obj2)
                {
                    return true;
                }
            }
            return false;
        }

        public static string ToCorrectString(this Keys key, bool shifted = false)
        {
            switch (key)
            {
                case Keys.Space:
                    return " ";
                    break;
                case Keys.Tab:
                    return "    ";
                    break;
                case Keys.OemPlus:
                    return shifted ? "+" : "=";
                    break;
                case Keys.OemMinus:
                    return shifted ? "_" : "-";
                    break;
                case Keys.OemTilde:
                    return shifted ? "~" : "`";
                    break;
                case Keys.OemBackslash:
                    return shifted ? "|" : "\\";
                    break;
                case Keys.OemQuotes:
                    return shifted ? "\"" : "\'";
                    break;
                case Keys.OemSemicolon:
                    return shifted ? ":" : ";";
                    break;
                case Keys.OemPeriod:
                    return shifted ? ">" : ".";
                    break;
                case Keys.OemComma:
                    return shifted ? "<" : ",";
                    break;
                case Keys.OemQuestion:
                    return shifted ? "?" : "/";
                    break;
                case Keys.D0:
                    return shifted ? ")" : "0";
                    break;
                case Keys.D1:
                    return shifted ? "!" : "1";
                    break;
                case Keys.D2:
                    return shifted ? "@" : "2";
                    break;
                case Keys.D3:
                    return shifted ? "#" : "3";
                    break;
                case Keys.D4:
                    return shifted ? "$" : "4";
                    break;
                case Keys.D5:
                    return shifted ? "%" : "5";
                    break;
                case Keys.D6:
                    return shifted ? "^" : "6";
                    break;
                case Keys.D7:
                    return shifted ? "&" : "7";
                    break;
                case Keys.D8:
                    return shifted ? "*" : "8";
                    break;
                case Keys.D9:
                    return shifted ? "(" : "9";
                    break;
            }

            if (key >= Keys.A && key <= Keys.Z)
            {
                return shifted ? key.ToString() : key.ToString().ToLower();
            }

            if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
            {
                return key.ToString().Remove(0, 6);
            }

            return "";
        }

        /// <summary>
        /// Returns the rotation for a vector (the vector should be a a position - another position)
        /// </summary>
        public static float ToRotation(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        /// <summary>
        /// Turns an angle measure into a difference in X and Y
        /// </summary>
        public static Vector2 ToVector(this float radians)
        {
            return new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
        }

        /// <summary>
        /// Returns the position a certain distance relative to another at an angle
        /// </summary>
        public static Vector2 PositionRelativeTo(this Vector2 target, float distance, float radians)
        {
            return new Vector2((float)Math.Cos(radians) * distance + target.X, (float)Math.Sin(radians) * distance + target.Y);
        }

        /// <summary>
        /// Retrieves a type by its name. For example, to colors (Color.Blue, etc.)
        /// As this method uses a (expensive) reflection call, it should only be invoked at load time.
        /// </summary>
        public static T ConvertFromProperty<T>(this string name)
        {
            var typeProperty = typeof(T).GetProperty(name);
            if (typeProperty != null)
            {
                return (T)typeProperty.GetValue(null, null);
            }
            else
            {
                return default(T);
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
        public static Color FromHex(this string hexString)
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
        /// Converts a number from degrees to radians
        /// </summary>
        public static float ToRadians(this float degrees)
        {
            return MathHelper.ToRadians(degrees);
        }
        public static float ToRadians(this int degrees)
        {
            return MathHelper.ToRadians(degrees);
        }

        /// <summary>
        /// Converts a number from radians to degrees
        /// </summary>
        public static float ToDegrees(this float radians)
        {
            return MathHelper.ToDegrees(radians);
        }
        public static float ToDegrees(this int radians)
        {
            return MathHelper.ToDegrees(radians);
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

        //Used for a string such as: 'Planet/Country/State/City/Street/House' - this would return House
        public static string LastSplit(this string path, char splitter)
        {
            string[] splitParts = path.Split(splitter);
            return splitParts[splitParts.Length - 1];
        }

        //Used for converting between sim units and display units
        #region FarseerPhysics
        public static float ToDisplayUnits(this float simUnits)
        {
            return ConvertUnits.ToDisplayUnits(simUnits);
        }
        public static float ToDisplayUnits(this int simUnits)
        {
            return ConvertUnits.ToDisplayUnits(simUnits);
        }
        public static Vector2 ToDisplayUnits(this Vector2 simUnits)
        {
            return ConvertUnits.ToDisplayUnits(simUnits);
        }
        public static Vector3 ToDisplayUnits(this Vector3 simUnits)
        {
            return ConvertUnits.ToDisplayUnits(simUnits);
        }

        public static double ToSimUnits(this double displayUnits)
        {
            return ConvertUnits.ToSimUnits(displayUnits);
        }
        public static float ToSimUnits(this float displayUnits)
        {
            return ConvertUnits.ToSimUnits(displayUnits);
        }
        public static float ToSimUnits(this int displayUnits)
        {
            return ConvertUnits.ToSimUnits(displayUnits);
        }
        public static Vector2 ToSimUnits(this Vector2 displayUnits)
        {
            return ConvertUnits.ToSimUnits(displayUnits);
        }
        public static Vector3 ToSimUnits(this Vector3 displayUnits)
        {
            return ConvertUnits.ToSimUnits(displayUnits);
        }

        #endregion
    }
}
