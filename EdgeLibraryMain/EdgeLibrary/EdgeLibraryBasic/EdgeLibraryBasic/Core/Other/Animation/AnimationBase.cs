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

namespace EdgeLibrary
{
    //Base for animations
    public class AnimationBase : Object
    {
        //Unfinished
        public List<string> TexturResourceData;
        public List<Texture2D> Textures;
        public bool RunBackwards;
        public bool ShowBlankOnFinish;
        public bool ShouldRepeat;
        public bool HasRunThrough { get; protected set; }
        public int currentTexture;
        protected float elapsedSinceLastSwitch;

        public delegate void AnimationEvent(AnimationBase e);

        public AnimationBase()
        {
            TexturResourceData = new List<string>();
            Textures = new List<Texture2D>();
            RunBackwards = false;
            ShowBlankOnFinish = false;
        }

        public virtual void FillTexture()
        {
            try
            {
                Textures.Clear();

                foreach (string texturResourceData in TexturResourceData)
                {
                    Textures.Add(ResourceData.getTexture(texturResourceData));
                }
            }
            catch
            { }
        }

        public virtual void Reset() { }

        public virtual Rectangle getTextureBox() { return new Rectangle(0, 0, 0, 0); }

        public virtual Texture2D Update(UpdateArgs updateArgs) { return Textures[0]; }
    }
}
