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

        public EActionRemove(EdgeGame mainGame, EElement elementToRemove) : base()
        {
            RequiresUpdate = false;
            game = mainGame;
            element = elementToRemove;
        }

        public override void PerformAction(ESprite targetSprite)
        {
            game.RemoveElement(element);
        }
    }

    public class EActionRemoveSelf : EAction
    {
        EdgeGame game;

        public EActionRemoveSelf(EdgeGame mainGame) : base()
        {
            game = mainGame;
            RequiresUpdate = false;
        }

        public override void PerformAction(ESprite targetSprite)
        {
            game.RemoveElement(targetSprite);
        }
    }
}
