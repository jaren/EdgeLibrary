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
    public class ESpriteA : ESprite
    {
        public EAnimationIndex TextureIndex;

        public ESpriteA(EAnimationIndex textures, Vector2 ePosition, int eWidth, int eHeight) : base("", ePosition, eWidth, eHeight)
        {
            TextureIndex = textures;
        }

        public ESpriteA(EAnimationIndex textures, Vector2 ePosition, int eWidth, int eHeight, Color eColor, float eRotation, Vector2 eScale) : this(textures, ePosition, eWidth, eHeight)
        {
            Color = eColor;
            Rotation = eRotation;
            Scale = eScale;
        }

        public override void updateElement(EUpdateArgs updateArgs)
        {
            base.updateElement(updateArgs);

            Texture = TextureIndex.Update(updateArgs);
        }
    }
}
