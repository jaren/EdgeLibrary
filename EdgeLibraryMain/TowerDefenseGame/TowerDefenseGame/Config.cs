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
        public static Difficulty Difficulty;

        public static List<EnemyData> Enemies = new List<EnemyData>()
        {
            new EnemyData(500, 1, 0, 50, 1, new List<EnemyData>(), "spikeBall1", Vector2.One*0.5f, 50, "Just a normal enemy."),
            new EnemyData(50000, 0.5f, 0, 50, 1, new List<EnemyData>(), "sun2", Vector2.One*1.5f, 50, "NOT just a normal enemy.")
        };

        private static float baseWaitTime = 1000f;

        #region Rounds List
        public static List<Round> RoundList = new List<Round>()
        {
            new Round(new List<RoundEnemyList>() //1
            {
                new RoundEnemyList(Enemy.Type.Default,baseWaitTime * 2f, 1)
            }),
            new Round(new List<RoundEnemyList>() //2
            {
                new RoundEnemyList(Enemy.Type.Default,baseWaitTime * 1.5f, 3)
            }),
            new Round(new List<RoundEnemyList>() //3
            {
                new RoundEnemyList(Enemy.Type.Default,baseWaitTime * 1.25f, 5)
            }),
            new Round(new List<RoundEnemyList>() //4
            {
                new RoundEnemyList(Enemy.Type.Default,baseWaitTime, 8)
            }),
            new Round(new List<RoundEnemyList>() //5
            {
                new RoundEnemyList(Enemy.Type.Default,baseWaitTime, 10),
                new RoundEnemyList(Enemy.Type.Boss,baseWaitTime * 4, 1)
            }),
            new Round(new List<RoundEnemyList>() //6
            {
                new RoundEnemyList(Enemy.Type.Default, baseWaitTime, 10),
                new RoundEnemyList(Enemy.Type.Default, baseWaitTime * 3, 1),
                new RoundEnemyList(Enemy.Type.Default, baseWaitTime, 9)
            }),
            new Round(new List<RoundEnemyList>() //7
            {
                new RoundEnemyList(Enemy.Type.Default, baseWaitTime, 5),
                new RoundEnemyList(Enemy.Type.Default, baseWaitTime * 3, 1),
                new RoundEnemyList(Enemy.Type.Default, baseWaitTime, 4),
                new RoundEnemyList(Enemy.Type.Default, baseWaitTime * 3, 1),
                new RoundEnemyList(Enemy.Type.Default, baseWaitTime, 4)
            }),
            new Round(new List<RoundEnemyList>() //8
            {
                new RoundEnemyList(Enemy.Type.Default,baseWaitTime, 20)
            }),
            new Round(new List<RoundEnemyList>() //9
            {
                new RoundEnemyList(Enemy.Type.Default, baseWaitTime, 10),
                new RoundEnemyList(Enemy.Type.Default, baseWaitTime * 5, 1),
                new RoundEnemyList(Enemy.Type.Default, baseWaitTime, 4),
                new RoundEnemyList(Enemy.Type.Default, baseWaitTime * 5, 1),
                new RoundEnemyList(Enemy.Type.Default, baseWaitTime, 9)
            }),
            new Round(new List<RoundEnemyList>() //10
            {
                new RoundEnemyList(Enemy.Type.Boss,baseWaitTime * 4, 10)
            }),
        };
        #endregion

        public static List<ProjectileData> Projectiles = new List<ProjectileData>()
        {
            //Normal
            new ProjectileData(10, 1000, 10, 0, 1, "particle_darkGrey", Vector2.One*0.5f, 1, 0),

            //High Speed Projectile
            new ProjectileData(50, 1000, 10, 0.2f, 1, "lightning_yellow", Vector2.One, 1, 0),

            #region Cluster Projectile
            new ProjectileData(10, 1000, 10, 0, 1, "particle_pink", Vector2.One, 1, 0, null, null, null, new Action<Projectile, Tower>( (projectile, tower) =>
            {
                ProjectileData clusterElement = new ProjectileData(10, 1000, 10, 0, 1, "particle_pink", Vector2.One, 1, 0);
                for (int i = 0; i < 10; i++)
                {
                    tower.Projectiles.Add(new Projectile(clusterElement, projectile.Target, 50, projectile.Position));
                }
            })),
            #endregion

            #region Exploding Projectile
                new ProjectileData(10, 500, 4, 0, 0, "coin_bronze", Vector2.One, 1, 0, new Action<Projectile, List<Enemy>, Tower>( (projectile, enemies, tower) =>
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
                if (projectile is ExplosionProjectile)
                {
                    ((ExplosionProjectile)projectile).Explode(enemies, tower);
                    ((ExplosionProjectile)projectile).ToDelete = true;
                }
            }), new Action<Projectile,Tower>( (projectile, tower) =>
            {
                projectile.ToDelete = true;
                tower.Projectiles.Add(new ExplosionProjectile(projectile.ProjectileData, "coin_silver", new Vector2(2f), projectile.Target, 100, projectile.Position, 300));
            })),
                #endregion

            #region Homing Projectile
            new ProjectileData(3, 1000, 10, 0, 1, "portal_yellowParticle", Vector2.One, 1, 0, new Action<Projectile, List<Enemy>, Tower>( (projectile, enemies, tower) =>
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
                #endregion

            #region Fire Projectile
            new ProjectileData(2, 800, 0, 0, 1, "portal_orangeParticle", Vector2.One, 1, 0, null, null, new Action<Projectile, List<Enemy>, Enemy, Tower>( (projectile, enemies, enemy, tower) =>
            {
                if (!enemy.HasEffect("Fire"))
                {
                    enemy.AddEffect(new FireEffect(3000));
                }
            })),
                #endregion

            #region Coin Projectile
            new ProjectileData(10, 1000, 10, 0, 1, "coin_gold", Vector2.One, 1, 0, null, null, null, new Action<Projectile, Tower>( (projectile, tower) =>
            {
                for (int i = 0; i < 10; i++)
                {
                    ProjectileData data = Projectiles[0];
                    data.Texture = "coin_gold";
                    Projectile p = new Projectile(data, projectile.Target, 50, projectile.Position);
                    tower.Projectiles.Add(p);
                }
            })),
            #endregion
        };

        public static List<TowerData> Towers = new List<TowerData>()
        {
            new TowerData(20, 1000, 400, 0, Projectiles[2], "enemyBlue1", MathHelper.ToRadians(180), Vector2.One, 100, "Spread"),
            new TowerData(7, 300, 600, 0, Projectiles[3], "enemyBlue2", MathHelper.ToRadians(180), Vector2.One, 300, "Explosive"),
            new TowerData(1, 100, 500, 100, Projectiles[4], "enemyBlue3", MathHelper.ToRadians(180), Vector2.One, 400, "Homing"),
            new TowerData(100, 200, 800, 80, Projectiles[5], "enemyBlue4", MathHelper.ToRadians(0), Vector2.One, 400, "Fire"),
            new TowerData(200, 10, 800, 80, Projectiles[6], "enemyBlue5", MathHelper.ToRadians(180), Vector2.One, 40000, "(Happy Face)")
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
