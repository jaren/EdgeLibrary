using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeLibrary.Edge
{
    public class ETickerEventArgs : EventArgs
    {
        public EUpdateArgs updateArgs;
        public int value;

        public ETickerEventArgs(EUpdateArgs eUpdateArgs, int eValue)
        {
            updateArgs = eUpdateArgs;
            value = eValue;
        }
    }

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
            Tick += new ETickerEventHandler(nullHandler);
        }

        public ETicker(double eMilliseconds, ERange eValueRange) : this(eMilliseconds)
        {
            ValueRange = eValueRange;
            currentValue = (int)ValueRange.Min;
        }

        protected void nullHandler(ETickerEventArgs e) { }

        public override void updateElement(EUpdateArgs updateArgs)
        {
            elapsedMilliseconds += updateArgs.gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedMilliseconds > MillisecondsWait)
            {
                elapsedMilliseconds = 0;
                currentValue++;
                if (currentValue > ValueRange.Max)
                {
                    currentValue = (int)ValueRange.Min;
                }
                Tick(new ETickerEventArgs(updateArgs, currentValue));
            }
        }
    }
}
