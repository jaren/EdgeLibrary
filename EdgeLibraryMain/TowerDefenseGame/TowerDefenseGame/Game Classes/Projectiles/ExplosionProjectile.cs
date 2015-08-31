using EdgeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class ExplosionProjectile : Projectile
    {
        float ExplosionRadius;
        Texture2D ExplosionTexture;
        Vector2 ExplosionScale;

        public ExplosionProjectile(ProjectileData data, float damage, string explosionTexture, Vector2 explosionScale, Enemy target, float accuracy, Vector2 position, float explosionRadius)
            : base(data, damage, target, accuracy, position)
        {
            ExplosionRadius = explosionRadius;
            ExplosionScale = explosionScale;
            ExplosionTexture = EdgeGame.GetTexture(explosionTexture);
        }

        public void Explode(List<Enemy> enemies, Tower tower)
        {
            Texture = ExplosionTexture;
            Scale = ExplosionScale;

            foreach (Enemy enemy in enemies)
            {
                if (CollisionDetection.CircleCircle(Position, ExplosionRadius, enemy.Position, enemy.EnemyData.CollisionRadius))
                {
                    enemy.Hit(Damage, ProjectileData.ArmorPierce);

                    //Causes a crash: stack overflow, infinite loop
                    //if (ProjectileData.SpecialActionsOnHit != null)
                    //{
                    //    ProjectileData.SpecialActionsOnHit(this, enemies, enemy, tower);
                    //}
                }
            }
        }
    }
}
