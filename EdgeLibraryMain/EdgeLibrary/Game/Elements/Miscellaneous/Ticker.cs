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
    //Basically a timer - sends off an event whenever a certain time has passed
    public class Ticker : Element
    {
        public double MillisecondsWait { get; set; }
        protected int currentValue;
        protected double elapsedMilliseconds;

        public delegate void TickerEventHandler(GameTime gameTime);
        public event TickerEventHandler Tick = delegate{};

        public Ticker(double eMilliseconds)
        {
            MillisecondsWait = eMilliseconds;
            elapsedMilliseconds = 0;
            currentValue = 0;
        }

        protected override void UpdateObject(GameTime gameTime)
        {
            elapsedMilliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedMilliseconds >= MillisecondsWait)
            {
                elapsedMilliseconds = 0;
                currentValue++;
                Tick(gameTime);
            }
        }
    }

    //An Ticker with an Range as the millisecond count
    public class RandomTicker : Element
    {
        public int MinMilliseconds;
        public int MaxMilliseconds;
        public int CurrentMillisecondsWait;
        protected int currentValue;
        protected double elapsedMilliseconds;

        public delegate void TickerEventHandler(GameTime gameTime);
        public event TickerEventHandler Tick = delegate {};

        public RandomTicker(int min, int max)
        {
            MinMilliseconds = min;
            MaxMilliseconds = max;
            CurrentMillisecondsWait = RandomTools.RandomInt(MinMilliseconds, MaxMilliseconds);
            elapsedMilliseconds = 0;
            currentValue = 0;
        }

        protected override void UpdateObject(GameTime gameTime)
        {
            elapsedMilliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedMilliseconds >= CurrentMillisecondsWait)
            {
                elapsedMilliseconds = 0;
                currentValue++;
                Tick(gameTime);

                CurrentMillisecondsWait = RandomTools.RandomInt(MinMilliseconds, MaxMilliseconds);
            }
        }
    }
}
