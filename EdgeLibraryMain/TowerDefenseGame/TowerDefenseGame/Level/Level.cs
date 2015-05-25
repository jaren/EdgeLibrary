using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TowerDefenseGame
{
    public class Level : Sprite
    {
        public WaypointList Waypoints;
        public List<Restriction> Restrictions;

        public Level(WaypointList waypoints, List<Restriction> restrictions, string texture)
            : base(texture, Vector2.Zero)
        {
            Waypoints = waypoints;
            Restrictions = restrictions;
        }

        public static Level ImportLevel(string xmlPath, string texture)
        {
            string completePath = string.Format("{0}\\{1}.tmx", EdgeGame.ContentRootDirectory, xmlPath);
            XDocument document = XDocument.Load(xmlPath);

            List<Waypoint> waypoints = new List<Waypoint>();
            List<Restriction> restrictions = new List<Restriction>();

            foreach(XElement element in document.Root.Elements())
            {
                if (element.Name == "objectgroup")
                {
                    bool Ellipse = false;

                    foreach(XElement innerElement in element.Elements())
                    {
                        Ellipse = false;

                        if(innerElement.HasElements)
                        {
                            if (innerElement.Elements().ToList()[0].Name == "ellipse")
                            {
                                Ellipse = true;
                            }
                        }
                        else
                        {
                            Ellipse = false;
                        }

                        if (element.Attribute("name").Value == Config.WaypointsXMLName)
                        {
                            float x = float.Parse(innerElement.Attribute("x").Value) + (float.Parse(innerElement.Attribute("width").Value) / 2f);
                            float y = float.Parse(innerElement.Attribute("y").Value) + (float.Parse(innerElement.Attribute("height").Value) / 2f);
                            List<int> nextWaypoints = new List<int>();
                            int id = 0;
                            int waypointType = 0;

                            foreach(XElement waypointProperty in innerElement.Elements().ToList()[0].Elements())
                            {
                                if (waypointProperty.Attribute("name").Value == "NextWaypoints")
                                {
                                    string[] nextWaypointsString = waypointProperty.Attribute("value").Value.Split(',');
                                    foreach(string nextWaypoint in nextWaypointsString)
                                    {
                                        nextWaypoints.Add(int.Parse(nextWaypoint));
                                    }
                                }
                                else if (waypointProperty.Attribute("name").Value == "WaypointID")
                                {
                                    id = int.Parse(waypointProperty.Attribute("value").Value);
                                }
                                else if (waypointProperty.Attribute("name").Value == "WaypointType")
                                {
                                    waypointType = int.Parse(waypointProperty.Attribute("value").Value);
                                }
                            }

                            waypoints.Add(new Waypoint(id, waypointType, nextWaypoints, new Vector2(x, y)));
                        }
                        else
                        {
                            RestrictionType restrictionType = (element.Attribute("name").Value == Config.ObjectsXMLName ? RestrictionType.Object : element.Attribute("name").Value == Config.PathXMLName ? RestrictionType.Path : element.Attribute("name").Value == Config.WaterXMLName ? RestrictionType.Water : RestrictionType.Object);
                            float x = float.Parse(innerElement.Attribute("x").Value);
                            float y = float.Parse(innerElement.Attribute("y").Value);
                            float width = float.Parse(innerElement.Attribute("width").Value);
                            float height = float.Parse(innerElement.Attribute("height").Value);

                            if (Ellipse)
                            {
                                float radius = (width + height)/4f;
                                restrictions.Add(new CircleRestriction(radius, new Vector2(x + radius, y + radius), restrictionType));
                            }
                            else
                            {
                                restrictions.Add(new Restriction(new Rectangle((int)x, (int)y, (int)width, (int)height), restrictionType));
                            }
                        }
                    }
                }
            }

            return new Level(new WaypointList(waypoints), restrictions, texture);
        }
    }
}
