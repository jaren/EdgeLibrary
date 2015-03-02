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
    //Repeats an action for a specific number of times - or forever
    public class ARepeat : Action
    {
        public Action Action;
        public int LoopTimes;
        public bool RepeatForever;
        private int loopedTimes;

        public ARepeat(Action action) : base()
        {
            Action = action;
            LoopTimes = 0;
            RepeatForever = true;
            loopedTimes = 0;
        }

        public ARepeat(int loopTimes, Action action) : base()
        {
            Action = action;
            LoopTimes = loopTimes;
            RepeatForever = false;
            loopedTimes = 0;
        }

        //Updates the specific action
        protected override void UpdateAction(GameTime gameTime, Sprite sprite)
        {
            loopedTimes++;

            Action.Update(gameTime, sprite);
            if (Action.toRemove)
            {
                Action.Reset();
            }

            if (loopedTimes >= LoopTimes && !RepeatForever)
            {
                Stop(gameTime, sprite);
            }
        }

        public override void Reset()
        {
            loopedTimes = 0;
            base.Reset();
        }

        public override Action Clone()
        {
            return RepeatForever ? new ARepeat(Action) : new ARepeat(LoopTimes, Action);
        }
    }
}
