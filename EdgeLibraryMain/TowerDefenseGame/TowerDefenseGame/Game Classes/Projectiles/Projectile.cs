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
        public Vector2 TargetPosition;
        public Enemy Target;
        public float Damage;
        public List<Enemy> PiercedEnemies;
        public int PiercedEnemiesCount;
        public AMoveTo MoveAction;
        public Object MiscData;
        public bool ToDelete = false;

        public Projectile(ProjectileData data, float damage, Enemy target, float accuracy, Vector2 position)
            : base(data.Texture, position, Color.White, data.Scale)
        {
            ProjectileData = data;
            Scale = data.Scale;
            Color = data.Color;

            PiercedEnemies = new List<Enemy>();
            PiercedEnemiesCount = 0;

            Damage = damage;

            if (target != null)
            {
                Target = target;
                SetTargetPosition(target.Position, accuracy);
            }
        }

        public void SetTargetPosition(Vector2 targetPosition, float accuracy)
        {
            TargetPosition = targetPosition - Position;
            TargetPosition.Normalize();
            TargetPosition = new Vector2(TargetPosition.X * ProjectileData.Range, TargetPosition.Y * ProjectileData.Range);
            TargetPosition = new Vector2(TargetPosition.X + RandomTools.RandomFloat(-accuracy * ProjectileData.Range / 100f, accuracy * ProjectileData.Range / 100f), TargetPosition.Y + RandomTools.RandomFloat(-accuracy * ProjectileData.Range / 100f, accuracy * ProjectileData.Range / 100f));
            TargetPosition = TargetPosition + Position;
            MoveAction = new AMoveTo(TargetPosition, ProjectileData.MovementSpeed);
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
                        enemy.Hit(Damage, ProjectileData.ArmorPierce);

                        if (ProjectileData.SpecialActionsOnHit != null)
                        {
                            ProjectileData.SpecialActionsOnHit(this, Enemies, enemy, tower);
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
        public float ArmorPierce;

        public System.Action<Projectile, List<Enemy>, Enemy, Tower> SpecialActionsOnHit;
        public System.Action<Projectile, List<Enemy>, Tower> SpecialActionsOnUpdate;
        public System.Action<Projectile, Tower> SpecialActionsOnCreate;
        public System.Action<Projectile, Tower> SpecialActionsOnDestroy;

        public string Texture;
        public Color Color;
        public Vector2 Scale;
        public float BaseRotation;

        //For the base texture without scale - it will be multiplied with scale
        public float CollisionRadius;

        public ProjectileData(float movementSpeed, float range, float armorPierce, int maxEnemyPierce, string texture, Color color, Vector2 scale, float collisionRadius, float baseRotation, System.Action<Projectile, List<Enemy>, Tower> specialActionsOnUpdate = null, System.Action<Projectile, Tower> specialActionsOnDestroy = null, Action<Projectile, List<Enemy>, Enemy, Tower> specialActionsOnHit = null, System.Action<Projectile, Tower> specialActionsOnCreate = null)
        {
            MovementSpeed = movementSpeed;
            Range = range;
            ArmorPierce = armorPierce;
            MaxEnemyPierce = maxEnemyPierce;
            Texture = texture;
            Scale = scale;
            Color = color;
            CollisionRadius = collisionRadius * (float)Math.Sqrt(Scale.X * Scale.Y);
            BaseRotation = baseRotation;
            SpecialActionsOnUpdate = specialActionsOnUpdate;
            SpecialActionsOnDestroy = specialActionsOnDestroy;
            SpecialActionsOnHit = specialActionsOnHit;
            SpecialActionsOnCreate = specialActionsOnCreate;
        }
    }
}
