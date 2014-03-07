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
    public struct ColorChangeIndex
    {
        private List<Color> Colors;
        private List<float> Times;
        private int index;
        private float elapsedTime;

        public ColorChangeIndex(Color color) : this(color, 1000) { }

        public ColorChangeIndex(Color color, float time)
        {
            Colors = new List<Color>();
            Times = new List<float>();
            index = 0;
            elapsedTime = 0;
            Add(color, time);
        }

        public void Clear()
        {
            Colors.Clear();
            Times.Clear();
        }

        public void Add(Color color, float time)
        {
            Colors.Add(color);
            Times.Add(time);
        }

        public void Add(Color color)
        {
            Add(color, 1000);
        }

        public void Remove(Color color)
        {
            Colors.Remove(color);
        }

        public static ColorChangeIndex Lerp(ColorChangeIndex index1, ColorChangeIndex index2, float value)
        {
            ColorChangeIndex index = new ColorChangeIndex(Color.Lerp(index1.Colors[0], index2.Colors[0], value), MathHelper.Lerp(index1.Times[0], index2.Times[0], value));
            for (int i = 1; i < index1.Colors.Count; i++)
            {
                index.Add(Color.Lerp(index1.Colors[i], index2.Colors[i], value), MathHelper.Lerp(index1.Times[i], index2.Times[i], value));
            }
            return index;
        }

        public Color Update(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime >= Times[index])
            {
                if (index < Colors.Count)
                {
                    index++;
                }
                elapsedTime = 0;
            }

            if (index >= Colors.Count - 1)
            {
                return Color.Lerp(Colors[index], Colors[index], elapsedTime / Times[index]);
            }
            return Color.Lerp(Colors[index], Colors[index + 1], elapsedTime / Times[index]);
        }
    }
}
