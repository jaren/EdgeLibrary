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
        public List<RoundEnemyList> EnemiesList;
        public int CurrentIndex = 0;

        public delegate void RoundEvent(Round round);
        public delegate void RoundEventEnemy(Round round, EnemyData enemy);
        public event RoundEvent OnFinish;
        public event RoundEventEnemy OnEmitEnemy;

        public Round(List<RoundEnemyList> enemies) : base(0)
        {
            EnemiesList = enemies;
            Enemies = new List<KeyValuePair<EnemyData, float>>();
            foreach (RoundEnemyList enemy in enemies)
            {
                for (int i = 0; i < enemy.Count; i++)
                {
                    Enemies.Add(new KeyValuePair<EnemyData,float>(enemy.EnemyData, enemy.TimeBetween));
                }
            }

            base.OnTick += Round_OnTick;
        }

        void Round_OnTick(GameTime gameTime)
        {
            OnEmitEnemy(this, Enemies[CurrentIndex].Key);
            CurrentIndex++;
            if (CurrentIndex >= Enemies.Count)
            {
                if (OnFinish != null)
                {
                    OnFinish(this);
                }
                return;
            }

            MillisecondsWait = Enemies[CurrentIndex].Value;
        }

        public new Round Clone()
        {
            return new Round(EnemiesList);
        }
    }

    public struct RoundEnemyList
    {
        public EnemyData EnemyData;
        public float TimeBetween;
        public int Count;

        public RoundEnemyList(Enemy.Type enemyType, float timeBetween, int count)
        {
            EnemyData = Config.Enemies[(int)enemyType];
            TimeBetween = timeBetween;
            Count = count;
        }
    }
}
