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
    public class EActionRepeat : EAction
    {
        public int repeatTimes { get; set; }
        public int completedTimes { get; protected set; }
        public EAction action;

        public EActionRepeat(EActionRepeat eAction)
        {
            RequiresUpdate = true;
            repeatTimes = eAction.repeatTimes;
            action = eAction.action;
        }

        public EActionRepeat(int eRepeatTimes, EAction eAction)
        {
            RequiresUpdate = true;
            completedTimes = 0;
            repeatTimes = eRepeatTimes;
            action = eAction;
        }

        public override void PerformAction(ESprite targetSprite)
        {
            targetSprite.runAction(action);
        }

        public override bool Update(ESprite targetSprite)
        {
            if (action.Update(targetSprite))
            {
                completedTimes++;
                targetSprite.runAction(action);
            }

            if (completedTimes >= repeatTimes)
            {
                return true;
            }

            return false;
        }
    }

    public class EActionRepeatForever : EAction
    {
        public int completedTimes { get; protected set; }
        public EAction action;

        public EActionRepeatForever(EActionRepeatForever eAction) : this(eAction.action) { }

        public EActionRepeatForever(EAction eAction)
        {
            RequiresUpdate = true;

            completedTimes = 0;
            action = eAction;
        }

        public override void PerformAction(ESprite targetSprite)
        {
            targetSprite.runAction(action);
        }

        public override bool Update(ESprite targetSprite)
        {
            if (action.Update(targetSprite))
            {
                completedTimes++;
                targetSprite.runAction(action);
            }

            return false;
        }
    }
}
