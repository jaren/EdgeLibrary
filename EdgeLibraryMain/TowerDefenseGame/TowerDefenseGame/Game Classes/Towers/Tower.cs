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
        public Enemy Target;
        public Ticker ShootTicker;
        private bool canShoot;
        public List<Projectile> Projectiles;
        private List<Projectile> projectilesToRemove;

        private List<Sprite> previousTargets = new List<Sprite>();
        private Sprite towerRange;

        public Tower(TowerData data, Vector2 position)
            : base(data.Texture, position)
        {
            TowerData = data;
            ShootTicker = new Ticker(data.AttackSpeed);
            ShootTicker.OnTick += ShootTicker_OnTick;
            ShootTicker.Started = true;
            canShoot = false;
            Projectiles = new List<Projectile>();
            towerRange = new Sprite("Circle", Position);
            towerRange.Scale = new Vector2(TowerData.Range / 500f);
            Scale = TowerData.Scale;
        }

        void ShootTicker_OnTick(GameTime gameTime)
        {
            canShoot = true;
        }

        public void UpdateTower(List<Enemy> Enemies)
        {
            Target = SelectTarget(Enemies);

            if (Target != null)
            {
                Rotation = -1f * (float)Math.Atan2(Position.X - Target.Position.X, Position.Y - Target.Position.Y) + TowerData.BaseRotation;
            }

            if (canShoot)
            {
                if (Target != null)
                {
                    Projectile projectile = new Projectile(TowerData.AttackData, Target, TowerData.Accuracy, Position);
                    if (projectile.ProjectileData.SpecialActionsOnCreate != null)
                    {
                        projectile.ProjectileData.SpecialActionsOnCreate(projectile, this);
                    }

                    projectile.Rotation = Rotation + projectile.ProjectileData.BaseRotation;
                    Projectiles.Add(projectile);
                    canShoot = false;
                    ShootTicker.elapsedMilliseconds = 0;

                    projectile.TargetPosition.Normalize();
                    previousTargets.Clear();

                    if (Config.DebugMode)
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            previousTargets.Add(new Sprite("portal_orangeParticle", Position + projectile.TargetPosition * i) { Color = Color.Red, Scale = Vector2.One * 0.1f });
                        }
                    }
                }
            }

            foreach (Projectile projectile in Projectiles)
            {
                projectile.UpdateProjectile(Enemies, this);
            }
        }

        public override void UpdateObject(GameTime gameTime)
        {
            ShootTicker.Update(gameTime);
            projectilesToRemove = new List<Projectile>();
            towerRange.Visible = Config.ShowRanges;

            foreach (Projectile projectile in Projectiles)
            {
                projectile.Update(gameTime);
                if (projectile.ShouldBeRemoved)
                {
                    projectilesToRemove.Add(projectile);
                }
            }
            foreach (Projectile projectile in projectilesToRemove)
            {
                Projectiles.Remove(projectile);
            }

            base.UpdateObject(gameTime);
        }

        public override void DrawObject(GameTime gameTime)
        {
            towerRange.Draw(gameTime);

            foreach(Sprite sprite in previousTargets)
            {
                sprite.Draw(gameTime);
            }

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

            switch (AttackTarget)
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

    [Flags]
    public enum PlaceableArea
    {
        Land = 1,
        Water = 2
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
        public PlaceableArea PlaceableArea;
        public ProjectileData AttackData;
        public string Description;

        public System.Action<Tower, Enemy> SpecialActionsOnSelectTarget;
        public System.Action<Tower, Enemy> SpecialActionsOnShoot;
        public System.Action<Tower> SpecialActionsOnCreate;
        public System.Action<Tower> SpecialActionsOnSell;

        public string Texture;
        public Vector2 Scale;
        public float BaseRotation;

        public TowerData(float attackDamage, float attackSpeed, float range, float accuracy, ProjectileData attackData, string texture, float baseRotation, Vector2 scale, int cost, PlaceableArea placeableArea, string description = "", System.Action<Tower, Enemy> specialActionsOnSelectTarget = null, System.Action<Tower> specialActionsOnCreate = null, System.Action<Tower, Enemy> specialActionsOnShoot = null, System.Action<Tower> specialActionsOnSell = null)
        {
            AttackDamage = attackDamage;
            AttackSpeed = attackSpeed;
            Range = range;
            AttackData = attackData;
            Texture = texture;
            Scale = scale;
            Accuracy = accuracy;
            Cost = cost;
            PlaceableArea = placeableArea;
            Description = description;
            BaseRotation = baseRotation;
            SpecialActionsOnSelectTarget = specialActionsOnSelectTarget;
            SpecialActionsOnCreate = specialActionsOnCreate;
            SpecialActionsOnSell = specialActionsOnSell;
            SpecialActionsOnShoot = specialActionsOnShoot;
        }
    }
}
