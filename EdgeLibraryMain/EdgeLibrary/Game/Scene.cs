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
    public class Scene
    {
        public List<Element> elements;
        public string ID { get; set; }
        public Texture2D Background;

        public Scene(string id)
        {
            ID = id;
            elements = new List<Element>();
        }

        public virtual void AddElement(Element element)
        {
            elements.Add(element);
            DebugLogger.LogAdd("Element", "       ID: " + element.ID, "        Type: " + element.GetType());
        }

        public virtual bool RemoveElement(string id)
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

        public virtual bool RemoveElement(Element element)
        {
            return elements.Remove(element);
        }

        public virtual Element Element(string id)
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

        public virtual void Update(GameTime gameTime)
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

        public virtual void Draw(GameTime gameTime)
        {
            elements = elements.OrderBy(X => X.DrawLayer).ToList();

            if (Background != null)
            {
                EdgeGame.drawTexture(Background, Vector2.Zero, null, Color.White, Vector2.One, 0, Vector2.Zero, SpriteEffects.None);
            }

            foreach (Element element in elements)
            {
                element.Draw(gameTime);
            }

        }

        public virtual void DrawDebug(GameTime gameTime)
        {
                foreach (Element element in elements)
                {
                    if (element is Sprite)
                    {
                        ((Sprite)element).DebugDraw(EdgeGame.DebugDrawColor);
                    }
                }
        }
    }
}
