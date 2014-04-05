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
    public class ARepeat : AAction
    {
        public AAction Action;
        public int LoopTimes;
        public bool RepeatForever;
        private int loopedTimes;

        public ARepeat(AAction action)
        {
            Action = action;
            LoopTimes = 0;
            RepeatForever = true;
            loopedTimes = 0;
        }

        public ARepeat(int loopTimes, AAction action)
        {
            Action = action;
            LoopTimes = loopTimes;
            RepeatForever = false;
            loopedTimes = 0;
        }

        //Updates the specific action
        public override void UpdateAction(GameTime gameTime, Sprite sprite)
        {
            //Sets toRemove to be false in case it's going to be repeated
            toRemove = false;

            loopedTimes++;

            Action.UpdateAction(gameTime, sprite);

            if (loopedTimes >= LoopTimes && !RepeatForever)
            {
                toRemove = true;
            }
        }

        public override AAction Copy()
        {
            return RepeatForever ? new ARepeat(Action) : new ARepeat(LoopTimes, Action);
        }
    }
}
