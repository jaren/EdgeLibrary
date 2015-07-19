using EdgeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public Vector2 Size;
        public string Name;
        public string Difficulty;
        public string Description;
        public Texture2D PreviewTexture;

        public Level(WaypointList waypoints, List<Restriction> restrictions, string texture, string name, string difficulty, string description)
            : base(texture, Vector2.Zero)
        {
            Waypoints = waypoints;
            Restrictions = restrictions;
            Size = new Vector2(EdgeGame.GetTexture(texture).Width, EdgeGame.GetTexture(texture).Height);
            Name = name;
            Difficulty = difficulty;
            Description = description;
            SamplerState = SamplerState.PointClamp;
            PreviewTexture = EdgeGame.GetTexture(texture + " Preview");
        }

        public void ResizeLevel(Vector2 size)
        {
            Vector2 ratio = new Vector2(size.X / Size.X, size.Y / Size.Y);

            foreach(Waypoint waypoint in Waypoints.Waypoints)
            {
                waypoint.Position = new Vector2(waypoint.Position.X * ratio.X, waypoint.Position.Y * ratio.Y);
            }
            foreach(Restriction restriction in Restrictions)
            {
                restriction.Resize(ratio);
            }

            Size = size;

            Scale = new Vector2(Size.X / Texture.Width, Size.Y / Texture.Height);
        }

        public override void DrawObject(GameTime gameTime)
        {
            base.DrawObject(gameTime);
        }

        public static Level ImportLevel(string xmlPath, string texture, string name = "Level", string difficulty = "Easy", string description = "A level")
        {
            string completePath = string.Format("{0}\\{1}.tmx", EdgeGame.ContentRootDirectory, xmlPath);
            XDocument document = XDocument.Load(completePath);

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
                                    foreach (string nextWaypoint in nextWaypointsString)
                                    {
                                        if (nextWaypoint != "")
                                        {
                                            nextWaypoints.Add(int.Parse(nextWaypoint));
                                        }
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
                            RestrictionType restrictionType = (element.Attribute("name").Value == Config.ObjectsXMLName ? RestrictionType.Object : element.Attribute("name").Value == Config.PathXMLName ? RestrictionType.Path : element.Attribute("name").Value == Config.WaterXMLName ? RestrictionType.Water : RestrictionType.Error);
                            if (restrictionType == RestrictionType.Error)
                            {
                                break;
                            }

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

            return new Level(new WaypointList(waypoints), restrictions, texture, name, difficulty, description);
        }
    }
}
