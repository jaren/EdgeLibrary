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

        public AQuickSequence()
        {
            Actions = new List<Action>();
        }

        public AQuickSequence(List<Action> actions)
        {
            Actions = new List<Action>(actions);
        }

        public AQuickSequence(params Action[] actions)
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
            foreach (Action action in Actions)
            {
                action.Update(gameTime, sprite);
            }

            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].toRemove)
                {
                    Actions.RemoveAt(i);
                    i--;

                    if (Actions.Count == 0)
                    {
                        Stop(gameTime, sprite);
                    }
                }
            }
        }

        //Returns a new Action
        public override Action Clone()
        {
            return new AQuickSequence(Actions);
        }
    }
}
