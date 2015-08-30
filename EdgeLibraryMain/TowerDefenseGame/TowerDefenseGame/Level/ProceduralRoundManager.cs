using EdgeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class ProceduralRoundManager : RoundManager
    {
        public Round CurrentRound;

        public delegate void ProceduralRoundManagerEnemyEvent(Round round, EnemyData enemy);
        public event ProceduralRoundManagerEnemyEvent OnEmitEnemy;
        public delegate void ProceduralRoundManagerNumberEvent(Round round, int number);
        public event ProceduralRoundManagerNumberEvent OnFinishRound;

        public ProceduralRoundManager() : base(new List<Round>())
        {
            CurrentRound = GenerateRound();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            CurrentRound.Update(gameTime);
        }

        public override void round_OnEmitEnemy(Round round, EnemyData enemy)
        {
            if (OnEmitEnemy != null)
            {
                OnEmitEnemy(round, enemy);
            }
        }

        public override void round_OnFinish(Round round)
        {
            CurrentRound = GenerateRound();
            CurrentIndex++;

            RoundRunning = false;

            if (OnFinishRound != null)
            {
                OnFinishRound(round, CurrentIndex - 1);
            }
        }

        public override void StartRound()
        {
            CurrentRound.Started = true;
            RoundRunning = true;
        }
        
        public Round GenerateRound()
        {
            List<RoundEnemyList> enemiesList = new List<RoundEnemyList>();
            for (int i = 0; i < RandomTools.RandomInt(1, (int)Math.Pow(CurrentIndex, 1.5f) + 2); i++)
            {
                List<EnemyData> enemyData = new List<EnemyData>();
                foreach (EnemyData data in Config.Enemies.Values.ToList())
                {
                    for (int count = 0; count < data.Usualness; count++)
                    {
                        enemyData.Add(data);
                    }
                }
                RoundEnemyList list = new RoundEnemyList("Normal", RandomTools.RandomFloat(10, 3000), RandomTools.RandomInt(5, (CurrentIndex+1)*5));
                list.EnemyData = enemyData[RandomTools.RandomInt(0, enemyData.Count)];
                enemiesList.Add(list);
            }
            Round round = new Round(enemiesList);
            round.OnEmitEnemy += round_OnEmitEnemy;
            round.OnFinish += round_OnFinish;
            return round;
        }
    }
}
