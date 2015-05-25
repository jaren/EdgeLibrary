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
        public WaypointList Waypoints;
        public List<Restriction> Restrictions;


        public Level(WaypointList waypoints, List<Restriction> restrictions, string texture) : base(texture, Vector2.Zero)
        {
            Waypoints = waypoints;
            Restrictions = restrictions;
        }
    }
}
