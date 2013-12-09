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
    public class EActionRepeat : EAction
    {
        public int repeatTimes { get; set; }
        public int completedTimes { get; protected set; }
        public EAction action;
        protected ESprite sprite;

        public EActionRepeat(EActionRepeat eAction)
        {
            requiresUpdate = true;
            repeatTimes = eAction.repeatTimes;
            action = eAction.action;
        }

        public EActionRepeat(int eRepeatTimes, EAction eAction)
        {
            requiresUpdate = true;
            completedTimes = 0;
            repeatTimes = eRepeatTimes;
            action = eAction;
        }

        public override void initWithSprite(ESprite eSprite)
        {
            sprite.runAction(action);
        }

        public override bool UpdateAction(ESprite targetSprite)
        {
            if (action.Update(targetSprite))
            {
                completedTimes++;
                sprite.runAction(action);
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
        protected ESprite sprite;

        public EActionRepeatForever(EActionRepeatForever eAction) : this(eAction.action) { }

        public EActionRepeatForever(EAction eAction)
        {
            requiresUpdate = true;

            completedTimes = 0;
            action = eAction;
        }

        public override void initWithSprite(ESprite eSprite)
        {
            sprite.runAction(action);
        }

        public override bool UpdateAction(ESprite targetSprite)
        {
            if (action.Update(targetSprite))
            {
                completedTimes++;
                sprite.runAction(action);
            }

            return false;
        }
    }
}
