﻿using System;
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
    //An action to make a sprite move to a position at a speed
    public class EActionMove : EAction
    {
        public Vector2 targetPos { get; set; }
        private float speed { get; set; }

        protected Vector2 moveVelocity;

        public EActionMove(Vector2 eTargetPos, float eSpeed) : base()
        {
            targetPos = eTargetPos;
            speed = eSpeed;
            initVars();
        }

        protected void initVars()
        {
            RequiresUpdate = true;
            moveVelocity = Vector2.Zero;
        }

        protected bool checkIfEnd(Sprite targetSprite)
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

        public override void PerformAction(Sprite targetSprite)
        {
            moveVelocity = new Vector2(targetPos.X - targetSprite.Position.X, targetPos.Y - targetSprite.Position.Y);
            moveVelocity.Normalize();
            Update(targetSprite);
        }

        public override bool Update(Sprite targetSprite)
        {
            moveVelocity = new Vector2(targetPos.X - targetSprite.Position.X, targetPos.Y - targetSprite.Position.Y);
            moveVelocity.Normalize();

            if (checkIfEnd(targetSprite))
            {
                return true;
            }

            targetSprite.Position += moveVelocity * speed;

            return false;
        }
    }

}