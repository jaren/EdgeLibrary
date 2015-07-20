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
            //TODO: Make it so the tower only shoots when the round is started
            if (canShoot)
            {
                Enemy selectedTarget = SelectTarget(Enemies);
                if (selectedTarget != null)
                {
                    Projectile projectile = new Projectile(TowerData.AttackData, selectedTarget, TowerData.Accuracy, Position);
                    if (projectile.ProjectileData.SpecialActionsOnCreate != null)
                    {
                        projectile.ProjectileData.SpecialActionsOnCreate(projectile);
                    }
                    Projectiles.Add(projectile);
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
            ShootTicker.Update(gameTime);
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

        public override void DrawObject(GameTime gameTime)
        {
            foreach(Projectile projectile in Projectiles)
            {
                projectile.Draw(gameTime);
            }
            base.DrawObject(gameTime);
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

        //0 - Perfect accuracy
        //Any number higher than 0 is the spread (in pixels) at 100 distance from the tower
        public float Accuracy;
        public int Cost;
        public ProjectileData AttackData;
        public string Description;

        public System.Action<Tower, Enemy> SpecialActionsOnSelectTarget;
        public System.Action<Tower, Enemy> SpecialActionsOnShoot;
        public System.Action<Tower> SpecialActionsOnCreate;
        public System.Action<Tower> SpecialActionsOnSell;

        public string Texture;
        public Vector2 Scale;

        public TowerData(float attackDamage, float attackSpeed, float range, float accuracy, ProjectileData attackData, string texture, Vector2 scale, int cost, string description = "", System.Action<Tower, Enemy> specialActionsOnSelectTarget = null, System.Action<Tower> specialActionsOnCreate = null, System.Action<Tower, Enemy> specialActionsOnShoot = null, System.Action<Tower> specialActionsOnSell = null)
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
