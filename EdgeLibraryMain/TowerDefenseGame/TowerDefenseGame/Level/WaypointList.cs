using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefenseGame
{
    public class WaypointList
    {
        public List<Waypoint> Waypoints;

        public WaypointList(List<Waypoint> waypoints)
        {
            Waypoints = waypoints;
        }

        public List<Waypoint> NextWaypoint(Waypoint waypoint)
        {
            List<Waypoint> returnWaypoints = new List<Waypoint>();

            foreach(Waypoint innerWaypoint in Waypoints)
            {
                if (waypoint.NextID.Contains(innerWaypoint.ID))
                {
                    returnWaypoints.Add(innerWaypoint);
                }
            }

            return returnWaypoints;
        }
    }

    public class Waypoint
    {
        //The ID used to identify the waypoint to others
        public int ID;

        //0 - Start
        //1 - Normal
        //2 - Finish
        public int Type;

        //All the possible next waypoints
        public List<int> NextID;

        //The position of the waypoint
        public Vector2 Position;

        public Waypoint(int id, int type, List<int> nextID, Vector2 position)
        {
            ID = id;
            Type = type;
            NextID = nextID;
            Position = position;
        }
    }
}
