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

        public Tower(TowerData data, Vector2 position) : base(data.Texture, position)
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

        public System.Action SpecialActionsOnShoot;
        public System.Action SpecialActionsOnCreate;
        public System.Action SpecialActionsOnSell;

        public string Texture;
        public Vector2 Scale;

        public TowerData(float attackDamage, float attackSpeed, float range, float accuracy, ProjectileData attackData, string texture, Vector2 scale, int cost, string description = "", System.Action specialActionsOnCreate = null, System.Action specialActionsOnShoot = null, System.Action specialActionsOnSell = null)
        {
            AttackDamage = attackDamage;
            AttackSpeed = attackSpeed;
            Range = range;
            AttackData = attackData;
            Texture = texture;
            Scale = scale;
            Accuracy = accuracy;
            Cost = cost;
            Description = description;
            SpecialActionsOnCreate = specialActionsOnCreate;
            SpecialActionsOnSell = specialActionsOnSell;
            SpecialActionsOnShoot = specialActionsOnShoot;
        }
    }
}
