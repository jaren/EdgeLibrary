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
    public class BlackWhiteEffect : Effect
    {
        public override void ApplyEffect(Texture2D texture)
        {
            Color[] colors = new Color[texture.Width*texture.Height];
            texture.GetData<Color>(colors);

            for (int i = 0; i < colors.Length; i++ )
            {
                byte colorAvg = (byte)((colors[i].R + colors[i].G + colors[i].B) / 3);
                colors[i].R = colorAvg;
                colors[i].G = colorAvg;
                colors[i].B = colorAvg;
            }
        }
    }
}
