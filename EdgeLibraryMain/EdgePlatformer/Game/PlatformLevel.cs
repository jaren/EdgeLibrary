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
using EdgeLibrary;

namespace EdgeLibrary.Platform
{
    //The equivalent of a "Scene"
    public class PlatformLevel
    {
        public List<PlatformSprite> sprites;
        //For text sprites, etc.
        public List<Element> nonplatformelements;
        public string ID { get; set; }
        public Vector2 Gravity { get; set; }

        public PlatformLevel(string id)
        {
            ID = id;
            sprites = new List<PlatformSprite>();
            nonplatformelements = new List<Element>();
            Gravity = new Vector2(0, -9.8f);
        }

        public void AddSprite(PlatformSprite sprite)
        {
            sprites.Add(sprite);
        }

        public bool RemoveSprite(string id)
        {
            foreach (PlatformSprite sprite in sprites)
            {
                if (sprite.ID == id)
                {
                    sprites.Remove(sprite);
                    return true;
                }
            }
            return false;
        }

        public bool RemoveSprite(PlatformSprite sprite)
        {
            return sprites.Remove(sprite);
        }

        public PlatformSprite Sprite(string id)
        {
            foreach (PlatformSprite sprite in sprites)
            {
                if (sprite.ID == id)
                {
                    return sprite;
                }
            }
            return null;
        }

        public virtual void Update(GameTime gameTime)
        {

            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].UpdateSprite(gameTime, Gravity, sprites);

                if (sprites[i].MarkedForPlatformRemoval)
                {
                    sprites.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < nonplatformelements.Count; i++)
            {
                nonplatformelements[i].Update(gameTime);

                if (nonplatformelements[i].MarkedForRemoval)
                {
                    nonplatformelements.RemoveAt(i);
                    i--;
                }
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            sprites = sprites.OrderBy(X => X.DrawLayer).ToList();

            foreach (PlatformSprite sprite in sprites)
            {
                sprite.Draw(gameTime);
            }

            nonplatformelements = nonplatformelements.OrderBy(X => X.DrawLayer).ToList();

            foreach (Element element in nonplatformelements)
            {
                element.Draw(gameTime);
            }
        }
    }
}
