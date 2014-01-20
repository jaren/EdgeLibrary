using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeLibrary.Basic
{
    //An action to remove a sprite from the game
    public class EActionRemove : EAction
    {
        public EElement element;

        public EActionRemove(EElement elementToRemove) : base()
        {
            RequiresUpdate = false;
            element = elementToRemove;
        }

        public override void PerformAction(ESprite targetSprite)
        {
            EdgeGame.RemoveElement(element);
        }
    }

    public class EActionRemoveSelf : EAction
    {
        public EActionRemoveSelf() : base()
        {
            RequiresUpdate = false;
        }

        public override void PerformAction(ESprite targetSprite)
        {
            EdgeGame.RemoveElement(targetSprite);
        }
    }
}
