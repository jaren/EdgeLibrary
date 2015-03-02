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
    public class AQuickSequence : Action
    {
        public List<Action> Actions;

        public AQuickSequence() : base()
        {
            Actions = new List<Action>();
        }

        public AQuickSequence(List<Action> actions) : base()
        {
            Actions = new List<Action>(actions);
        }

        public AQuickSequence(params Action[] actions)
            : base()
        {
            Actions = new List<Action>(actions);
        }

        //Adds an action to the list
        public void AddAction(Action action)
        {
            Actions.Add(action);
        }

        //Removes an action from the list
        public void RemoveAction(int index)
        {
            Actions.RemoveAt(index);
        }

        //Updates all the actions in the list at once and automatically removes them
        protected override void UpdateAction(GameTime gameTime, Sprite sprite)
        {
            //Updates the actions even if they should be removed - used for repetition
            foreach (Action action in Actions)
            {
                action.Update(gameTime, sprite);
            }
        }

        //Resets all the actions in the sequence
        public override void Reset()
        {
            foreach(Action action in Actions)
            {
                action.Reset();
            }
            base.Reset();
        }

        //Returns a new Action
        public override Action Clone()
        {
            return new AQuickSequence(Actions);
        }
    }
}
