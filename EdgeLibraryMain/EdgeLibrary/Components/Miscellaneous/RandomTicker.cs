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
    //An Ticker which randomly ticks between two values
    public class RandomTicker : GameComponent, ICloneable
    {
        public double MinMilliseconds;
        public double MaxMilliseconds;
        public double CurrentMillisecondsWait;
        protected double elapsedMilliseconds;

        public delegate void TickerEventHandler(GameTime gameTime);
        public event TickerEventHandler OnTick;

        public RandomTicker(double milliseconds) : this(milliseconds, milliseconds) { }
        public RandomTicker(double min, double max)
            : base(EdgeGame.Game)
        {
            MinMilliseconds = min;
            MaxMilliseconds = max;
            CurrentMillisecondsWait = RandomTools.RandomDouble(MinMilliseconds, MaxMilliseconds);
            elapsedMilliseconds = 0;
        }

        public override void Update(GameTime gameTime)
        {
            elapsedMilliseconds += gameTime.ElapsedGameTime.TotalMilliseconds * EdgeGame.GameSpeed;

            if (elapsedMilliseconds >= CurrentMillisecondsWait)
            {
                elapsedMilliseconds = 0;
                if (OnTick != null)
                {
                    OnTick(gameTime);
                }

                CurrentMillisecondsWait = RandomTools.RandomDouble(MinMilliseconds, MaxMilliseconds);
            }
        }

        public object Clone()
        {
            RandomTicker clone = (RandomTicker)MemberwiseClone();
            if (OnTick != null)
            {
                clone.OnTick = (TickerEventHandler)OnTick.Clone();
            }
            return clone;
        }
    }
}
