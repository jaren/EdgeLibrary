using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;
using System.Xml.Linq;

namespace EdgeLibrary
{
    //May add more things to Scenes later on
    public class Scene : Element
    {
        public List<Element> elements;

        public Scene(string id) : base(false)
        {
            ID = id;
            elements = new List<Element>();
        }

        public void AddElement(Element element)
        {
            elements.Add(element);
        }

        public bool RemoveElement(string id)
        {
            foreach (Element element in elements)
            {
                if (element.ID == id)
                {
                    elements.Remove(element);
                    return true;
                }
            }
            return false;
        }

        public bool RemoveElement(Element element)
        {
            return elements.Remove(element);
        }

        public Element Element(string id)
        {
            foreach (Element element in elements)
            {
                if (element.ID == id)
                {
                    return element;
                }
            }
            return null;
        }

        public override void updateElement(GameTime gameTime)
        {
            foreach (Element element in elements)
            {
                element.Update(gameTime);
            }
        }

        public override void drawElement(GameTime gameTime)
        {
            foreach (Element element in elements)
            {
                element.Draw(gameTime);
            }
        }

        public void DrawDebug(GameTime gameTime)
        {
            if (Visible)
            {
                foreach (Element element in elements)
                {
                    try
                    {
                        ((Sprite)element).CollisionBody.Shape.DebugDraw(EdgeGame.DebugDrawColor);
                    }
                    catch { }
                }
            }
        }
    }
}
