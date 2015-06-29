using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class RoundManager
    {
        public List<Round> Rounds;
        public int CurrentIndex;
        public bool RoundRunning;

        public RoundManager(List<Round> rounds)
        {
            Rounds = rounds;
            CurrentIndex = 0;

            foreach(Round round in Rounds)
            {
                round.OnEmitEnemy += round_OnEmitEnemy;
                round.OnFinish += round_OnFinish;
            }
        }

        public void Restart()
        {
            CurrentIndex = 0;
            StartRound();
        }

        public void StartRound()
        {
            Rounds[CurrentIndex].Started = true;
            RoundRunning = true;
        }

        public void round_OnEmitEnemy(Round round, EnemyData enemy)
        {
        }

        public void round_OnFinish(Round round)
        {
            RoundRunning = false;
            CurrentIndex++;
            Rounds[CurrentIndex].Started = false;
        }
    }
}
