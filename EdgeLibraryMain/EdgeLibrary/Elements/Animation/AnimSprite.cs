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
    public class AnimSprite : Sprite
    {
        private Dictionary<string,AnimationIndex> animations;
        public string SelectedAnimation;

        public AnimSprite(string id, string eTextureName, Vector2 ePosition) : base(id, eTextureName, ePosition)
        {
            animations = new Dictionary<string, AnimationIndex>();
        }

        public void SelectAnimation(string animation)
        {
            SelectedAnimation = animation;
        }

        public void AddAnimation(string id, AnimationIndex animation)
        {
            animations.Add(id, animation);
        }

        public void RemoveAnimation(string id)
        {
            animations.Remove(id);
        }

        public override void updateElement(GameTime gameTime)
        {
            base.updateElement(gameTime);

            animations[SelectedAnimation].Update(this, gameTime);
        }
    }
}
