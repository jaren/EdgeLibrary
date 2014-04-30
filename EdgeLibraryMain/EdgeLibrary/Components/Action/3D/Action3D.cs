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
    //Provides the base for all Actions - sprite changers
    public abstract class Action3D
    {
        //If set to true, the class currently managing this action should remove it from its index
        public bool toRemove { get; private set; }
        //If set to true, the action will not update
        public bool Paused;

        public delegate void Action3DEvent(Action3D action, GameTime gameTime, Sprite3D sprite);
        public event Action3DEvent OnFinish = delegate { };

        public void Update(GameTime gameTime, Sprite3D sprite)
        {
            if (!Paused)
            {
                //In case this action will be repeated - if it wasn't removed after updating, then it should be removed
                toRemove = false;

                UpdateAction(gameTime, sprite);
            }
        }

        //Used to update the action
        protected abstract void UpdateAction(GameTime gameTime, Sprite3D sprite);

        //Returns a Clone of the action so that multiple sprites don't share the same action
        public abstract Action3D Clone();
        
        //Marks the action for removal from the sprite's action list
        //OnFinish is NOT called if Stop is not passed in GameTime and Sprite
        public void Stop() { toRemove = true; }
        protected void Stop(GameTime gameTime, Sprite3D sprite) { toRemove = true; OnFinish(this, gameTime, sprite); }
    }
}
