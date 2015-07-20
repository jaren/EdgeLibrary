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
        public Action<Enemy> EffectOnUpdate;
        public Ticker Ticker;

        public Effect(string name, float milliseconds, Action<Enemy> effectOnUpdate, Action<Enemy> effectOnTick)
        {
            Name = name;
            EffectOnUpdate = effectOnUpdate;
            Ticker = new Ticker(milliseconds);
            Ticker.Started = true;
        }
    }
}
