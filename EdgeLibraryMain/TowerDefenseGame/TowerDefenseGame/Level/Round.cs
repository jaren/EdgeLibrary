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
        public List<KeyValuePair<EnemyData,float>> Enemies;
        public int CurrentIndex = 0;

        public delegate void RoundEvent(Round round);
        public delegate void RoundEventEnemy(Round round, EnemyData enemy);
        public event RoundEvent OnFinish;
        public event RoundEventEnemy OnEmitEnemy;

        public Round(List<RoundEnemyList> enemies) : base(0)
        {
            Enemies = new List<KeyValuePair<EnemyData, float>>();
            foreach (RoundEnemyList enemy in enemies)
            {
                for (int i = 0; i < enemy.Count; i++)
                {
                    Enemies.Add(new KeyValuePair<EnemyData,float>(enemy.EnemyData, enemy.TimeBetween));
                }
            }
            MillisecondsWait = enemies[0].TimeBetween;
            base.OnTick += Round_OnTick;
        }

        int hitCount = 0;
        void Round_OnTick(GameTime gameTime)
        {
            hitCount++;
            if (CurrentIndex >= Enemies.Count())
            {

            }
            OnEmitEnemy(this, Enemies[CurrentIndex].Key);
            CurrentIndex++;

            if (CurrentIndex >= Enemies.Count && OnFinish != null)
            {
                OnFinish(this);
                return;
            }

            MillisecondsWait = Enemies[CurrentIndex].Value;
        }
    }

    public struct RoundEnemyList
    {
        public EnemyData EnemyData;
        public float TimeBetween;
        public int Count;

        public RoundEnemyList(EnemyData enemyData, float timeBetween, int count)
        {
            EnemyData = enemyData;
            TimeBetween = timeBetween;
            Count = count;
        }
    }
}
