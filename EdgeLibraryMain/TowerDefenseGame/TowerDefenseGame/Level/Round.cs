using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class Round : Ticker
    {
        public Dictionary<EnemyType, float> Enemies;
        public int CurrentIndex = 0;

        public Round(Dictionary<EnemyType, float> enemies) : base(0)
        {
            Enemies = enemies;
            base.OnTick += Round_OnTick;
        }

        void Round_OnTick(GameTime gameTime)
        {
            CurrentIndex++;
            MillisecondsWait = Enemies.Values.ToList()[CurrentIndex];
        }
    }
}
