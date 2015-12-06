using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class DamageEffect : Effect
    {
        Color ColorMin;
        Color ColorMax;
        float Damage;

        public DamageEffect(string name, float damage, Color colorMin, Color colorMax, float duration) : base(name, duration, 100)
        {
            ColorMin = colorMin;
            ColorMax = colorMax;
            Damage = damage;
        }

        public override void UpdateEffect(GameTime gameTime, Enemy enemy)
        {
            base.UpdateEffect(gameTime, enemy);
            enemy.Color = RandomTools.RandomColor(ColorMin, ColorMin);
            enemy.Hit(Damage, 1);

            if (ShouldRemove)
            {
                enemy.Color = Color.White;
            }
        }
    }
}
