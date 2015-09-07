using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class SlowEffect : Effect
    {
        public float OriginalSpeed = 0;
        public float Slow;

        public SlowEffect(float slow, float duration) : base("Slow", duration, 100) { Slow = slow; }

        public override void UpdateEffect(GameTime gameTime, Enemy enemy)
        {
            if (OriginalSpeed == 0)
            {
                OriginalSpeed = enemy.EnemyData.Speed;
            }

            base.UpdateEffect(gameTime, enemy);
            enemy.Color = RandomTools.RandomColor(Color.LightBlue, Color.Aquamarine);
            enemy.ChangeSpeed(OriginalSpeed * Slow);

            if (ShouldRemove)
            {
                enemy.ChangeSpeed(OriginalSpeed);
                enemy.Color = Color.White;
            }
        }
    }
}
