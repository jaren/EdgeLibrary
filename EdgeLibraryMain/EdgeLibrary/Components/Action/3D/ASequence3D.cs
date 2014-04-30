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
    public class ASequence3D : Action3D
    {
        //Called when the next action is looped through
        public delegate void ASequenceEvent(ASequence3D sequence, Action3D currentAction, Sprite3D sprite, GameTime gameTime);
        public event ASequenceEvent OnActionTransition = delegate { };

        public List<Action3D> Actions;
        public int CurrentIndex;

        public ASequence3D()
        {
            Actions = new List<Action3D>();
        }

        public ASequence3D(List<Action3D> actions)
        {
            Actions = new List<Action3D>(actions);
        }

        public ASequence3D(params Action3D[] actions)
        {
            Actions = new List<Action3D>(actions);
        }

        //Adds an action to the end of the list
        public void AddActionLast(Action3D action)
        {
            Actions.Add(action);
        }

        //Adds an action to the beginning of list
        public void AddActionFirst(Action3D action)
        {
            Actions.Insert(0, action);
        }

        //Adds an action to the list at an index
        public void AddAction(int index, Action3D action)
        {
            Actions.Insert(index, action);
        }

        //Removes an action from the list
        public void RemoveAction(int index)
        {
            Actions.RemoveAt(index);
        }

        //Updates all the actions in the list at once and automatically removes them
        protected override void UpdateAction(GameTime gameTime, Sprite3D sprite)
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
        public override Action3D Clone()
        {
            return new ASequence3D(Actions);
        }
    }
}
