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
        public List<string> TexturEData;
        public List<Texture2D> Textures;
        public bool RunBackwards;
        public bool ShowBlankOnFinish;
        public bool ShouldRepeat;
        public bool HasRunThrough { get; protected set; }
        public int currentTexture;
        protected float elapsedSinceLastSwitch;

        public delegate void AnimationEvent(EAnimationBase e);

        public EAnimationBase()
        {
            TexturEData = new List<string>();
            Textures = new List<Texture2D>();
            RunBackwards = false;
            ShowBlankOnFinish = false;
        }

        public virtual void FillTexture()
        {
            try
            {
                Textures.Clear();

                foreach (string texturEData in TexturEData)
                {
                    Textures.Add(EData.getTexture(texturEData));
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
