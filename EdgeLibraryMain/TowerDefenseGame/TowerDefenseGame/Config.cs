using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using EdgeLibrary;

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
            new EnemyData(50, 1, 1, 1, new List<EnemyData>(), "Enemy", Vector2.One, 50, "Just a normal enemy.")
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
            })
        };

        public static List<ProjectileData> Projectiles = new List<ProjectileData>()
        {
            new ProjectileData(500, 1000, 10, 0, 1, "Projectile", Vector2.One, 1, 0)
        };

        public static List<TowerData> Towers = new List<TowerData>()
        {
            new TowerData(20, 100, 150, 0, Projectiles[0], "Tower1", Vector2.One, 200, "Just a normal tower."),
            new TowerData(7, 20, 100, 0, Projectiles[0], "Tower2", Vector2.One, 300, "Fast firing"),
            new TowerData(100, 300, 500, 0, Projectiles[0], "Tower3", Vector2.One, 400, "S")
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
