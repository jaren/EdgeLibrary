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
    //Holds a list of actions and runs them all
    public class AQuickSequence3D : Action3D
    {
        public List<Action3D> Actions;

        public AQuickSequence3D()
        {
            Actions = new List<Action3D>();
        }

        public AQuickSequence3D(List<Action3D> actions)
        {
            Actions = new List<Action3D>(actions);
        }

        public AQuickSequence3D(params Action3D[] actions)
        {
            Actions = new List<Action3D>(actions);
        }

        //Adds an action to the list
        public void AddAction(Action3D action)
        {
            Actions.Add(action);
        }

        //Removes an action from the list
        public void RemoveAction(int index)
        {
            Actions.RemoveAt(index);
        }

        //Updates all the actions in the list at once and automatically removes them
        protected override void UpdateAction(GameTime gameTime, Sprite3D sprite)
        {
            //Updates the actions even if they should be removed - used for repetition
            foreach (Action3D action in Actions)
            {
                action.Update(gameTime, sprite);
            }
        }

        //Returns a new Action
        public override Action3D Clone()
        {
            return new AQuickSequence3D(Actions);
        }
    }
}
