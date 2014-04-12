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
        //Used to identify the scene
        public string ID { get; protected set; }

        //The elements in the current scene
        public List<Element> Elements { get; protected set; }

        //The image to display on the background of the screen
        public Texture2D Background;

        //Used to check if two elements have the same ID
        private List<string> elementIDs;

        public Scene(string id)
        {
            ID = id;

            Elements = new List<Element>();

            elementIDs = new List<string>();
        }

        //Adds an element
        public void AddElement(Element element)
        {
            DebugLogger.LogAdd(element.GetType().Name + " Added", "ID: " + element.ID);
            Elements.Add(element);
        }

        //Removes an element
        public bool RemoveElement(Element element)
        {
            DebugLogger.LogRemove(element.GetType().Name + " Removed", "ID: " + element.ID);
            return Elements.Remove(element);
        }
        public bool RemoveElement(string element)
        {
            return RemoveElement(GetElement(element));
        }
        public void RemoveElement(int index)
        {
            Elements.RemoveAt(index);
        }

        //Gets the element with the given ID
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

        //Updates all of the elements in the scene
        public virtual void Update(GameTime gameTime)
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
                    Elements.RemoveAt(i);
                    i--;
                }
            }
        }

        //Renders the scene to a Texture2D
        public Texture2D RenderToTexture(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            RenderTarget2D target = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            graphicsDevice.SetRenderTarget(target);
            graphicsDevice.Clear(EdgeGame.Instance.ClearColor);
            spriteBatch.Begin();
            Draw(gameTime, spriteBatch);
            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
            return target;
        }

        //Draws all of the elements in the scene
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Background != null)
            {
                spriteBatch.Draw(Background, new Rectangle(0, 0, (int)EdgeGame.Instance.WindowSize.X, (int)EdgeGame.Instance.WindowSize.Y), Color.White);
            }

            //Orders the elements by their draw layer
            Elements = Elements.OrderBy(x => x.DrawLayer).ToList();

            foreach (Element element in Elements)
            {
                element.Draw(gameTime, spriteBatch);
            }
        }

        //Draws all the collision bodies of the elements in the scene
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
