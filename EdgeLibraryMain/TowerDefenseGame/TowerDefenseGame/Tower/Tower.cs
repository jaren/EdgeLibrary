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

        public Tower(TowerData data) : base(data.Texture, Vector2.Zero)
        {
            TowerData = data;
        }
    }

    public struct TowerData
    {
        public float AttackDamage;
        public float AttackSpeed;
        public float ArmorPierce;
        public float Range;
        public float Accuracy;
        public int Cost;
        public ProjectileData AttackData;

        public string Texture;
        public Vector2 Scale;

        public TowerData(float attackDamage, float attackSpeed, float armorPierce, float range, float accuracy, ProjectileData attackData, string texture, Vector2 scale, int cost)
        {
            AttackDamage = attackDamage;
            AttackSpeed = attackSpeed;
            ArmorPierce = armorPierce;
            Range = range;
            AttackData = attackData;
            Texture = texture;
            Scale = scale;
            Accuracy = accuracy;
            Cost = cost;
        }

        public void SpecialActions()
        {

        }
    }

    public struct ProjectileData
    {
        public float MovementSpeed;
        public float Range;
        public int MaxEnemyPierce;

        public ProjectileData(float movementSpeed, float range, int maxEnemyPierce)
        {
            MovementSpeed = movementSpeed;
            Range = range;
            MaxEnemyPierce = maxEnemyPierce;
        }

        public void SpecialActions()
        {

        }
    }
}
