using EdgeLibrary;
using Microsoft.Xna.Framework;
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
        public double Duration;
        public double ElapsedTime;
        public bool ShouldRemove;

        public Effect(string name, double duration, float milliseconds)
        {
            Name = name;
            Ticker = new Ticker(milliseconds);
            Duration = duration;
            ElapsedTime = 0;
            ShouldRemove = false;
        }

        public virtual void UpdateEffect(GameTime gameTime, Enemy enemy)
        {
            ElapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (ElapsedTime > Duration)
            {
                ShouldRemove = true;
            }
        }
    }
}
