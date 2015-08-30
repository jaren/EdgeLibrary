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
        public float RealMaxHealth;
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
        public bool CompletedPath;
        private Waypoint currentWaypoint;
        private Sprite enemyHealthBar = new Sprite("health10", Vector2.Zero);
        private TextSprite debugEnemyHealth = new TextSprite("Georgia-20", "", Vector2.Zero, Color.White, Vector2.One);
        public delegate void EnemyEvent(Enemy enemy, Waypoint waypoint);
        public event EnemyEvent OnReachWaypoint;

        public Enemy(EnemyData data, Vector2 position)
            : base(data.Texture, position)
        {
            EnemyData = data;
            Scale = data.Scale;
            TrackDistance = 0;
            RealMaxHealth = EnemyData.MaxHealth * Config.EnemyHealthMultiplier[(int)Config.Difficulty];
            Health = RealMaxHealth;
            CompletedPath = false;
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

        public bool RemoveEffect(string name)
        {
            Effect badEffect = null;
            foreach (Effect addedEffect in Effects)
            {
                if (addedEffect.Name == name)
                {
                    badEffect = addedEffect;
                }
            }
            if (badEffect != null)
            {
                Effects.Remove(badEffect);
                return true;
            }
            return false;
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

            int textureNumber = (int)Math.Round((Health / RealMaxHealth) * 10);
            enemyHealthBar.TextureName = "health" + (textureNumber > 10 ? 10 : textureNumber);
            enemyHealthBar.Scale = new Vector2(0.75f);

            float healthBarYPos = Position.Y - ((Height / 2f) * Scale.Y);
            float altHealthBarYPos = Position.Y + ((Height / 2f) * Scale.Y);

            enemyHealthBar.Position = new Vector2(Position.X, healthBarYPos < 0 ? altHealthBarYPos : healthBarYPos);
            enemyHealthBar.Update(gameTime);

            debugEnemyHealth.Text = Health.ToString();
            debugEnemyHealth.Position = Position;
            debugEnemyHealth.Update(gameTime);
            debugEnemyHealth.Visible = Config.DebugMode;

            for (int i = 0; i < Effects.Count(); i++)
            {
                Effects[i].UpdateEffect(gameTime, this);
                if (Effects[i].ShouldRemove)
                {
                    Effects.Remove(Effects[i]);
                    i--;
                }
            }
        }

        public override void DrawObject(GameTime gameTime)
        {
            base.DrawObject(gameTime);
            enemyHealthBar.Draw(gameTime);
            debugEnemyHealth.Draw(gameTime);
        }

        public enum Type
        {
            Default,
            Boss,
            Ufo,
            BossUfo
        }
    }

    public struct EnemyData
    {
        public float MaxHealth;
        public float Speed;
        public int Usualness;

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

        public EnemyData(float maxHealth, float speed, float armor, int moneyOnDeath, int livesTaken, List<EnemyData> deathEnemies, string texture, Vector2 scale, float collisionRadius, int usualness, string description = "", System.Action<Enemy> specialActionsOnCreate = null, System.Action<Enemy> specialActionsOnHit = null, System.Action<Enemy> specialActionsOnDestroy = null)
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
            Usualness = usualness;
        }
    }
}
