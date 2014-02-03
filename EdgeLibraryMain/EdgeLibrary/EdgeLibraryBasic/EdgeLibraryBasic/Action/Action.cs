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
    public class EAction : Object
    {
        /// <summary>
        /// Gets a boolean indicating whether this action needs to be continuously updated from this point onwards.
        /// </summary>
        public bool RequiresUpdate { get; protected set; }

        public EAction()
        {
            RequiresUpdate = false;
        }

        /// <summary>
        /// Immediately completes the action so that it is not further updated.
        /// </summary>
        public void cancel() { RequiresUpdate = false; }

        /// <summary>
        /// Performs this action on a sprite, setting <see cref="RequiresUpdate"/> if this action needs further updating.
        /// </summary>
        /// <param name="targetSprite">The sprite to perform the action on.</param>
        public virtual void PerformAction(Sprite targetSprite) { }
        /// <summary>
        /// Continues to perform the action on the sprite.
        /// </summary>
        /// <param name="targetSprite">The sprite to continue to perform this action on.</param>
        /// <returns>True if the action is completed; false otherwise.</returns>
        public virtual bool Update(Sprite targetSprite) { return true; }
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
     * public override void initWithSprite(Sprite sprite) {}
     * public override void updatElement() {return done;}
     * }
     */
}
