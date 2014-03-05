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
    public enum ShadeTypes
    {
        Top,
        Bottom,
        Left,
        Right
    }

    //Work in progress
    public class ShadeEffect : Effect
    {
        public ShadeTypes ShadeType;
        public byte startDarkness;
        public byte endDarkness;

        public ShadeEffect(ShadeTypes shadeType, byte start, byte end)
        {
            ShadeType = shadeType;
            startDarkness = start;
            endDarkness = end;
        }

        public override void ApplyEffect(Texture2D texture)
        {
            Color[] colors = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(colors);

            byte currentDarkness = 0;

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    switch (ShadeType)
                    {
                        case ShadeTypes.Top:
                            currentDarkness = (byte)MathHelper.Lerp(startDarkness, endDarkness, y/texture.Height);
                            break;
                        case ShadeTypes.Bottom:
                            currentDarkness = (byte)MathHelper.Lerp(startDarkness, endDarkness, (texture.Height - y) / texture.Height);
                            break;
                        case ShadeTypes.Left:
                            currentDarkness = (byte)MathHelper.Lerp(startDarkness, endDarkness, x / texture.Width);
                            break;
                        case ShadeTypes.Right:
                            currentDarkness = (byte)MathHelper.Lerp(startDarkness, endDarkness, (texture.Width - x) / texture.Width);
                            break;
                    }
                    colors[x + y * texture.Width] = MathTools.AddToColor(colors[x + y * texture.Width], currentDarkness);
                }
            }

            texture.SetData<Color>(colors);
        }
    }
}
