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
    public class ETextureIndex : EObject
    {
        //Unfinished
        public List<string> TextureData;
        public List<Texture2D> Textures;

        public ETextureIndex()
        {
            TextureData = new List<string>();
            Textures = new List<Texture2D>();
        }

        public void Fill(EScene scene)
        {
            scene.FillTextureIndex(this);
        }

        public void Fill(EdgeGame game)
        {
            game.FillTextureIndex(this);
        }
    }

    public class EAnimationIndex : ETextureIndex
    {
        public List<int> TextureTimes;
        public int currentTexture;
        private float elapsedSinceLastSwitch;

        public EAnimationIndex() : base()
        {
            TextureTimes = new List<int>();
            elapsedSinceLastSwitch = 0;
            currentTexture = 0;
        }

        public EAnimationIndex(int loopRate, List<string> textures) : this()
        {
            for (int i = 0; i < textures.Count; i++)
            {
                TextureData.Add(textures[i]);
                TextureTimes.Add(loopRate);
            }
        }

        public EAnimationIndex(int loopRate, params string[] textures) : this(loopRate, new List<string>(textures))
        {
        }

        public Texture2D Update(EUpdateArgs updateArgs)
        {
            elapsedSinceLastSwitch += updateArgs.gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedSinceLastSwitch >= TextureTimes[currentTexture])
            {
                if (currentTexture >= Textures.Count - 1)
                {
                    currentTexture = 0;
                }
                else
                {
                    currentTexture++;
                }

                elapsedSinceLastSwitch = 0;
            }

            return Textures[currentTexture];
        }
    }
}
