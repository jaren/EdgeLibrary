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

namespace EdgeLibrary.Edge
{

    public class EActionMove : EAction
    {
        public Vector2 targetPos { get; set; }
        public float speed { get; set; }

        protected Vector2 moveVelocity;

        public EActionMove(Vector2 eTargetPos, float eSpeed)
        {
            targetPos = eTargetPos;
            speed = eSpeed;
            initVars();
        }

        public EActionMove(EActionMove action) : this(action.targetPos, action.speed)
        {
            //Pass-through constructor
        }

        protected void initVars()
        {
            requiresUpdate = true;
            moveVelocity = Vector2.Zero;
        }

        protected bool checkIfEnd(ESprite targetSprite)
        {
            if (targetSprite.Position == targetPos) { return true; }
            else
            {
                if ((moveVelocity.X > 0) && (targetSprite.Position.X > targetPos.X - moveVelocity.X))
                {
                    targetSprite.Position = targetPos;
                    return true;
                }
                else if ((moveVelocity.X < 0) && (targetSprite.Position.X < targetPos.X + moveVelocity.X))
                {
                    targetSprite.Position = targetPos;
                    return true;
                }

                if ((moveVelocity.Y > 0) && (targetSprite.Position.Y > targetPos.Y - moveVelocity.Y))
                {
                    targetSprite.Position = targetPos;
                    return true;
                }
                else if ((moveVelocity.Y < 0) && (targetSprite.Position.Y < targetPos.Y + moveVelocity.Y))
                {
                    targetSprite.Position = targetPos;
                    return true;
                }
            }
            return false;
        }

        public override void initWithSprite(ESprite targetSprite)
        {
            moveVelocity = new Vector2(targetPos.X - targetSprite.Position.X, targetPos.Y - targetSprite.Position.Y);
            moveVelocity.Normalize();
        }

        public override bool UpdateAction(ESprite targetSprite)
        {
            if (checkIfEnd(targetSprite))
            {
                return true;
            }

            targetSprite.Position += moveVelocity * speed;

            return false;
        }
    }

}
