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
        public EnemyData EnemyType;

        public Enemy(EnemyData data, Vector2 position) : base (data.Texture, position)
        {
            EnemyType = data;
        }
    }

    public struct EnemyData
    {
        public float Health;
        public float Speed;
        public float Armor;
        public int LivesTaken;
        public List<EnemyData> DeathEnemies;
        public string Description;

        public System.Action SpecialActionsOnCreate;
        public System.Action SpecialActionsOnDestroy;

        public string Texture;
        public Vector2 Scale;

        public EnemyData(float health, float speed, float armor, int livesTaken, List<EnemyData> deathEnemies, string texture, Vector2 scale, string description = "", System.Action specialActionsOnCreate = null, System.Action specialActionsOnDestroy = null)
        {
            Health = health;
            Speed = speed;
            Armor = armor;
            DeathEnemies = deathEnemies;
            Texture = texture;
            Scale = scale;
            LivesTaken = livesTaken;
            SpecialActionsOnCreate = specialActionsOnCreate;
            SpecialActionsOnDestroy = specialActionsOnDestroy;
            Description = description;
        }
    }
}
