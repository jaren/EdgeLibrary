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
    //Only used with sprites
    public class XMLAnimationCapability : Capability
    {
        private float elapsedSinceSwitch;

        public float SwitchTime;
        public List<Texture2D> textures;
        public int currentTexture;

        public XMLAnimationCapability(string xmlPath, string texturePath, int switchTime) : base("XMLAnimation")
        {
            SwitchTime = switchTime;
            textures = new List<Texture2D>();
            foreach (var kvp in TextureTools.SplitSpritesheet(texturePath, xmlPath))
            {
                textures.Add(kvp.Value);
            }
        }

        public override void Update(GameTime gameTime, Element element)
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

            ((Sprite)element).Texture = textures[currentTexture];
        }

        public override Capability NewInstance(Element e)
        {
            //This capability can not be included as a default capability
            return null;
        }
    }
}
