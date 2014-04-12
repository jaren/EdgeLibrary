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
    //Creates a simple fade effect between two scenes
    public class FadeTransition : Transition
    {
        public FadeTransition(string id, Scene a, Scene b, float timePerFrame, int frames) : base(id, a, b, timePerFrame, frames) { }

        public override void GenerateFrames()
        {
            for (int f = 0; f < Frames; f++)
            {
                Color[] colors = new Color[Texture1.Width * Texture1.Height];

                for (int i = 0; i < ColorArray1.Length; i++ )
                {
                    colors[i] = Color.Lerp(ColorArray1[i], ColorArray2[i], f / (float)Frames);
                }
                
                Colors.Add(colors);
            }
        }
    }
}
