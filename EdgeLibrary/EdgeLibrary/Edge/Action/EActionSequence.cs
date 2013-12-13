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
    public class EActionQuickSequence : EAction
    {
        protected List<EAction> Actions;

        public EActionQuickSequence(params EAction[] eActions)
        {
            requiresUpdate = false;
            Actions = new List<EAction>(eActions);
        }

        public override void initWithSprite(ESprite sprite)
        {
            foreach (EAction action in Actions)
            {
                sprite.runAction(action);
            }
        }
    }

    public class EActionSequence : EAction
    {
        protected List<EAction> Actions;
        public int currentNumber { get; set; }

        public EActionSequence(params EAction[] eActions)
        {
            currentNumber = 0;
            requiresUpdate = true;
            Actions = new List<EAction>(eActions);
        }

        public override void initWithSprite(ESprite sprite)
        {
            Actions[currentNumber].initWithSprite(sprite);
        }

        public override bool UpdateAction(ESprite targetSprite)
        {
            if (Actions.Count - 1 < currentNumber)
            {
                currentNumber = 0;
                return true;
            }

            if (Actions[currentNumber].Update(targetSprite))
            {
                currentNumber++;

                if (Actions.Count - 1 < currentNumber)
                {
                    currentNumber = 0;
                    return true;
                }

                Actions[currentNumber].initWithSprite(targetSprite);
            }

            return false;
        }
    }
}
