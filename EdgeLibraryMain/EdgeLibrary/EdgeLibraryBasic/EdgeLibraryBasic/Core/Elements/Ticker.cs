using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeLibrary
{
    public class TickerEventArgs : EventArgs
    {
        /// <summary>
        /// The update arguments for the update which caused the event.
        /// </summary>
        public UpdateArgs UpdateArgs;
        public int value;

        public TickerEventArgs(UpdateArgs UpdateArgs, int eValue)
        {
            UpdateArgs = UpdateArgs;
            value = eValue;
        }
    }

    //Basically a timer - sends off an event whenever a certain time has passed
    public class Ticker : Element
    {
        public double MillisecondsWait { get; set; }
        public Range ValuRange { get; set; }
        protected int currentValue;
        protected double elapsedMilliseconds;

        public delegate void TickerEventHandler(TickerEventArgs e);
        public event TickerEventHandler Tick;

        public Ticker(double eMilliseconds) 
        {
            MillisecondsWait = eMilliseconds; 
            elapsedMilliseconds = 0;
            ValuRange = new Range(0);
            currentValue = 0;
        }

        public Ticker(double eMilliseconds, Range eValuRange) : this(eMilliseconds)
        {
            ValuRange = eValuRange;
            currentValue = (int)ValuRange.Min;
        }

        public override void updatElement(UpdateArgs updateArgs)
        {
            elapsedMilliseconds += updateArgs.gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedMilliseconds >= MillisecondsWait)
            {
                elapsedMilliseconds = 0;
                currentValue++;
                if (currentValue > ValuRange.Max)
                {
                    currentValue = (int)ValuRange.Min;
                }
                if (Tick != null)
                {
                    Tick(new TickerEventArgs(updateArgs, currentValue));
                }
            }
        }
    }

    //An Ticker with an Range as the millisecond count
    public class ERandomTicker : Element
    {
        public Range MillisecondsRange { get; set; }
        public int CurrentMillisecondsWait { get; set; }
        public Range ValuRange { get; set; }
        protected int currentValue;
        protected double elapsedMilliseconds;
        protected Random random;

        public delegate void TickerEventHandler(TickerEventArgs e);
        public event TickerEventHandler Tick;

        public ERandomTicker(Range eMilliseconds)
        {
            random = new Random();

            MillisecondsRange = eMilliseconds;
            CurrentMillisecondsWait = (int)MillisecondsRange.GetRandom(random);
            elapsedMilliseconds = 0;
            ValuRange = new Range(0);
            currentValue = 0;
        }

        public ERandomTicker(Range eMilliseconds, Range eValuRange) : this(eMilliseconds)
        {
            ValuRange = eValuRange;
            currentValue = (int)ValuRange.Min;
        }

        public override void updatElement(UpdateArgs updateArgs)
        {
            elapsedMilliseconds += updateArgs.gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedMilliseconds >= CurrentMillisecondsWait)
            {
                elapsedMilliseconds = 0;
                currentValue++;
                if (currentValue > ValuRange.Max)
                {
                    currentValue = (int)ValuRange.Min;
                }
                if (Tick != null)
                {
                    Tick(new TickerEventArgs(updateArgs, currentValue));
                }

                CurrentMillisecondsWait = (int)MillisecondsRange.GetRandom(random);
            }
        }
    }
}
