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
    //An action to rotate a sprite at a constant speed
    public class EActionRotate : EAction
    {
        public float rotation { get; set; }
        public float speed { get; set; }

        protected float startRotation;

        public EActionRotate(float eRotation, float eSpeed) : base()
        {
            RequiresUpdate = true;
            rotation = eRotation;
            speed = Math.Abs(eSpeed);
        }

        public override void PerformAction(ESprite sprite)
        {
            startRotation = sprite.Rotation;
        }

        public override bool Update(ESprite targetSprite)
        {
            if ((rotation > 0) && (targetSprite.Rotation > (rotation + startRotation - speed))) return true;
            else if ((rotation < 0) && (targetSprite.Rotation < (rotation + startRotation + speed))) return true;

            targetSprite.Rotation += speed;
            return false;
        }
    }
}
