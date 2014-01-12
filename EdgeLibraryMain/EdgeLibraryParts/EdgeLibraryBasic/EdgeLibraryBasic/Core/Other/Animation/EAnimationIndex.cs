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

    //An animation index which doesn't support spritesheets
    //Advantages of this - loopRate can be specified between frames
    //Disadvantages - textures must be loaded individually
    public class EAnimationIndex : EAnimationBase
    {
        public List<int> TextureTimes;

        public EAnimationIndex()
            : base()
        {
            TextureTimes = new List<int>();
            elapsedSinceLastSwitch = 0;
            currentTexture = 0;
            HasRunThrough = false;
            ShouldRepeat = true;
        }

        public EAnimationIndex(int loopRate, List<string> textures)
            : this()
        {
            for (int i = 0; i < textures.Count; i++)
            {
                TextureData.Add(textures[i]);
                TextureTimes.Add(loopRate);
            }
        }

        public EAnimationIndex(int loopRate, params string[] textures)
            : this(loopRate, new List<string>(textures))
        {
        }

        public override Rectangle getTextureBox()
        {
            return new Rectangle(0, 0, Textures[currentTexture].Width, Textures[currentTexture].Height);
        }

        public override void Reset()
        {
            HasRunThrough = false;
            currentTexture = 0;
        }

        public override Texture2D Update(EUpdateArgs updateArgs)
        {
            if (!HasRunThrough || ShouldRepeat)
            {
                elapsedSinceLastSwitch += updateArgs.gameTime.ElapsedGameTime.Milliseconds;

                if (elapsedSinceLastSwitch >= TextureTimes[currentTexture])
                {
                    if (currentTexture >= Textures.Count - 1)
                    {
                        currentTexture = 0;
                        HasRunThrough = true;
                    }
                    else
                    {
                        currentTexture++;
                    }

                    elapsedSinceLastSwitch = 0;
                }

                return Textures[currentTexture];
            }
            else
            {
                if (ShowBlankOnFinish)
                {
                    return EMath.Blank;
                }
                else
                {
                    return Textures[0];
                }
            }
        }
    }
}
