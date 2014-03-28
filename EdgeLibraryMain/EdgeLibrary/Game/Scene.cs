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
    public class Scene
    {
        public string ID { get; protected set; }

        private List<Element> Elements;

        //Used to check if two elements have the same ID
        private List<string> elementIDs;

        public Scene(string id)
        {
            ID = id;

            Elements = new List<Element>();

            elementIDs = new List<string>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Element element in Elements)
            {
                element.Update(gameTime);

                //If the element is a sprite, then update the collision
                if (element is Sprite)
                {
                    ((Sprite)element).UpdateCollision(gameTime, Elements);
                }
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

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Orders the elements by their draw layer
            Elements = Elements.OrderBy(x => x.DrawLayer).ToList();

            foreach (Element element in Elements)
            {
                element.Draw(gameTime, spriteBatch);
            }
        }

        public void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            foreach (Element element in Elements)
            {
                if (element is Sprite)
                {
                    ((Sprite)element).DrawDebug(gameTime, spriteBatch, color);
                }
            }
        }
    }
}
