using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class FireEffect : Effect
    {

        public FireEffect(float duration) : base("Fire", duration, 100) { }

        public override void UpdateEffect(GameTime gameTime, Enemy enemy)
        {
            base.UpdateEffect(gameTime, enemy);
            enemy.Color = RandomTools.RandomColor(Color.Orange, Color.Red);
            enemy.Hit(2, 1);

            if (ShouldRemove)
            {
                enemy.Color = Color.White;
            }
        }
    }
}
