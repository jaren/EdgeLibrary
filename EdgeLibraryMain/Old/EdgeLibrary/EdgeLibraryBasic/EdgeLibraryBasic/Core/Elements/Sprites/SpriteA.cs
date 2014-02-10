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

namespace EdgeLibrary
{
    //A sprite with animation capabilities
    public class SpriteA : Sprite
    {
        public Dictionary<string, AnimationBase> Animations;
        public string selectedAnimation {get; protected set;}

        public SpriteA(AnimationBase textures, string texturesID, Vector2 ePosition) : base("", ePosition)
        {
            Animations = new Dictionary<string, AnimationBase>();
            Animations.Add(texturesID, textures);
            selectedAnimation = texturesID;

            _width = 0;
            _height = 0;
        }

        public SpriteA(AnimationBase textures, string texturesID, Vector2 ePosition, int eWidth, int eHeight) : this(textures, texturesID, ePosition)
        {
            _width = eWidth;
            _height = eHeight;
            reloadBoundingBox();
        }

        public SpriteA(AnimationIndex textures, string texturesID, Vector2 ePosition, int eWidth, int eHeight, Color eColor, float eRotation, Vector2 eScale) : this(textures, texturesID, ePosition, eWidth, eHeight)
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
                Animations[selectedAnimation].Reset();
                return true;
            }
            return false;
        }

        public void ResetAnimation()
        {
            Animations[selectedAnimation].Reset();
        }

        public void AddAnimation(string key, AnimationBase animation)
        {
            Animations.Add(key, animation);
        }

        public override void updatElement(UpdateArgs updateArgs)
        {
            base.updatElement(updateArgs);

            Texture = Animations[selectedAnimation].Update(updateArgs);

            _width = Animations[selectedAnimation].getTextureBox().Width;
            _height = Animations[selectedAnimation].getTextureBox().Height;
        }

        public override void FillTexture()
        {
            foreach(AnimationBase animationIndex in Animations.Values)
            {
                animationIndex.FillTexture();
            }

            //Doesn't work with spritesheet animations
            if (_width == 0)
            {
                _width = Animations.Values.First().Textures[0].Width;
            }
            if (_height == 0)
            {
                _height = Animations.Values.First().Textures[0].Height;
            }
            reloadBoundingBox();
        }

        public override void drawElement(GameTime gameTime)
        {
            Rectangle origin = Animations[selectedAnimation].getTextureBox();

            switch (DrawType)
            {
                case SpriteDrawType.NoRatio:
                    base.DrawTexture(origin, Texture, BoundingBox, Color, Rotation, spriteEffects);
                    break;
                case SpriteDrawType.KeepHeight:
                    base.DrawWithHeight(origin, Texture, Height, Color, Rotation, spriteEffects);
                    break;
                case SpriteDrawType.KeepWidth:
                    base.DrawWithWidth(origin, Texture, Width, Color, Rotation, spriteEffects);
                    break;
                case SpriteDrawType.Scaled:
                    base.DrawWithScale(origin, Texture, ScaledDrawScale, Color, Rotation, spriteEffects);
                    break;
            }
        }
    }
}
