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
using System.Xml;

namespace EdgeLibrary.Basic
{
    //Base for animations
    public class EAnimationBase : EObject
    {
        //Unfinished
        public List<string> TextureData;
        public List<Texture2D> Textures;
        public bool ShowBlankOnFinish;
        public bool ShouldRepeat;
        public bool HasRunThrough { get; protected set; }
        public int currentTexture;
        protected float elapsedSinceLastSwitch;

        public EAnimationBase()
        {
            TextureData = new List<string>();
            Textures = new List<Texture2D>();
            ShowBlankOnFinish = false;
        }

        public virtual void FillTexture(EData eData)
        {
            try
            {
                Textures.Clear();

                foreach (string textureData in TextureData)
                {
                    Textures.Add(eData.getTexture(textureData));
                }
            }
            catch
            { }
        }

        public virtual void Reset() { }

        public virtual Rectangle getTextureBox() { return new Rectangle(0, 0, 0, 0); }

        public virtual Texture2D Update(EUpdateArgs updateArgs) { return Textures[0]; }
    }
}
