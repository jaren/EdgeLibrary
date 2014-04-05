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
    public class AQuickSequence : AAction
    {
        public List<AAction> Actions;

        public AQuickSequence()
        {
            Actions = new List<AAction>();
        }

        public AQuickSequence(List<AAction> actions)
        {
            Actions = new List<AAction>(actions);
        }

        public AQuickSequence(params AAction[] actions)
        {
            Actions = new List<AAction>(actions);
        }

        //Adds an action to the list
        public void AddAction(AAction action)
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
            foreach (AAction action in Actions)
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
                        toRemove = true;
                    }
                }
            }
        }

        //Returns a new AAction
        public override AAction Copy()
        {
            return new AQuickSequence(Actions);
        }
    }
}
