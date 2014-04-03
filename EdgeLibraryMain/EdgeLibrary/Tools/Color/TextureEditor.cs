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
    //A class to edit textures
    public class TextureEditor
    {
        //The event which is called for every pixel on the texture
        public delegate void ColorLoopEvent(ref Color color, int x, int y);
        public event ColorLoopEvent OnColorCheck = delegate { };

        //Applies the texture editor to a certain texture, modifying it
        public void ApplyTo(Texture2D texture)
        {
            Color[] colors = new Color[texture.Width*texture.Height];
            texture.GetData<Color>(colors);
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    OnColorCheck(ref colors[x + y * texture.Width], x, y);
                }
            }
            texture.SetData<Color>(colors);
        }

        //Applies the texture editor to a certain texture but doesn't modify it and returns the new texture
        public Texture2D GetApplied(Texture2D texture)
        {
            Texture2D returnTexture = TextureTools.Copy(texture);
            ApplyTo(returnTexture);
            return returnTexture;
        }
    }
}
