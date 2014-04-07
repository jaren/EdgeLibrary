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
    public class PanelSprite : Sprite
    {
        public List<Sprite> Sprites;
        private List<string> spriteIDs;

        public PanelSprite(string textureName, Vector2 position) : base(textureName, position)
        {
            Sprites = new List<Sprite>();
            spriteIDs = new List<string>();
        }

        public PanelSprite(string textureName, Vector2 position, Color color, Vector2 scale, float rotation, SpriteEffects spriteEffects) : base(textureName, position, color, scale, rotation, spriteEffects)
        {
            Sprites = new List<Sprite>();
            spriteIDs = new List<string>();
        }

        //Adds a sprite
        public void AddSprite(Sprite sprite)
        {
            Sprites.Add(sprite);
        }

        //Removes a sprite
        public bool RemoveSprite(Sprite sprite)
        {
            return Sprites.Remove(sprite);
        }
        public bool RemoveSprite(string id)
        {
            return RemoveSprite(GetSprite(id));
        }

        //Gets a sprite by ID
        public Sprite GetSprite(string id)
        {
            foreach (Sprite sprite in Sprites)
            {
                if (sprite.ID == id)
                {
                    return sprite;
                }
            }
            return null;
        }

        //Updates all the sprites in the list
        protected override void UpdateObject(GameTime gameTime)
        {
            foreach (Sprite sprite in Sprites)
            {
                sprite.Update(gameTime);
            }

            //Checks if two sprites have the same ID
            spriteIDs.Clear();
            foreach (Sprite sprite in Sprites)
            {
                if (spriteIDs.Contains(sprite.ID))
                {
                    throw new Exception(string.Format("There was a duplicate sprite ID. ID:{0}", sprite.ID));
                }
                spriteIDs.Add(sprite.ID);
            }

            //Checks if any sprites need to be removed
            for (int i = 0; i < Sprites.Count; i++)
            {
                if (Sprites[i].toRemove == true)
                {
                    Sprites.RemoveAt(i);
                    i--;
                }
            }
        }

        protected override void DrawObject(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Orders the sprites by their draw layer
            Sprites = Sprites.OrderBy(x => x.DrawLayer).ToList();

            foreach (Sprite sprite in Sprites)
            {
                sprite.Draw(gameTime, spriteBatch);
            }
        }
    }
}
