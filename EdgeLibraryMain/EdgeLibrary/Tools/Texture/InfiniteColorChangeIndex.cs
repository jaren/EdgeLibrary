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
    public class InfiniteColorChangeIndex : ColorChangeIndex
    {
        public Color Min;
        public Color Max;
        public double MinTime;
        public double MaxTime;
        protected Color CurrentColor1;
        protected Color CurrentColor2;
        protected double currentTime;

        public InfiniteColorChangeIndex(Color min, Color max, double time) : this(min, max, time, time) { }

        public InfiniteColorChangeIndex(Color min, Color max, double minTime, double maxTime)
        {
            Min = min;
            Max = max;
            MinTime = minTime;
            MaxTime = maxTime;

            CurrentColor1 = RandomTools.RandomColor(min, max);
            CurrentColor2 = RandomTools.RandomColor(min, max);
            currentTime = RandomTools.RandomDouble(minTime, maxTime);
        }

        public override Color Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds * EdgeGame.GameSpeed;

            if (elapsedTime > currentTime)
            {
                CurrentColor1 = CurrentColor2;
                CurrentColor2 = RandomTools.RandomColor(Min, Max);
                currentTime = RandomTools.RandomDouble(MinTime, MaxTime);
                elapsedTime = 0;
            }

            return Color.Lerp(CurrentColor1, CurrentColor2, (float)elapsedTime / (float)currentTime);
        }
    }
}
