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

        public Scene(string id)
        {
            ID = id;

            Elements = new List<Element>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Element element in Elements)
            {
                element.Update(gameTime);
            }

            for (int i = 0; i < Elements.Count; i++)
            {
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Element element in Elements)
            {
                element.Draw(gameTime, spriteBatch);
            }
        }

        public void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Element element in Elements)
            {
                if (element is Sprite)
                {
                    ((Sprite)element).DebugDraw(gameTime, spriteBatch);
                }
            }
        }
    }
}
