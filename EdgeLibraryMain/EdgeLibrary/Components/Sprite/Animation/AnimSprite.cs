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
        private Dictionary<string, AnimationIndex> Animations;
        public string SelectedAnimation;

        public AnimSprite(string textureName, Vector2 position) : base(textureName, position)
        {
            Animations = new Dictionary<string, AnimationIndex>();
        }

        public void SelectAnimation(string animation)
        {
            SelectedAnimation = animation;
        }

        public void AddAnimation(string id, AnimationIndex animation)
        {
            Animations.Add(id, animation);
        }

        public void RemoveAnimation(string id)
        {
            Animations.Remove(id);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Animations[SelectedAnimation].Update(this, gameTime);
        }

        public override object Clone()
        {
            AnimSprite clone = (AnimSprite)base.Clone();
            clone.Animations = new Dictionary<string, AnimationIndex>();
            foreach (KeyValuePair<string, AnimationIndex> animation in Animations)
            {
                clone.Animations.Add(animation.Key, animation.Value);
            }
            return clone;
        }
    }
}
