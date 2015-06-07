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
        public float Range;
        public float Accuracy;
        public int Cost;
        public ProjectileData AttackData;
        public string Description;

        public System.Action SpecialActions;

        public string Texture;
        public Vector2 Scale;

        public TowerData(float attackDamage, float attackSpeed, float range, float accuracy, ProjectileData attackData, string texture, Vector2 scale, int cost, string description = "", System.Action specialActions = null)
        {
            AttackDamage = attackDamage;
            AttackSpeed = attackSpeed;
            Range = range;
            AttackData = attackData;
            Texture = texture;
            Scale = scale;
            Accuracy = accuracy;
            Cost = cost;
            SpecialActions = specialActions;
            Description = description;
        }
    }

    public struct ProjectileData
    {
        public float MovementSpeed;
        public float Range;
        public int MaxEnemyPierce;
        public float ArmorPierce;

        public System.Action SpecialActions;

        public ProjectileData(float movementSpeed, float range, float armorPierce, int maxEnemyPierce, System.Action specialActions)
        {
            MovementSpeed = movementSpeed;
            Range = range;
            ArmorPierce = armorPierce;
            MaxEnemyPierce = maxEnemyPierce;
            SpecialActions = specialActions;
        }
    }
}
