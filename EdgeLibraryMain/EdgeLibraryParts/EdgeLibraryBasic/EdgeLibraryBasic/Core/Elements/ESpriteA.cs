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

namespace EdgeLibrary.Basic
{
    //A sprite with animation capabilities
    public class ESpriteA : ESprite
    {
        public Dictionary<string, EAnimationBase> Animations;
        public string selectedAnimation {get; protected set;}

        private string normalAnimation = "normal";

        public ESpriteA(EAnimationBase textures, Vector2 ePosition, int eWidth, int eHeight) : base("", ePosition, eWidth, eHeight)
        {
            Animations = new Dictionary<string, EAnimationBase>();
            Animations.Add(normalAnimation, textures);
            selectedAnimation = normalAnimation;
        }

        public ESpriteA(EAnimationIndex textures, Vector2 ePosition, int eWidth, int eHeight, Color eColor, float eRotation, Vector2 eScale) : this(textures, ePosition, eWidth, eHeight)
        {
            Color = eColor;
            Rotation = eRotation;
            Scale = eScale;
        }

        public bool SelectAnimation(string key)
        {
            if (Animations.Keys.Contains(key))
            {
                selectedAnimation = key;
                return true;
            }
            return false;
        }

        public void AddAnimation(string key, EAnimationIndex animation)
        {
            Animations.Add(key, animation);
        }

        public override void updateElement(EUpdateArgs updateArgs)
        {
            base.updateElement(updateArgs);

            Texture = Animations[selectedAnimation].Update(updateArgs);
        }

        public override void FillTexture(EData eData)
        {
            foreach(EAnimationBase animationIndex in Animations.Values)
            {
                animationIndex.FillTexture(eData);
            }
        }

        public override void drawElement(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Rectangle origin = Animations[selectedAnimation].getTextureBox();

            switch (DrawType)
            {
                case ESpriteDrawType.NoRatio:
                    base.DrawToSpriteBatch(spriteBatch, origin, Texture, BoundingBox, Color, Rotation);
                    break;
                case ESpriteDrawType.KeepHeight:
                    base.DrawToSpriteBatchWithHeight(spriteBatch, origin, Texture, Height, Color, Rotation);
                    break;
                case ESpriteDrawType.KeepWidth:
                    base.DrawToSpriteBatchWithHeight(spriteBatch, origin, Texture, Width, Color, Rotation);
                    break;
                case ESpriteDrawType.Scaled:
                    base.DrawToSpriteBatchWithScale(spriteBatch, origin, Texture, ScaledDrawScale, Color, Rotation);
                    break;
            }
        }
    }
}
