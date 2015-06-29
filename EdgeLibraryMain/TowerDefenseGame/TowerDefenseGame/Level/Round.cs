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
        public Dictionary<EnemyData, float> Enemies;
        public int CurrentIndex = 0;

        public delegate void RoundEvent(Round round);
        public delegate void RoundEventEnemy(Round round, EnemyData enemy);
        public event RoundEvent OnFinish;
        public event RoundEventEnemy OnEmitEnemy;

        public Round(Dictionary<EnemyData, float> enemies) : base(0)
        {
            Enemies = enemies;
            base.OnTick += Round_OnTick;
        }

        void Round_OnTick(GameTime gameTime)
        {
            CurrentIndex++;
            MillisecondsWait = Enemies.Values.ToList()[CurrentIndex];

            OnEmitEnemy(this, Enemies.Keys.ToList()[CurrentIndex]);

            if (CurrentIndex >= Enemies.Keys.Count && OnFinish != null)
            {
                OnFinish(this);
            }
        }
    }
}
