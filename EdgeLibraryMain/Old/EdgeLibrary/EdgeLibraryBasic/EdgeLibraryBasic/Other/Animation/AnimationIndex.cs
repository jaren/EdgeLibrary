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

    //An animation index which doesn't support spritesheets
    //Advantages of this - loopRate can be specified between frames
    //Disadvantages - textures must be loaded individually
    public class AnimationIndex : AnimationBase
    {
        public List<int> TextureTimes;
        public event AnimationEvent FinishedAnimation;

        public AnimationIndex() : base()
        {
            TextureTimes = new List<int>();
            elapsedSinceLastSwitch = 0;
            currentTexture = 1;
            HasRunThrough = false;
            ShouldRepeat = true;
        }

        public AnimationIndex(int loopRate, List<string> textures) : this()
        {
            for (int i = 0; i < textures.Count; i++)
            {
                TexturResourceData.Add(textures[i]);
                TextureTimes.Add(loopRate);
            }
        }

        public AnimationIndex(int loopRate, params string[] textures) : this(loopRate, new List<string>(textures))
        {
        }

        public override Rectangle getTextureBox()
        {
            if (!RunBackwards)
            {
                return new Rectangle(0, 0, Textures[currentTexture].Width, Textures[currentTexture].Height);
            }
            else
            {
                return new Rectangle(0, 0, Textures[Textures.Count - currentTexture].Width, Textures[Textures.Count - currentTexture].Height);
            }
        }

        public override void Reset()
        {
            HasRunThrough = false;
            currentTexture = 1;
        }

        public override Texture2D Update(UpdateArgs updateArgs)
        {
            if (!HasRunThrough || ShouldRepeat)
            {
                elapsedSinceLastSwitch += updateArgs.gameTime.ElapsedGameTime.Milliseconds;

                if (elapsedSinceLastSwitch >= TextureTimes[currentTexture])
                {
                    if (currentTexture >= Textures.Count)
                    {
                        currentTexture = 1;
                        HasRunThrough = true;
                        if (FinishedAnimation != null)
                        {
                            FinishedAnimation(this);
                        }
                    }
                    else
                    {
                        currentTexture++;
                    }

                    elapsedSinceLastSwitch = 0;
                }

                if (!RunBackwards)
                {
                    return Textures[currentTexture];
                }
                else
                {
                    return Textures[Textures.Count - currentTexture];
                }
            }
            else
            {
                if (ShowBlankOnFinish)
                {
                    return TextureTools.Blank;
                }
                else
                {
                    return Textures[0];
                }
            }
        }
    }
}
