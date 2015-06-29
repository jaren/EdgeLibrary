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

        public Projectile(ProjectileData data, Vector2 position) : base(data.Texture, position)
        {
            ProjectileData = data;
        }
    }

    public struct ProjectileData
    {
        public float MovementSpeed;
        public float Range;
        public int MaxEnemyPierce;
        public float ArmorPierce;

        public System.Action SpecialActionsOnHit;
        public System.Action SpecialActionsOnCreate;

        public string Texture;
        public Vector2 Scale;
        public float BaseRotation;

        public ProjectileData(float movementSpeed, float range, float armorPierce, int maxEnemyPierce, string texture, Vector2 scale, float baseRotation, System.Action specialActionsOnHit = null, System.Action specialActionsOnCreate = null)
        {
            MovementSpeed = movementSpeed;
            Range = range;
            ArmorPierce = armorPierce;
            MaxEnemyPierce = maxEnemyPierce;
            Texture = texture;
            Scale = scale;
            BaseRotation = baseRotation;
            SpecialActionsOnHit = specialActionsOnHit;
            SpecialActionsOnCreate = specialActionsOnHit;
        }
    }
}
