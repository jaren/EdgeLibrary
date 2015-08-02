﻿using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class Round : Ticker
    {
        public List<RoundEnemyList> Enemies;
        public int CurrentIndex = 0;

        public delegate void RoundEvent(Round round);
        public delegate void RoundEventEnemy(Round round, EnemyData enemy);
        public event RoundEvent OnFinish;
        public event RoundEventEnemy OnEmitEnemy;

        public Round(List<RoundEnemyList> enemies) : base(0)
        {
            Enemies = enemies;
            MillisecondsWait = Enemies[0].TimeBetween;
            base.OnTick += Round_OnTick;
        }

        void Round_OnTick(GameTime gameTime)
        {

            OnEmitEnemy(this, Enemies[CurrentIndex].EnemyData);
            CurrentIndex++;

            if (CurrentIndex >= Enemies.Count && OnFinish != null)
            {
                OnFinish(this);
                return;
            }

            MillisecondsWait = Enemies[CurrentIndex].TimeBetween;
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
