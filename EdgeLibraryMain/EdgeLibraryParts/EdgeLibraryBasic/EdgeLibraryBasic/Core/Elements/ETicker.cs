using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeLibrary.Basic
{
    public class ETickerEventArgs : EventArgs
    {
        /// <summary>
        /// The update arguments for the update which caused the event.
        /// </summary>
        public EUpdateArgs UpdateArgs;
        public int value;

        public ETickerEventArgs(EUpdateArgs eUpdateArgs, int eValue)
        {
            UpdateArgs = eUpdateArgs;
            value = eValue;
        }
    }

    //Basically a timer - sends off an event whenever a certain time has passed
    public class ETicker : EElement
    {
        public double MillisecondsWait { get; set; }
        public ERange ValueRange { get; set; }
        protected int currentValue;
        protected double elapsedMilliseconds;

        public delegate void ETickerEventHandler(ETickerEventArgs e);
        public event ETickerEventHandler Tick;

        public ETicker(double eMilliseconds) 
        {
            MillisecondsWait = eMilliseconds; 
            elapsedMilliseconds = 0;
            ValueRange = new ERange(0);
            currentValue = 0;
        }

        public ETicker(double eMilliseconds, ERange eValueRange) : this(eMilliseconds)
        {
            ValueRange = eValueRange;
            currentValue = (int)ValueRange.Min;
        }

        public override void updateElement(EUpdateArgs updateArgs)
        {
            elapsedMilliseconds += updateArgs.gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedMilliseconds >= MillisecondsWait)
            {
                elapsedMilliseconds = 0;
                currentValue++;
                if (currentValue > ValueRange.Max)
                {
                    currentValue = (int)ValueRange.Min;
                }
                if (Tick != null)
                {
                    Tick(new ETickerEventArgs(updateArgs, currentValue));
                }
            }
        }
    }

    //An ETicker with an ERange as the millisecond count
    public class ERandomTicker : EElement
    {
        public ERange MillisecondsRange { get; set; }
        public int CurrentMillisecondsWait { get; set; }
        public ERange ValueRange { get; set; }
        protected int currentValue;
        protected double elapsedMilliseconds;
        protected Random random;

        public delegate void ETickerEventHandler(ETickerEventArgs e);
        public event ETickerEventHandler Tick;

        public ERandomTicker(ERange eMilliseconds)
        {
            random = new Random();

            MillisecondsRange = eMilliseconds;
            CurrentMillisecondsWait = (int)MillisecondsRange.GetRandom(random);
            elapsedMilliseconds = 0;
            ValueRange = new ERange(0);
            currentValue = 0;
        }

        public ERandomTicker(ERange eMilliseconds, ERange eValueRange) : this(eMilliseconds)
        {
            ValueRange = eValueRange;
            currentValue = (int)ValueRange.Min;
        }

        public override void updateElement(EUpdateArgs updateArgs)
        {
            elapsedMilliseconds += updateArgs.gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedMilliseconds >= CurrentMillisecondsWait)
            {
                elapsedMilliseconds = 0;
                currentValue++;
                if (currentValue > ValueRange.Max)
                {
                    currentValue = (int)ValueRange.Min;
                }
                if (Tick != null)
                {
                    Tick(new ETickerEventArgs(updateArgs, currentValue));
                }

                CurrentMillisecondsWait = (int)MillisecondsRange.GetRandom(random);
            }
        }
    }
}
