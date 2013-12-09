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
    public class EAction : EObject
    {
        public bool requiresUpdate { get; protected set; }
        protected bool done = false;

        public EAction()
        {
            requiresUpdate = false;
        }

        public void cancel() { done = true; }
        public virtual void initWithSprite(ESprite sprite) { }
        //Returns true if the action has finished
        public bool Update(ESprite targetSprite) { if (done) { return true; } return updateAction(targetSprite); }
        public virtual bool updateAction(ESprite targetSprite) { return true; }
    }

    /*
     * public class EActionBase
     * {
     * 
     * public EActionBase()
     * {
     * requiresUpdate = false;
     * }
     * 
     * public override void initWithSprite(ESprite sprite) {}
     * public override void updateElement() {return done;}
     * }
     */
}
