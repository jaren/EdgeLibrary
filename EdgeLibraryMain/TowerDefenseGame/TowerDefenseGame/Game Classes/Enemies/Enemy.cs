using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class Enemy : Sprite
    {
        public EnemyData EnemyData;
        public float TrackDistance;
        public float Health;
        public List<Effect> Effects;
        public Waypoint CurrentWaypoint
        {
            get
            {
                return currentWaypoint;
            }
            set
            {
                currentWaypoint = value;
                AMoveTo moveAction = new AMoveTo(currentWaypoint.Position, EnemyData.Speed);
                moveAction.OnFinish += moveAction_OnFinish;
                AddAction(moveAction);
            }
        }
        private Waypoint currentWaypoint;

        public delegate void EnemyEvent(Enemy enemy, Waypoint waypoint);
        public event EnemyEvent OnReachWaypoint;

        public Enemy(EnemyData data, Vector2 position)
            : base(data.Texture, position)
        {
            EnemyData = data;
            TrackDistance = 0;
            Health = EnemyData.MaxHealth;
            Effects = new List<Effect>();
        }

        public void Hit(float damage, float armorPierce)
        {
            if (EnemyData.SpecialActionsOnHit != null)
            {
                EnemyData.SpecialActionsOnHit(this);
            }

            Health -= damage - (EnemyData.Armor * (1 - armorPierce));

            if (Health <= 0)
            {
                if (EnemyData.SpecialActionsOnDestroy != null)
                {
                    EnemyData.SpecialActionsOnDestroy(this);
                }
                ShouldBeRemoved = true;
            }
        }

        void moveAction_OnFinish(EdgeLibrary.Action action, GameTime gameTime, Sprite sprite)
        {
            OnReachWaypoint(this, currentWaypoint);
        }

        public void AddEffect(Effect effect)
        {
            Effects.Add(effect);
        }

        public bool HasEffect(string name)
        {
            foreach (Effect addedEffects in Effects)
            {
                if (addedEffects.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public override void UpdateObject(GameTime gameTime)
        {
            base.UpdateObject(gameTime);

            foreach (Effect effect in Effects)
            {
                effect.UpdateEffect(this);
            }
        }
    }

    public struct EnemyData
    {
        public float MaxHealth;
        public float Speed;

        //Armor should be at least 1. Anything less will cause it to take more than normal damage
        public float Armor;

        public int MoneyOnDeath;

        public int LivesTaken;
        public List<EnemyData> DeathEnemies;
        public string Description;

        public System.Action<Enemy> SpecialActionsOnCreate;
        public System.Action<Enemy> SpecialActionsOnHit;
        public System.Action<Enemy> SpecialActionsOnDestroy;

        public string Texture;
        public Vector2 Scale;

        //For the base texture without scale - it will be multiplied with scale
        public float CollisionRadius;

        public EnemyData(float maxHealth, float speed, float armor, int moneyOnDeath, int livesTaken, List<EnemyData> deathEnemies, string texture, Vector2 scale, float collisionRadius, string description = "", System.Action<Enemy> specialActionsOnCreate = null, System.Action<Enemy> specialActionsOnHit = null, System.Action<Enemy> specialActionsOnDestroy = null)
        {
            MaxHealth = maxHealth;
            Speed = speed;
            Armor = armor;
            MoneyOnDeath = moneyOnDeath;
            DeathEnemies = deathEnemies;
            Texture = texture;
            Scale = scale;
            CollisionRadius = collisionRadius * (float)Math.Sqrt(scale.X * scale.Y);
            LivesTaken = livesTaken;
            SpecialActionsOnCreate = specialActionsOnCreate;
            SpecialActionsOnHit = specialActionsOnHit;
            SpecialActionsOnDestroy = specialActionsOnDestroy;
            Description = description;
        }
    }
}
