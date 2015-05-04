using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class Level : Sprite
    {
        public List<Vector2> Waypoints;

        public Level(List<Vector2> waypoints, string texture) : base(texture, Vector2.Zero)
        {
            Waypoints = waypoints;
        }
    }
}
