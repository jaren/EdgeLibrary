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
        public int MinMilliseconds;
        public int MaxMilliseconds;
        public int CurrentMillisecondsWait;
        protected double elapsedMilliseconds;

        public delegate void TickerEventHandler(GameTime gameTime);
        public event TickerEventHandler Tick = delegate { };

        public RandomTicker(int min, int max)
            : base(EdgeGame.Game)
        {
            MinMilliseconds = min;
            MaxMilliseconds = max;
            CurrentMillisecondsWait = RandomTools.RandomInt(MinMilliseconds, MaxMilliseconds);
            elapsedMilliseconds = 0;
        }

        protected override void UpdateObject(GameTime gameTime)
        {
            elapsedMilliseconds += gameTime.ElapsedGameTime.TotalMilliseconds * EdgeGame.GameSpeed;

            if (elapsedMilliseconds >= CurrentMillisecondsWait)
            {
                elapsedMilliseconds = 0;
                Tick(gameTime);

                CurrentMillisecondsWait = RandomTools.RandomInt(MinMilliseconds, MaxMilliseconds);
            }
        }

        public override object Clone()
        {
            RandomTicker clone = (RandomTicker)MemberwiseClone();
            return clone;
        }
    }
}
