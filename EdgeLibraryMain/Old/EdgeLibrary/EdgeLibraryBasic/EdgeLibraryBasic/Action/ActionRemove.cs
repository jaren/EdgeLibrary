using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeLibrary
{
    //An action to remove a sprite from the game
    public class EActionRemove : EAction
    {
        public Element element;

        public EActionRemove(Element elementToRemove) : base()
        {
            RequiresUpdate = false;
            element = elementToRemove;
        }

        public override void PerformAction(Sprite targetSprite)
        {
            EdgeGame.RemovElement(element);
        }
    }

    public class EActionRemoveSelf : EAction
    {
        public EActionRemoveSelf() : base()
        {
            RequiresUpdate = false;
        }

        public override void PerformAction(Sprite targetSprite)
        {
            EdgeGame.RemovElement(targetSprite);
        }
    }
}
