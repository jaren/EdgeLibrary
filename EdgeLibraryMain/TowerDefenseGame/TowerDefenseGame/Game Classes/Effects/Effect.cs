using EdgeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class Effect
    {
        public string Name;
        public Ticker Ticker;

        public Effect(string name, float milliseconds)
        {
            Name = name;
            Ticker = new Ticker(milliseconds);
        }

        public virtual void UpdateEffect(Enemy enemy)
        {

        }
    }
}
