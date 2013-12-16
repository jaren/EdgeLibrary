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
    public class EActionFollow : EAction
    {
        public ESprite spriteToFollow { get; set; }
        public float speed { get; set; }
        protected EActionMove moveAction;

        public EActionFollow(ESprite eSpriteToFollow, float eSpeed)
        {
            requiresUpdate = true;
            spriteToFollow = eSpriteToFollow;
            speed = eSpeed;
            moveAction = new EActionMove(spriteToFollow.Position, speed);
        }

        public override bool UpdateAction(ESprite targetSprite)
        {
            if (spriteToFollow == null) 
            {
                return true;
            }

            moveAction.cancel();
            moveAction = new EActionMove(spriteToFollow.Position, speed);
            targetSprite.runAction(moveAction);

            return false;
        }
    }
}
