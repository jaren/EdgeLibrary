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
        public FireEffect() : base("Fire", 100)
        {

        }

        public override void UpdateEffect(Enemy enemy)
        {
            base.UpdateEffect(enemy);
            enemy.Color = RandomTools.RandomColor(Color.Orange, Color.Red);
            enemy.Hit(2, 1);
        }
    }
}
