using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;


namespace EdgeDemo.CheckersGame
{
    public class MenuBase
    {
        public string Name;
        protected List<GameComponent> Components;

        public MenuBase(string name)
        {
            Name = name;
            Components = new List<GameComponent>();
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach(GameComponent component in Components)
            {
                component.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            foreach(GameComponent component in Components)
            {
                if (component is DrawableGameComponent)
                {
                    ((DrawableGameComponent)component).Draw(gameTime);
                }
            }
        }

        public virtual void SwitchTo()
        {

        }
    }
}
