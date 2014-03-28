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
using System.Xml.Linq;

namespace EdgeLibrary
{
    public class AnimationIndex
    {
        public float SwitchTime;
        public int currentTexture;

        protected List<Texture2D> textures;

        private float elapsedSinceSwitch;

        public AnimationIndex()
        {
            SwitchTime = 100;
            textures = new List<Texture2D>();
        }

        public AnimationIndex(int switchTime)
            : this()
        {
            SwitchTime = switchTime;
        }

        public static AnimationIndex FromXMLSpriteSheet(string xmlPath, string texturePath, int switchTime)
        {
            AnimationIndex index = new AnimationIndex(switchTime);
            foreach (var kvp in TextureTools.SplitSpritesheet(texturePath, xmlPath))
            {
                index.textures.Add(kvp.Value);
            }
            return index;
        }

        public void AddTexture(Texture2D texture)
        {
            textures.Add(texture);
        }

        public void AddTexture(string texture)
        {
            textures.Add(Resources.TextureFromString(texture));
        }

        public bool RemoveTexture(Texture2D texture)
        {
            return textures.Remove(texture);
        }

        public void Clear()
        {
            textures.Clear();
        }

        public virtual void Update(Sprite s, GameTime gameTime)
        {
            elapsedSinceSwitch += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedSinceSwitch >= SwitchTime)
            {
                elapsedSinceSwitch = 0;

                currentTexture++;
                if (currentTexture > textures.Count)
                {
                    currentTexture = 0;
                }
            }

            s.Texture = textures[currentTexture];
        }
    }
}
