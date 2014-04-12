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
    public class FadeTransition : Transition
    {
        private Color[] ColorArray1;
        private Color[] ColorArray2;
        private Color[] CurrentColorArray;

        public FadeTransition(string id, Scene a, Scene b, float time) : base(id, a, b, time) 
        {
            ColorArray1 = new Color[Texture1.Width * Texture1.Height];
            ColorArray2 = new Color[Texture1.Width * Texture1.Height];
            Texture1.GetData<Color>(ColorArray1);
            Texture2.GetData<Color>(ColorArray2);
            CurrentColorArray = ColorArray1;
        }

        public override void UpdateTransition(GameTime gameTime)
        {
            for (int i = 0; i < ColorArray1.Length; i++)
            {
                CurrentColorArray[i] = Color.Lerp(ColorArray1[i], ColorArray2[i], (float)(elapsedTime/Time));
            }
            Background.SetData<Color>(CurrentColorArray);
        }
    }
}
