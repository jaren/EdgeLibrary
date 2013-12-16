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
    public class EActionRunEventArgs : EventArgs
    {
        public EAction action;
        public ESprite sender;

        public EActionRunEventArgs(EAction eAction, ESprite eSender)
        {
            action = eAction;
            sender = eSender;
        }
    }

    //Runs a function
    public class EActionRun : EAction
    {
        public delegate void EActivate(EActionRunEventArgs e);
        public event EActivate OnActivate;

        public EActionRun(EActivate activate)
        {
            OnActivate += activate;
            requiresUpdate = false;
        }

        public EActionRun(EActionRun action) : this(action.OnActivate)
        {
        }

        public override void initWithSprite(ESprite sprite)
        {
            OnActivate(new EActionRunEventArgs(this, sprite));
        }
    }
}
