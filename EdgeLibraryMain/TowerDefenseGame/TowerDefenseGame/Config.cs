using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using EdgeLibrary;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseGame
{
    public static class Config
    {
        public static float[] EnemyHealthMultiplier = new float[] { 0.5f, 1f, 2f };
        public static float[] TowerCostMultiplier = new float[] { 0.75f, 1f, 1.25f };
        public static int[] LivesNumber = new int[] { 25, 10, 1 };
        public static int[] StartingMoneyNumber = new int[] { 600, 550, 500 };

        public static List<EnemyData> Enemies = new List<EnemyData>()
    {
        new EnemyData(500, 1, 0, 50, 1, new List<EnemyData>(), "Enemy", Vector2.One, 50, "Just a normal enemy."),
        new EnemyData(50000, 0.5f, 0, 50, 1, new List<EnemyData>(), "BossEnemy", Vector2.One*2, 50, "Too much health.")
    };

        private static float round1Wait = 2000f;
        private static float round2Wait = 1500f;
        private static float round3Wait = 1000f;
        private static float round4Wait = 500f;
        public static List<Round> RoundList = new List<Round>()
    {
        new Round(new List<KeyValuePair<EnemyData,float>>() 
        {
            new KeyValuePair<EnemyData, float>(Enemies[0], round1Wait),
            new KeyValuePair<EnemyData, float>(Enemies[0], round1Wait)
        }),
        new Round(new List<KeyValuePair<EnemyData,float>>() 
        {
            new KeyValuePair<EnemyData, float>(Enemies[0], round2Wait),
            new KeyValuePair<EnemyData, float>(Enemies[0], round2Wait),
            new KeyValuePair<EnemyData, float>(Enemies[0], round2Wait),
            new KeyValuePair<EnemyData, float>(Enemies[0], round2Wait)
        }),
        new Round(new List<KeyValuePair<EnemyData,float>>() 
        {
            new KeyValuePair<EnemyData, float>(Enemies[0], round3Wait),
            new KeyValuePair<EnemyData, float>(Enemies[0], round3Wait),
            new KeyValuePair<EnemyData, float>(Enemies[0], round3Wait),
            new KeyValuePair<EnemyData, float>(Enemies[0], round3Wait),
            new KeyValuePair<EnemyData, float>(Enemies[0], round3Wait),
            new KeyValuePair<EnemyData, float>(Enemies[0], round3Wait)
        }),
        new Round(new List<KeyValuePair<EnemyData,float>>() 
        {
            new KeyValuePair<EnemyData, float>(Enemies[0], round4Wait),
            new KeyValuePair<EnemyData, float>(Enemies[0], round4Wait),
            new KeyValuePair<EnemyData, float>(Enemies[0], round4Wait),
            new KeyValuePair<EnemyData, float>(Enemies[0], round4Wait),
            new KeyValuePair<EnemyData, float>(Enemies[0], round4Wait),
            new KeyValuePair<EnemyData, float>(Enemies[0], round4Wait),
            new KeyValuePair<EnemyData, float>(Enemies[0], round4Wait),
            new KeyValuePair<EnemyData, float>(Enemies[0], round4Wait)
        }),
        new Round(new List<KeyValuePair<EnemyData,float>>() 
        {
            new KeyValuePair<EnemyData, float>(Enemies[1], round3Wait)
        }),
    };

        public static List<ProjectileData> Projectiles = new List<ProjectileData>()
    {
        new ProjectileData(10, 1000, 10, 0, 1, "Projectile", Vector2.One, 1, 0), //Normal
        new ProjectileData(50, 1000, 10, 0.2f, 1, "Projectile", Vector2.One, 1, 0), //High speed
        new ProjectileData(10, 1000, 10, 0, 1, "Projectile2", Vector2.One, 1, 0, null, null, null, new Action<Projectile, Tower>( (projectile, tower) => //Cluster
        {
            ProjectileData clusterElement = new ProjectileData(10, 1000, 10, 0, 1, "Projectile2", Vector2.One, 1, 0);
            for (int i = 0; i < 10; i++)
            {
                tower.Projectiles.Add(new Projectile(clusterElement, projectile.Target, 50, projectile.Position));
            }
        })),
        new ProjectileData(10, 500, 4, 0, 0, "ExplosiveProjectile", Vector2.One, 1, 0, new Action<Projectile, List<Enemy>, Tower>( (projectile, enemies, tower) => //Exploding
        {
            if (projectile.Target.ShouldBeRemoved == false)
            {
                projectile.RemoveAction("MoveAction");
                Vector2 differenceVector = projectile.Target.Position - projectile.Position;
                differenceVector.Normalize();
                differenceVector = new Vector2(differenceVector.X * projectile.ProjectileData.MovementSpeed, differenceVector.Y * projectile.ProjectileData.MovementSpeed);
                projectile.Position += differenceVector;
            }
            else
            {
                if (projectile.MiscData == null)
                {
                    projectile.AddAction(projectile.MoveAction);
                    projectile.MiscData = true;
                }
            }
        }), null, new Action<Projectile, List<Enemy>, Enemy, Tower>( (projectile, enemies, enemy, tower) =>
        {
            ((ExplosionProjectile)projectile).Explode(enemies, tower);
            ((ExplosionProjectile)projectile).ToDelete = true;
        }), new Action<Projectile,Tower>( (projectile, tower) => 
        {
            projectile.ToDelete = true;
            tower.Projectiles.Add(new ExplosionProjectile(projectile.ProjectileData, "Explosion", projectile.Target, 100, projectile.Position, 300));
        })),
        new ProjectileData(3, 1000, 10, 0, 1, "Projectile", Vector2.One, 1, 0, new Action<Projectile, List<Enemy>, Tower>( (projectile, enemies, tower) =>  //Homing
        {
            if (projectile.Target.ShouldBeRemoved == false)
            {
                projectile.RemoveAction("MoveAction");
                Vector2 differenceVector = projectile.Target.Position - projectile.Position;
                differenceVector.Normalize();
                differenceVector = new Vector2(differenceVector.X * projectile.ProjectileData.MovementSpeed, differenceVector.Y * projectile.ProjectileData.MovementSpeed);
                projectile.Position += differenceVector;
            }
            else
            {
                if (projectile.MiscData == null)
                {
                    projectile.AddAction(projectile.MoveAction);
                    projectile.MiscData = true;
                }
            }
        })),
        new ProjectileData(2, 800, 0, 0, 1, "Fire", Vector2.One*2, 1, 0, null, null, new Action<Projectile, List<Enemy>, Enemy, Tower>( (projectile, enemies, enemy, tower) =>  //Fire
        {
            if (!enemy.HasEffect("Fire"))
            {
                enemy.AddEffect(new FireEffect());
            }
        }))
    };

        public static List<TowerData> Towers = new List<TowerData>()
    {
        new TowerData(20, 1000, 400, 0, Projectiles[2], "Tower1", Vector2.One, 100, "Spread"),
        new TowerData(7, 300, 600, 0, Projectiles[3], "Tower2", Vector2.One, 300, "Explosive"),
        new TowerData(1, 50, 500, 100, Projectiles[4], "Tower3", Vector2.One, 400, "Homing"),
        new TowerData(100, 0, 800, 80, Projectiles[5], "Tower2", Vector2.One, 400, "Fire")
    };

        public static string TrackEasyDifficulty = "Easy";
        public static string TrackMediumDifficulty = "Medium";
        public static string TrackHardDifficulty = "Hard";

        public static string EasyDifficulty = "Easy";
        public static string MediumDifficulty = "Medium";
        public static string HardDifficulty = "Hard";

        public static string StatusFont = "ComicSans-20";
        public static string BigStatusFont = "ComicSans-40";
        public static string SquareFont = "ComicSans-20";
        public static string DebugFont = "Impact-20";

        public static string MenuTitleFont = "Georgia-50";
        public static string MenuMiniTitleFont = "Georgia-30";
        public static string MenuSubtitleFont = "Georgia-20";
        public static string MenuButtonTextFont = "Georgia-20";
        public static Color MenuButtonColor = Color.DarkOrange;
        public static Color MenuTextColor = Color.Orange;

        public static string WaypointsXMLName = "Waypoints";
        public static string ObjectsXMLName = "DecorationCollision";
        public static string PathXMLName = "Path";
        public static string WaterXMLName = "Water";

        public static string ButtonNormalTexture = "grey_button03";
        public static string ButtonClickTexture = "grey_button02";
        public static string ButtonMouseOverTexture = "grey_button01";

        public static float CameraZoomSpeed = 1;
        public static float CameraMaxZoom = 10f;
        public static float CameraMinZoom = 300f;
        public static float CameraScrollSpeed = 10f;

        public static Keys BackKey = Keys.Escape;
    }
}
