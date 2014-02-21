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
    public class Scene
    {
        public List<Element> elements;
        public string ID { get; set; }

        public Scene(string id)
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

        public void Update(GameTime gameTime)
        {

            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].Update(gameTime);

                if (elements[i].MarkedForRemoval)
                {
                    elements.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (Element element in elements)
            {
                element.Draw(gameTime);
            }
        }

        public void DrawDebug(GameTime gameTime)
        {
                foreach (Element element in elements)
                {
                    if (element is Sprite)
                    {
                        if (!(element is TextSprite) || EdgeGame.CollisionsInTextSprites)
                        {
                            ((Sprite)element).CollisionBody.Shape.DebugDraw(EdgeGame.DebugDrawColor);
                        }
                    }
                }
        }
    }
}
