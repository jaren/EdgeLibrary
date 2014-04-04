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
    //Generates multiple types of textures, such as gradients, circles, etc.
    public static class TextureGeneratorTools
    {
        public static Texture2D GenerateGradient(Color color1, Color color2, int width, int height, bool vertical)
        {
            Texture2D texture = EdgeGame.CreateNewTexture(width, height);
            Color[] colors = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (vertical)
                    {
                        colors[x + y * width] = Color.Lerp(color1, color2, y / (float)height);
                    }
                    else
                    {
                        colors[x + y * width] = Color.Lerp(color1, color2, x / (float)width);
                    }
                }
            }
            texture.SetData<Color>(colors);
            return texture;
        }

        public static Texture2D GenerateCircle(int radius, Color color)
        {
            Texture2D texture = EdgeGame.CreateNewTexture(radius * 2, radius * 2);
            Color[] colors = new Color[radius*radius*4];
            foreach(Vector2 point in MathTools.GetCirclePoints(new Vector2(radius, radius), radius))
            {
                colors[(int)point.X + (int)point.Y * radius * 2] = color;
            }
            texture.SetData<Color>(colors);
            return texture;
        }
    }
}
