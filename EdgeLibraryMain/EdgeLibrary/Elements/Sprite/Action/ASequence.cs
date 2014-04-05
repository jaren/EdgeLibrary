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
    //Holds a list of actions and runs them one at a time - the next one doesn't start until the previous one has finished
    public class ASequence : AAction
    {
        //Called when the next action is looped through
        public delegate void ASequenceEvent(ASequence sequence, AAction currentAction, Sprite sprite, GameTime gameTime);
        public event ASequenceEvent OnActionTransition = delegate { };

        public List<AAction> Actions;
        public int CurrentIndex;

        public ASequence()
        {
            Actions = new List<AAction>();
        }

        public ASequence(List<AAction> actions)
        {
            Actions = new List<AAction>(actions);
        }

        public ASequence(params AAction[] actions)
        {
            Actions = new List<AAction>(actions);
        }

        //Adds an action to the end of the list
        public void AddActionLast(AAction action)
        {
            Actions.Add(action);
        }

        //Adds an action to the beginning of list
        public void AddActionFirst(AAction action)
        {
            Actions.Insert(0, action);
        }

        //Adds an action to the list at an index
        public void AddAction(int index, AAction action)
        {
            Actions.Insert(index, action);
        }

        //Removes an action from the list
        public void RemoveAction(int index)
        {
            Actions.RemoveAt(index);
        }

        //Updates all the actions in the list at once and automatically removes them
        protected override void UpdateAction(GameTime gameTime, Sprite sprite)
        {
            Actions[CurrentIndex].Update(gameTime, sprite);

            if (Actions[CurrentIndex].toRemove)
            {
                CurrentIndex++;

                if (CurrentIndex > Actions.Count - 1)
                {
                    CurrentIndex = 0;
                    toRemove = true;
                }

                OnActionTransition(this, Actions[CurrentIndex], sprite, gameTime);
            }
        }

        //Returns a new AAction
        public override AAction Copy()
        {
            return new ASequence(Actions);
        }
    }
}
