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


namespace EdgeLibrary
{
    //A container for elements
    public class Panel : Element
    {
        protected List<Element> Elements;

        //Used to check if two elements have the same ID
        private List<string> elementIDs;

        public Panel(string id, params Element[] elementsToAdd)
        {
            Elements = new List<Element>();

            foreach (Element e in elementsToAdd)
            {
                Elements.Add(e);
            }
        }

        //Adds an element
        public void AddElement(Element element)
        {
            Elements.Add(element);
        }

        //Removes an element
        public bool RemoveElement(Element element)
        {
            return Elements.Remove(element);
        }
        public bool RemoveElement(string id)
        {
            return RemoveElement(GetElement(id));
        }

        //Gets an element by ID
        public Element GetElement(string id)
        {
            foreach (Element element in Elements)
            {
                if (element.ID == id)
                {
                    return element;
                }
            }
            return null;
        }

        //Updates all the elements in the list
        protected override void UpdateObject(GameTime gameTime)
        {
            foreach (Element element in Elements)
            {
                element.Update(gameTime);
            }

            //Checks if two elements have the same ID
            elementIDs.Clear();
            foreach (Element e in Elements)
            {
                if (elementIDs.Contains(e.ID))
                {
                    throw new Exception(string.Format("There was a duplicate element ID. ID:{0}", e.ID));
                }
                elementIDs.Add(e.ID);
            }

            //Checks if any elements need to be removed
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i].toRemove == true)
                {
                    Elements.Remove(Elements[i]);
                    i--;
                }
            }
        }

        protected override void  DrawObject(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Orders the elements by their draw layer
            Elements = Elements.OrderBy(x => x.DrawLayer).ToList();

            foreach (Element element in Elements)
            {
                element.Draw(gameTime, spriteBatch);
            }
        }
    }
}
