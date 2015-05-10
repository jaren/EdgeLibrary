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
        public EnemyType EnemyType;

        public Enemy(EnemyType type) : base (type.Texture, Vector2.Zero)
        {
            EnemyType = type;
        }
    }

    public struct EnemyType
    {
        public float Health;
        public float Speed;
        public float Armor;
        public int LivesTaken;
        public List<EnemyType> DeathEnemies;

        public string Texture;
        public Vector2 Scale;

        public EnemyType(float health, float speed, float armor, int livesTaken, List<EnemyType> deathEnemies, string texture, Vector2 scale)
        {
            Health = health;
            Speed = speed;
            Armor = armor;
            DeathEnemies = deathEnemies;
            Texture = texture;
            Scale = scale;
            LivesTaken = livesTaken;
        }

        public void SpecialActions()
        {

        }
    }
}
