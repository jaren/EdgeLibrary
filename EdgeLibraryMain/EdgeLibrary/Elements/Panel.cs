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

namespace EdgeLibrary
{
    public class Panel : Element
    {
        protected List<Element> elements;

        public Panel(string id, params Element[] elementsToAdd) : base(id)
        {
            elements = new List<Element>();

            foreach (Element e in elementsToAdd)
            {
                elements.Add(e);
            }
        }

        public void AddElement(Element element)
        {
            element.REMOVE();
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

        protected static int updateCounter = 0;
        protected static int drawCounter = 0;

        public override void updateElement(GameTime gameTime)
        {
            updateCounter++;

            foreach (Element element in elements)
            {

                element.Update(gameTime);
            }
        }

        public override void drawElement(GameTime gameTime)
        {
            drawCounter++;

            foreach (Element element in elements)
            {
                element.Draw(gameTime);
            }
        }
    }
}
