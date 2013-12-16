using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeLibrary.Basic
{
    public class EActionRemove : EAction
    {
        public EdgeGame game;
        public EElement element;

        public EActionRemove(EdgeGame mainGame, EElement elementToRemove)
        {
            requiresUpdate = false;
            game = mainGame;
            element = elementToRemove;
        }

        public override void initWithSprite(ESprite targetSprite)
        {
            game.RemoveElement(element);
        }
    }

    public class EActionRemoveSelf : EAction
    {
        EdgeGame game;

        public EActionRemoveSelf(EdgeGame mainGame)
        {
            game = mainGame;
            requiresUpdate = false;
        }

        public override void initWithSprite(ESprite targetSprite)
        {
            game.RemoveElement(targetSprite);
        }
    }
}
