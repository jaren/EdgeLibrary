using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TowerDefenseGame
{
    public class RoundManager
    {
        public List<Round> Rounds;
        public int CurrentIndex;
        public bool RoundRunning;

        public delegate void RoundManagerEnemyEvent(Round round, EnemyData enemy);
        public event RoundManagerEnemyEvent OnEmitEnemy;
        public delegate void RoundManagerEvent(Round round);
        public event RoundManagerEvent OnFinish;
        public delegate void RoundManagerNumberEvent(Round round, int number);
        public event RoundManagerNumberEvent OnFinishRound;

        public RoundManager(List<Round> rounds)
        {
            Rounds = rounds;
            CurrentIndex = 0;

            foreach (Round round in Rounds)
            {
                round.OnEmitEnemy += round_OnEmitEnemy;
                round.OnFinish += round_OnFinish;
                round.Started = false;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (CurrentIndex < Rounds.Count)
            {
                Rounds[CurrentIndex].Update(gameTime);
            }
        }

        public virtual void Restart()
        {
            CurrentIndex = 0;
            StartRound();
        }

        public virtual void StartRound()
        {
            Rounds[CurrentIndex].Started = true;
            RoundRunning = true;
        }

        public virtual void round_OnEmitEnemy(Round round, EnemyData enemy)
        {
            if (OnEmitEnemy != null)
            {
                OnEmitEnemy(round, enemy);
            }
        }

        public virtual void round_OnFinish(Round round)
        {
            RoundRunning = false;
            CurrentIndex++;

            if (CurrentIndex >= Rounds.Count)
            {
                if (OnFinish != null)
                {
                    OnFinish(round);
                }
            }
            else
            {
                Rounds[CurrentIndex].Started = false;
            }

            if (OnFinishRound != null)
            {
                OnFinishRound(round, CurrentIndex - 1);
            }
        }
    }
}
