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
    //An action that runs many actions at one time
    public class EActionQuickSequence : EAction
    {
        protected List<EAction> Actions;

        public EActionQuickSequence(params EAction[] eActions) : base()
        {
            RequiresUpdate = false;
            Actions = new List<EAction>(eActions);
        }

        public override void PerformAction(ESprite sprite)
        {
            foreach (EAction action in Actions)
            {
                sprite.runAction(action);
            }
        }
    }

    //An action which runs many actions, one after the other
    public class EActionSequence : EAction
    {
        protected List<EAction> Actions;
        protected Dictionary<ESprite, int> SpriteCurrentNumbers;

        public EActionSequence(params EAction[] eActions) : base()
        {
            SpriteCurrentNumbers = new Dictionary<ESprite, int>();
            RequiresUpdate = true;
            Actions = new List<EAction>(eActions);
        }

        public override void PerformAction(ESprite sprite)
        {
            Actions[0].PerformAction(sprite);
            SpriteCurrentNumbers.Add(sprite, 0);
        }

        public override bool Update(ESprite targetSprite)
        {
            if (Actions.Count - 1 < SpriteCurrentNumbers[targetSprite])
            {
                SpriteCurrentNumbers[targetSprite] = 0;
                return true;
            }

            if (Actions[SpriteCurrentNumbers[targetSprite]].Update(targetSprite))
            {
                SpriteCurrentNumbers[targetSprite]++;

                if (Actions.Count - 1 < SpriteCurrentNumbers[targetSprite])
                {
                    SpriteCurrentNumbers[targetSprite] = 0;
                    return true;
                }

                Actions[SpriteCurrentNumbers[targetSprite]].PerformAction(targetSprite);
            }

            return false;
        }
    }
}
