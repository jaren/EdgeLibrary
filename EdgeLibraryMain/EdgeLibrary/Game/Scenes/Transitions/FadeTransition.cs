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
        public FadeTransition(Scene a, Scene b, float timePerFrame, int frames) : base(a, b, timePerFrame, frames) { }

        public override void GenerateFrame()
        {
            for (int i = 0; i < ColorArray1.Length; i++ )
            {
                CurrentColors[i] = Color.Lerp(ColorArray1[i], ColorArray2[i], currentFrame / (float)Frames);
            }
        }
    }
}
