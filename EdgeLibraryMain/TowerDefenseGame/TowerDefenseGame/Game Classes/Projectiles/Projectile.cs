using EdgeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public AMoveTo MoveAction;
        public Object MiscData;
        private bool ToDelete = false;

        public Projectile(ProjectileData data, Enemy target, float accuracy, Vector2 position)
            : base(data.Texture, position, Color.White, data.Scale)
        {
            ProjectileData = data;
            Target = target;

            PiercedEnemies = new List<Enemy>();
            PiercedEnemiesCount = 0;

            Vector2 targetPosition = target.Position - Position;
            targetPosition.Normalize();
            targetPosition = new Vector2(targetPosition.X * ProjectileData.Range, targetPosition.Y * ProjectileData.Range);
            targetPosition = new Vector2(targetPosition.X + RandomTools.RandomFloat(-accuracy * ProjectileData.Range / 100f, accuracy * ProjectileData.Range / 100f), targetPosition.Y + RandomTools.RandomFloat(-accuracy * ProjectileData.Range / 100f, accuracy * ProjectileData.Range / 100f));
            MoveAction = new AMoveTo(targetPosition, ProjectileData.MovementSpeed);
            MoveAction.OnFinish += moveAction_OnFinish;
            AddAction("MoveAction", MoveAction);
        }

        void moveAction_OnFinish(EdgeLibrary.Action action, GameTime gameTime, Sprite sprite)
        {
            ToDelete = true;
        }

        public void UpdateProjectile(List<Enemy> Enemies, Tower tower)
        {
            if (ProjectileData.SpecialActionsOnUpdate != null)
            {
                ProjectileData.SpecialActionsOnUpdate(this, Enemies, tower);
            }

            if (ToDelete)
            {
                markForDeletion(tower);
            }

            foreach(Enemy enemy in Enemies)
            {
                if (!PiercedEnemies.Contains(enemy))
                {
                    if (CollisionDetection.CircleCircle(Position, ProjectileData.CollisionRadius, enemy.Position, enemy.EnemyData.CollisionRadius))
                    {
                        enemy.Hit(ProjectileData.Damage, ProjectileData.ArmorPierce);

                        if (ProjectileData.SpecialActionsOnHit != null)
                        {
                            ProjectileData.SpecialActionsOnHit(this, enemy, tower);
                        }

                        PiercedEnemies.Add(enemy);
                        PiercedEnemiesCount++;
                        if (PiercedEnemiesCount >= ProjectileData.MaxEnemyPierce)
                        {
                            markForDeletion(tower);
                        }
                    }
                }
            }
        }

        private void markForDeletion(Tower tower)
        {
            if (ProjectileData.SpecialActionsOnDestroy != null)
            {
                ProjectileData.SpecialActionsOnDestroy(this, tower);
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

        public System.Action<Projectile, Enemy, Tower> SpecialActionsOnHit;
        public System.Action<Projectile, List<Enemy>, Tower> SpecialActionsOnUpdate;
        public System.Action<Projectile, Tower> SpecialActionsOnCreate;
        public System.Action<Projectile, Tower> SpecialActionsOnDestroy;

        public string Texture;
        public Vector2 Scale;
        public float BaseRotation;

        //For the base texture without scale - it will be multiplied with scale
        public float CollisionRadius;

        public ProjectileData(float movementSpeed, float range, float damage, float armorPierce, int maxEnemyPierce, string texture, Vector2 scale, float collisionRadius, float baseRotation, System.Action<Projectile, List<Enemy>, Tower> specialActionsOnUpdate = null, System.Action<Projectile, Tower> specialActionsOnDestroy = null, Action<Projectile, Enemy, Tower> specialActionsOnHit = null, System.Action<Projectile, Tower> specialActionsOnCreate = null)
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
            SpecialActionsOnUpdate = specialActionsOnUpdate;
            SpecialActionsOnDestroy = specialActionsOnDestroy;
            SpecialActionsOnHit = specialActionsOnHit;
            SpecialActionsOnCreate = specialActionsOnCreate;
        }
    }
}
