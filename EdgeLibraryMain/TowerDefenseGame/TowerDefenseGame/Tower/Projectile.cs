using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class Projectile : Sprite
    {
        public ProjectileData ProjectileData;
        public Enemy Target;
        public List<Enemy> PiercedEnemies;
        public int PiercedEnemiesCount;

        public Projectile(ProjectileData data, Enemy target, Vector2 position)
            : base(data.Texture, position)
        {
            ProjectileData = data;
            Target = target;

            PiercedEnemies = new List<Enemy>();
            PiercedEnemiesCount = 0;

            Vector2 targetPosition = target.Position - Position;
            targetPosition.Normalize();
            targetPosition = new Vector2(targetPosition.X * ProjectileData.Range, targetPosition.Y * ProjectileData.Range);
            AMoveTo moveAction = new AMoveTo(targetPosition, ProjectileData.MovementSpeed);
            moveAction.OnFinish += moveAction_OnFinish;
            AddAction(moveAction);
        }

        void moveAction_OnFinish(EdgeLibrary.Action action, GameTime gameTime, Sprite sprite)
        {
            markForDeletion();
        }

        public void UpdateProjectile(List<Enemy> Enemies)
        {
            foreach(Enemy enemy in Enemies)
            {
                if (!PiercedEnemies.Contains(enemy))
                {
                    if (CollisionDetection.CircleCircle(Position, ProjectileData.CollisionRadius, enemy.Position, enemy.EnemyData.CollisionRadius))
                    {
                        enemy.Hit(ProjectileData.Damage, ProjectileData.ArmorPierce);

                        if (ProjectileData.SpecialActionsOnHit != null)
                        {
                            ProjectileData.SpecialActionsOnHit(this, enemy);
                        }

                        PiercedEnemies.Add(enemy);
                        PiercedEnemiesCount++;
                        if (PiercedEnemiesCount >= ProjectileData.MaxEnemyPierce)
                        {
                            markForDeletion();
                        }
                    }
                }
            }
        }

        private void markForDeletion()
        {
            if (ProjectileData.SpecialActionsOnDestroy != null)
            {
                ProjectileData.SpecialActionsOnDestroy(this);
            }
            ShouldBeRemoved = true;
        }
    }

    public struct ProjectileData
    {
        public float MovementSpeed;
        public float Range;
        public int MaxEnemyPierce;
        public float Damage;
        public float ArmorPierce;

        public System.Action<Projectile, Enemy> SpecialActionsOnHit;
        public System.Action<Projectile> SpecialActionsOnCreate;
        public System.Action<Projectile> SpecialActionsOnDestroy;

        public string Texture;
        public Vector2 Scale;
        public float BaseRotation;

        //For the base texture without scale - it will be multiplied with scale
        public float CollisionRadius;

        public ProjectileData(float movementSpeed, float range, float damage, float armorPierce, int maxEnemyPierce, string texture, Vector2 scale, float collisionRadius, float baseRotation, System.Action<Projectile> specialActionsOnDestroy = null, Action<Projectile, Enemy> specialActionsOnHit = null, System.Action<Projectile> specialActionsOnCreate = null)
        {
            MovementSpeed = movementSpeed;
            Range = range;
            Damage = damage;
            ArmorPierce = armorPierce;
            MaxEnemyPierce = maxEnemyPierce;
            Texture = texture;
            Scale = scale;
            CollisionRadius = collisionRadius * (float)Math.Sqrt(Scale.X * Scale.Y);
            BaseRotation = baseRotation;
            SpecialActionsOnDestroy = specialActionsOnDestroy;
            SpecialActionsOnHit = specialActionsOnHit;
            SpecialActionsOnCreate = specialActionsOnCreate;
        }
    }
}
