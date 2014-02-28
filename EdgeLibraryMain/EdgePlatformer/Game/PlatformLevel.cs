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

        public PlatformLevel(string id, Vector2 gravity)
        {
            ID = id;
            sprites = new List<PlatformSprite>();
            nonplatformelements = new List<Element>();
            Gravity = gravity;
        }

        public void CreateScreenBox()
        {
            PlatformStatic top = new PlatformStatic(string.Format("{0}_topBox", ID), "Pixel", new Vector2(EdgeGame.WindowSize().X/2, 1));
            top.Scale = new Vector2(EdgeGame.WindowSize().X, 1);
            top.Style.Color = Color.White;
            sprites.Add(top);

            PlatformStatic bottom = new PlatformStatic(string.Format("{0}_bottomBox", ID), "Pixel", new Vector2(EdgeGame.WindowSize().X / 2, EdgeGame.WindowSize().Y));
            bottom.Scale = new Vector2(EdgeGame.WindowSize().X, 1);
            bottom.Style.Color = Color.White;
            sprites.Add(bottom);

            PlatformStatic right = new PlatformStatic(string.Format("{0}_rightBox", ID), "Pixel", new Vector2(1, EdgeGame.WindowSize().Y/2));
            right.Scale = new Vector2(1, EdgeGame.WindowSize().Y);
            right.Style.Color = Color.White;
            sprites.Add(right);

            PlatformStatic left = new PlatformStatic(string.Format("{0}_leftBox", ID), "Pixel", new Vector2(EdgeGame.WindowSize().X, EdgeGame.WindowSize().Y / 2));
            left.Scale = new Vector2(1, EdgeGame.WindowSize().Y);
            left.Style.Color = Color.White;
            sprites.Add(left);
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
