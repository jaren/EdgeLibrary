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
        public List<KeyValuePair<EnemyData, float>> Enemies;
        public int CurrentIndex = 0;

        public delegate void RoundEvent(Round round);
        public delegate void RoundEventEnemy(Round round, EnemyData enemy);
        public event RoundEvent OnFinish;
        public event RoundEventEnemy OnEmitEnemy;

        public Round(List<KeyValuePair<EnemyData, float>> enemies) : base(0)
        {
            Enemies = enemies;
            MillisecondsWait = Enemies[0].Value;
            base.OnTick += Round_OnTick;
        }

        void Round_OnTick(GameTime gameTime)
        {
            CurrentIndex++;

            if (CurrentIndex >= Enemies.Count && OnFinish != null)
            {
                OnFinish(this);
                return;
            }

            MillisecondsWait = Enemies[CurrentIndex].Value;
            OnEmitEnemy(this, Enemies[CurrentIndex].Key);
        }
    }
}
