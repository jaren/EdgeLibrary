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
    //An action to run a function
    [Obsolete("Subclass EAction to implement an action instead.")]
    public class EActionRunEventArgs : EventArgs
    {
        public EAction action;

        /// <summary>
        /// The sprite the action is performed on.
        /// </summary>
        public Sprite Sprite;

        public EActionRunEventArgs(EAction eAction, Sprite eSender)
        {
            action = eAction;
            Sprite = eSender;
        }
    }

    [Obsolete("Subclass EAction to implement an action instead.")]
    public class EActionRun : EAction
    {
        public delegate void EActivate(EActionRunEventArgs e);
        public event EActivate OnActivate;

        public EActionRun(EActivate activate)  : base()
        {
            OnActivate += activate;
            RequiresUpdate = false;
        }

        public EActionRun(EActionRun action)
            : this(action.OnActivate)
        {
        }

        public override void PerformAction(Sprite sprite)
        {
            if (OnActivate != null)
            {
                OnActivate(new EActionRunEventArgs(this, sprite));
            }
        }
    }
}
