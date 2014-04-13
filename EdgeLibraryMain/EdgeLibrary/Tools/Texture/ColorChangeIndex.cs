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
    /// <summary>
    /// Decides how a sprite or particle will change colors
    /// </summary>
    public struct ColorChangeIndex
    {
        //The colors and times showing how to change the color and at how long
        public List<Color> Colors;
        public List<float> Times;
        public int index;
        private double elapsedTime;
        public bool HasFinished { get; private set; }

        public ColorChangeIndex(Color color) : this(1000, color) { }

        public ColorChangeIndex(params KeyValuePair<Color, float>[] pairs) : this()
        {
            Colors = new List<Color>();
            Times = new List<float>();
            index = 0;
            elapsedTime = 0;

            HasFinished = false;

            foreach (KeyValuePair<Color, float> pair in pairs)
            {
                Add(pair.Key, pair.Value);
            }
        }

        public ColorChangeIndex(float time, params Color[] colors) : this()
        {
            Colors = new List<Color>();
            Times = new List<float>();
            index = 0;
            elapsedTime = 0;

            HasFinished = false;

            foreach (Color color in colors)
            {
                Add(color, time);
            }
        }

        //Clears the colors and times
        public void Clear()
        {
            Colors.Clear();
            Times.Clear();
        }

        //Set the specific index
        public void Set(int index, Color color, float time)
        {
            Colors[index] = color;
            Times[index] = time;
        }

        //Set the time at a specific index
        public void SetTime(int index, float time)
        {
            Times[index] = time;
        }

        //Set the color at a specific index
        public void SetColor(int index, Color color)
        {
            Colors[index] = color;
        }

        //Adds a new color/time
        public void Add(Color color, float time)
        {
            Colors.Add(color);
            Times.Add(time);
        }

        //Adds a new color with 1000 as the time
        public void Add(Color color)
        {
            Add(color, 1000);
        }

        //Removes a certain color
        public void Remove(Color color)
        {
            Colors.Remove(color);
        }

        //Creates a random index between two other indexes
        public static ColorChangeIndex Lerp(ColorChangeIndex index1, ColorChangeIndex index2, float value)
        {
            ColorChangeIndex index = new ColorChangeIndex(MathHelper.Lerp(index1.Times[0], index2.Times[0], value), Color.Lerp(index1.Colors[0], index2.Colors[0], value));
            for (int i = 1; i < index1.Colors.Count; i++)
            {
                index.Add(Color.Lerp(index1.Colors[i], index2.Colors[i], value), MathHelper.Lerp(index1.Times[i], index2.Times[i], value));
            }
            return index;
        }

        //Updates the color index
        public Color Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds * EdgeGame.GetFrameTimeMultiplier(gameTime);

            if (elapsedTime >= Times[index])
            {
                if (index < Colors.Count - 1)
                {
                    index++;
                }
                elapsedTime = 0;
            }

            if (index >= Colors.Count - 1)
            {
                HasFinished = true;
                return Colors[index];
            }
            else
            {
                HasFinished = false;
            }
            return Color.Lerp(Colors[index], Colors[index + 1], (float)elapsedTime / Times[index]);
        }
    }
}
