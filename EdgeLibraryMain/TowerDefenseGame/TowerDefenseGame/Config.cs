﻿using System;
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
        public static int[] StartingMoneyNumber = new int[] { 600, 500, 400 };
        public static Difficulty Difficulty;
        public static bool DebugMode = false;
        public static bool ShowRanges = false;

        public static Vector2 CommonRatio = new Vector2(0.85f);

        public static Dictionary<string, EnemyData> Enemies = new Dictionary<string, EnemyData>()
        {
            {"Normal", new EnemyData(500, 1, 0, 50, 1, new List<EnemyData>(), "spikeBall1", Vector2.One*0.5f, 50, 8, "Just a normal enemy.")},
            {"Boss Slow", new EnemyData(10000, 0.5f, 0, 100, 2, new List<EnemyData>(), "sun2", Vector2.One*1.5f, 50, 2, "NOT just a normal enemy.")},
            {"Fast", new EnemyData(300, 2, 0, 50, 1, new List<EnemyData>(), "ufoRed", Vector2.One * 0.5f, 50, 1, "A slightly less normal enemy.")},
            {"Boss Fast", new EnemyData(10000, 2, 0, 750, 10, new List<EnemyData>(), "ufoRed", Vector2.One * 2f, 50, 1, "Vous etes mort.")}
        };

        private static float baseWaitTime = 1000f;

        #region Rounds List
        public static List<Round> RoundList = new List<Round>()
        {
            new Round(new List<RoundEnemyList>() //1
            {
                new RoundEnemyList("Normal",baseWaitTime * 2f, 1)
            }),
            new Round(new List<RoundEnemyList>() //2
            {
                new RoundEnemyList("Normal",baseWaitTime * 1.5f, 3)
            }),
            new Round(new List<RoundEnemyList>() //3
            {
                new RoundEnemyList("Normal",baseWaitTime * 1.25f, 5)
            }),
            new Round(new List<RoundEnemyList>() //4
            {
                new RoundEnemyList("Normal",baseWaitTime, 8),
            }),
            new Round(new List<RoundEnemyList>() //5
            {
                new RoundEnemyList("Normal",baseWaitTime, 5),
                new RoundEnemyList("Fast", baseWaitTime * 1.25f, 1),
                new RoundEnemyList("Boss Slow",baseWaitTime * 4, 1)
            }),
            new Round(new List<RoundEnemyList>() //6
            {
                new RoundEnemyList("Normal", baseWaitTime, 10),
                new RoundEnemyList("Fast", baseWaitTime * 3, 1),
                new RoundEnemyList("Fast", baseWaitTime * 1.25f, 2),
                new RoundEnemyList("Normal", baseWaitTime * 3, 1),
                new RoundEnemyList("Normal", baseWaitTime, 9)
            }),
            new Round(new List<RoundEnemyList>() //7
            {
                new RoundEnemyList("Normal", baseWaitTime, 10),
                new RoundEnemyList("Fast", baseWaitTime * 3, 1),
                new RoundEnemyList("Fast", baseWaitTime, 4),
                new RoundEnemyList("Normal", baseWaitTime * 3, 1),
                new RoundEnemyList("Normal", baseWaitTime, 9)
            }),
            new Round(new List<RoundEnemyList>() //8
            {
                new RoundEnemyList("Normal",baseWaitTime, 15),
                new RoundEnemyList("Fast", baseWaitTime * 3f, 1),
                new RoundEnemyList("Fast", baseWaitTime * 1.25f, 9)
            }),
            new Round(new List<RoundEnemyList>() //9
            {
                new RoundEnemyList("Normal",baseWaitTime, 5),
                new RoundEnemyList("Fast", baseWaitTime, 5),
                new RoundEnemyList("Fast", baseWaitTime * 5, 1),
                new RoundEnemyList("Fast", baseWaitTime, 4),
                new RoundEnemyList("Fast", baseWaitTime * 5, 1),
                new RoundEnemyList("Fast", baseWaitTime, 4)
            }),
            new Round(new List<RoundEnemyList>() //10
            {
                new RoundEnemyList("Fast", baseWaitTime * 1.5f, 20),
                new RoundEnemyList("Boss Slow",baseWaitTime * 4, 10)
            }),
            new Round(new List<RoundEnemyList>() //11
            {
                new RoundEnemyList("Boss Fast", baseWaitTime * 1.5f, 1)
            }),
        };
        #endregion

        public static Dictionary<string, ProjectileData> Projectiles = new Dictionary<string, ProjectileData>()
        {
            {"Normal", new ProjectileData(10, 1000, 0, 1, "particle_darkGrey", Color.White, Vector2.One*0.5f, 1, 0)},

            {"High Speed", new ProjectileData(50, 1000, 0.5f, 5, "lighting_yellow", Color.White, Vector2.One * 1.5f, 1, 0)},

            {"Cluster", new ProjectileData(10, 1000, 0, 1, "particle_pink", Color.White, Vector2.One, 1, 0, null, null, null, new Action<Projectile, Tower>( (projectile, tower) =>
            {
                ProjectileData clusterElement = new ProjectileData(10, 1000, 0, 1, "particle_pink", Color.White, Vector2.One, 1, 0);
                for (int i = 0; i < 10; i++)
                {
                    tower.Projectiles.Add(new Projectile(clusterElement, projectile.Damage, projectile.Target, 50, projectile.Position) { Rotation = tower.Rotation + projectile.ProjectileData.BaseRotation });
                }
            }))},

            {"Explosive", new ProjectileData(10, 500, 0, 0, "coin_bronze", Color.White, Vector2.One, 1, 0, null, null, ExplosionProjectileExplode, new Action<Projectile,Tower>( (projectile, tower) =>
            {
                ExplosionProjectileCreate(projectile, tower, 150, "coin_silver", 61, 0);
            }))},

            {"Homing", new ProjectileData(10, 1000, 0, 1, "portal_yellowParticle", Color.White, Vector2.One, 1, 0, HomingProjectileHome)},

            {"Homing Explosive", new ProjectileData(10, 1000, 0, 1, "portal_yellowParticle", Color.White, Vector2.One, 1, 0, HomingProjectileHome, null, 
            ExplosionProjectileExplode, new Action<Projectile,Tower>( (projectile, tower) =>
            {
                ExplosionProjectileCreate(projectile, tower, 150, "coin_silver", 61, 0);
            }))},

            {"Fire", new ProjectileData(5, 350, 0, 2, "flame", Color.White, new Vector2(1), 1, 0, null, null, new Action<Projectile, List<Enemy>, Enemy, Tower>( (projectile, enemies, enemy, tower) =>
            {
                EnemyAddFireEffect(enemy, 3000);
            }))},

            {"Cluster Fire", new ProjectileData(7, 500, 0, 2, "flameBlue", Color.White, new Vector2(1), 1, 0, null, null, new Action<Projectile, List<Enemy>, Enemy, Tower>( (projectile, enemies, enemy, tower) =>
            {
                EnemyAddFireEffect(enemy, 3000);
            }), new Action<Projectile,Tower>( (projectile, tower) => 
            {
                ProjectileData clusterElement = new ProjectileData(7, 500, 0, 2, "flameBlue", Color.White, new Vector2(1), 1, 0, null, null, new Action<Projectile, List<Enemy>, Enemy, Tower>((eProjectile, eEnemies, eEnemy, eTower) =>
                {
                    EnemyAddFireEffect(eEnemy, 3000);
                }));

                for (int i = 0; i < 10; i++)
                {
                    tower.Projectiles.Add(new Projectile(clusterElement, projectile.Damage, projectile.Target, 50, projectile.Position) { Rotation = tower.Rotation + projectile.ProjectileData.BaseRotation});
                }
            }))},
        };

        public static void EnemyAddFireEffect(Enemy enemy, int duration)
        {
            if (enemy.HasEffect("Fire"))
            {
                ((FireEffect)enemy.GetEffect("Fire")).Duration = duration;
            }
            else
            {
                enemy.AddEffect(new FireEffect(duration));
            }
        }

        public static void HomingProjectileHome(Projectile projectile, List<Enemy> enemies, Tower tower)
        {
            if (projectile.Target.ShouldBeRemoved == false && !projectile.Target.CompletedPath)
            {
                projectile.RemoveAction("MoveAction");
                Vector2 differenceVector = projectile.Target.Position - projectile.Position;
                differenceVector.Normalize();
                differenceVector = new Vector2(differenceVector.X * projectile.ProjectileData.MovementSpeed * EdgeGame.GameSpeed, differenceVector.Y * projectile.ProjectileData.MovementSpeed * EdgeGame.GameSpeed);
                projectile.Position += differenceVector;
            }
            else
            {
                bool foundTarget = false;
                foreach (Enemy enemy in enemies)
                {
                    if (!enemy.ShouldBeRemoved && !projectile.Target.CompletedPath)
                    {
                        foundTarget = true;
                        projectile.Target = enemy;
                        projectile.ProjectileData.SpecialActionsOnUpdate(projectile, enemies, tower);
                    }
                }

                if (!foundTarget && projectile.MiscData == null)
                {
                    projectile.AddAction(projectile.MoveAction);
                    projectile.MiscData = true;
                }
            }
        }

        public static void ExplosionProjectileExplode(Projectile projectile, List<Enemy> enemies, Enemy enemy, Tower tower)
        {
            if (projectile is ExplosionProjectile)
            {
                ((ExplosionProjectile)projectile).Explode(enemies, tower);
                ((ExplosionProjectile)projectile).ToDelete = true;
            }
        }

        public static void ExplosionProjectileCreate(Projectile projectile, Tower tower, float explosionRadius, string texture, float textureSize, int accuracy)
        {
            projectile.ToDelete = true;
            tower.Projectiles.Add(new ExplosionProjectile(projectile.ProjectileData, projectile.Damage, texture, new Vector2(explosionRadius / textureSize * 2), projectile.Target, accuracy, projectile.Position, explosionRadius));
        }

        public static List<TowerData> Towers = new List<TowerData>()
        {
            //Base Towers
            new TowerData("Spread", 25, 1000, 400, 0, Projectiles["Cluster"], "enemyBlue1", MathHelper.ToRadians(180), new Vector2(0.5f), 200, (PlaceableArea.Land), ""),
            new TowerData("Slow", 0, 0, 100, 0, new ProjectileData(), "enemyBlue2", MathHelper.ToRadians(180), new Vector2(0.5f), 500, (PlaceableArea.Land), "", null, null, new Action<Tower, List<Enemy>>((tower, enemies) =>
                {
                    foreach(Enemy enemy in enemies)
                    {
                        if (Vector2.DistanceSquared(enemy.Position, tower.Position) <= (tower.TowerData.Range*tower.TowerData.Range))
                        {
                            enemy.AddEffect(new SlowEffect(0.5f, 2000));
                        }
                    }
                }), null, null, false),

            new TowerData("Homing", 10, 250, 350, 0, Projectiles["Homing"], "enemyBlue3", MathHelper.ToRadians(180), new Vector2(0.5f), 750, (PlaceableArea.Land), ""),
            new TowerData("Fire", 0, 1500, 200, 25, Projectiles["Fire"], "enemyBlue4", MathHelper.ToRadians(0), new Vector2(0.5f), 300, (PlaceableArea.Land), ""),
            new TowerData("High Speed", 100, 3000, 450, 0, Projectiles["High Speed"], "enemyBlue5", MathHelper.ToRadians(180), new Vector2(0.5f), 400, (PlaceableArea.Land), ""),
            
            //Upgrades
            new TowerData("Homing Explosives", 20, 550, 400, 0, Projectiles["Homing Explosive"], "enemyRed3", MathHelper.ToRadians(180), new Vector2(0.5f), 1500, (PlaceableArea.Land), "Homing"),
            new TowerData("Cluster Fire", 0, 1500, 400, 25, Projectiles["Cluster Fire"], "enemyRed4", MathHelper.ToRadians(0), new Vector2(0.5f), 1500, (PlaceableArea.Land), "Fire"),
            new TowerData("Slow Fire", 0, 0, 150, 0, new ProjectileData(), "enemyRed2", MathHelper.ToRadians(180), new Vector2(0.5f), 1500, (PlaceableArea.Land), "Slow", null, null, new Action<Tower, List<Enemy>>((tower, enemies) =>
                {
                    foreach(Enemy enemy in enemies)
                    {
                        if (Vector2.DistanceSquared(enemy.Position, tower.Position) <= (tower.TowerData.Range*tower.TowerData.Range))
                        {
                            enemy.AddEffect(new SlowEffect(0.5f, 3000));

                            if (enemy.HasEffect("Fire"))
                            {
                                ((FireEffect)enemy.GetEffect("Fire")).Duration = 3000;
                            }
                            else
                            {
                                enemy.AddEffect(new FireEffect(3000));
                            }
                        }
                    }
                }), null, null, false),
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
