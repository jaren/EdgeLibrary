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
    public class ASequence : Action
    {
        //Called when the next action is looped through
        public delegate void ASequenceEvent(ASequence sequence, Action currentAction, Sprite sprite, GameTime gameTime);
        public event ASequenceEvent OnActionTransition = delegate { };

        public List<Action> Actions;
        public int CurrentIndex;

        public ASequence() : base()
        {
            Actions = new List<Action>();
        }

        public ASequence(List<Action> actions) : base()
        {
            Actions = new List<Action>(actions);
        }

        public ASequence(params Action[] actions) : base()
        {
            Actions = new List<Action>(actions);
        }
        
        public ASequence(string ID) : base(ID)
        {
            Actions = new List<Action>();
        }

        public ASequence(string ID, List<Action> actions) : base(ID)
        {
            Actions = new List<Action>(actions);
        }

        public ASequence(string ID, params Action[] actions) : base(ID)
        {
            Actions = new List<Action>(actions);
        }

        //Adds an action to the end of the list
        public void AddActionLast(Action action)
        {
            Actions.Add(action);
        }

        //Adds an action to the beginning of list
        public void AddActionFirst(Action action)
        {
            Actions.Insert(0, action);
        }

        //Adds an action to the list at an index
        public void AddAction(int index, Action action)
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
                    Stop(gameTime, sprite);
                }

                OnActionTransition(this, Actions[CurrentIndex], sprite, gameTime);
            }
        }

        //Returns a new Action
        public override Action Clone()
        {
            return new ASequence(ID, Actions);
        }
    }
}
