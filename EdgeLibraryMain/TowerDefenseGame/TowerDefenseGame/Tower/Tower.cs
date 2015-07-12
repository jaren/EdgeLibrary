using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class Tower : Sprite
    {
        public TowerData TowerData;
        public AttackTarget AttackTarget;
        public Ticker ShootTicker;
        private bool canShoot;
        public List<Projectile> Projectiles;
        private List<Projectile> projectilesToRemove;

        public Tower(TowerData data, Vector2 position)
            : base(data.Texture, position)
        {
            TowerData = data;
            ShootTicker = new Ticker(data.AttackSpeed);
            ShootTicker.OnTick += ShootTicker_OnTick;
            ShootTicker.Started = true;
            canShoot = false;
            Projectiles = new List<Projectile>();
        }

        void ShootTicker_OnTick(GameTime gameTime)
        {
            canShoot = true;
        }

        public void UpdateTower(List<Enemy> Enemies)
        {
            if (canShoot)
            {
                Enemy selectedTarget = SelectTarget(Enemies);
                if (selectedTarget != null)
                {
                    Projectiles.Add(new Projectile(TowerData.AttackData, selectedTarget, Position));
                    canShoot = false;
                }
            }

            foreach(Projectile projectile in Projectiles)
            {
                projectile.UpdateProjectile(Enemies);
            }
        }

        public override void UpdateObject(GameTime gameTime)
        {
            projectilesToRemove = new List<Projectile>();
            foreach(Projectile projectile in Projectiles)
            {
                projectile.Update(gameTime);
                if (projectile.ShouldBeRemoved)
                {
                    projectilesToRemove.Add(projectile);
                }
            }
            foreach(Projectile projectile in projectilesToRemove)
            {
                Projectiles.Remove(projectile);
            }

            base.UpdateObject(gameTime);
        }

        private Enemy SelectTarget(List<Enemy> Enemies)
        {
            List<Enemy> EnemiesInRange = new List<Enemy>();
            foreach (Enemy enemy in Enemies)
            {
                if (Vector2.DistanceSquared(enemy.Position, Position) <= (TowerData.Range * TowerData.Range))
                {
                    EnemiesInRange.Add(enemy);
                }
            }

            if (EnemiesInRange.Count == 0) { return null; }

            switch(AttackTarget)
            {
                case AttackTarget.First:
                    EnemiesInRange.OrderBy(x => x.TrackDistance);
                    return EnemiesInRange[0];
                    break;
                case AttackTarget.Last:
                    EnemiesInRange.OrderByDescending(x => x.TrackDistance);
                    return EnemiesInRange[0];
                    break;
                case AttackTarget.Strong:
                    EnemiesInRange.OrderByDescending(x => x.Health).ThenByDescending(x => x.EnemyData.Armor);
                    return EnemiesInRange[0];
                    break;
                case AttackTarget.Weak:
                    EnemiesInRange.OrderBy(x => x.Health).ThenBy(x => x.EnemyData.Armor);
                    return EnemiesInRange[0];
                    break;
            }
            return null;
        }
    }

    public enum AttackTarget
    {
        First,
        Strong,
        Last,
        Weak
    }

    public struct TowerData
    {
        public float AttackDamage;
        public float AttackSpeed;
        public float Range;
        public float Accuracy;
        public int Cost;
        public ProjectileData AttackData;
        public string Description;

        public System.Action SpecialActionsOnSelectTarget;
        public System.Action SpecialActionsOnShoot;
        public System.Action SpecialActionsOnCreate;
        public System.Action SpecialActionsOnSell;

        public string Texture;
        public Vector2 Scale;

        public TowerData(float attackDamage, float attackSpeed, float range, float accuracy, ProjectileData attackData, string texture, Vector2 scale, int cost, string description = "", System.Action specialActionsOnSelectTarget = null, System.Action specialActionsOnCreate = null, System.Action specialActionsOnShoot = null, System.Action specialActionsOnSell = null)
        {
            AttackDamage = attackDamage;
            AttackSpeed = attackSpeed;
            Range = range;
            AttackData = attackData;
            Texture = texture;
            Scale = scale;
            Accuracy = accuracy;
            Cost = cost;
            Description = description;
            SpecialActionsOnSelectTarget = specialActionsOnSelectTarget;
            SpecialActionsOnCreate = specialActionsOnCreate;
            SpecialActionsOnSell = specialActionsOnSell;
            SpecialActionsOnShoot = specialActionsOnShoot;
        }
    }
}
