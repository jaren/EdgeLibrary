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
    public class ARepeat3D : Action3D
    {
        public Action3D Action;
        public int LoopTimes;
        public bool RepeatForever;
        private int loopedTimes;

        public ARepeat3D(Action3D action)
        {
            Action = action;
            LoopTimes = 0;
            RepeatForever = true;
            loopedTimes = 0;
        }

        public ARepeat3D(int loopTimes, Action3D action)
        {
            Action = action;
            LoopTimes = loopTimes;
            RepeatForever = false;
            loopedTimes = 0;
        }

        //Updates the specific action
        protected override void UpdateAction(GameTime gameTime, Sprite3D sprite)
        {
            loopedTimes++;

            Action.Update(gameTime, sprite);

            if (loopedTimes >= LoopTimes && !RepeatForever)
            {
                Stop(gameTime, sprite);
            }
        }

        public override Action3D Clone()
        {
            return RepeatForever ? new ARepeat3D(Action) : new ARepeat3D(LoopTimes, Action);
        }
    }
}
