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
    public class PlatformLevel : Scene
    {
        public List<PlatformSprite> sprites;
        public Vector2 Gravity { get; set; }

        public PlatformLevel(string id, Vector2 gravity) : base(id)
        {
            sprites = new List<PlatformSprite>();
            Gravity = gravity;
        }

        public static PlatformLevel LevelFromTexture(string texturePath, string id, Vector2 gravity)
        {
            PlatformLevel level = new PlatformLevel(id, gravity);

            Texture2D texture = ResourceManager.textureFromString(texturePath);
            Color[] colors = new Color[texture.Width*texture.Height];
            texture.GetData<Color>(colors);

            Dictionary<Rectangle, Color> levelBoxes = new Dictionary<Rectangle,Color>();

            for (int i = 0; i < colors.Length; i++)
            {
                //Unfinished
            }
            return level;
        }

        public void CreateScreenBox()
        {
            PlatformStatic top = new PlatformStatic(string.Format("{0}_topBox", ID), "Pixel", new Vector2(EdgeGame.WindowSize.X/2, -100));
            top.Width = EdgeGame.WindowSize.X;
            top.Height = 200;
            top.Style.Color = Color.White;
            sprites.Add(top);

            PlatformStatic bottom = new PlatformStatic(string.Format("{0}_bottomBox", ID), "Pixel", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y + 100));
            bottom.Width = EdgeGame.WindowSize.X;
            bottom.Height = 200;
            bottom.Style.Color = Color.White;
            sprites.Add(bottom);

            PlatformStatic right = new PlatformStatic(string.Format("{0}_rightBox", ID), "Pixel", new Vector2(-100, EdgeGame.WindowSize.Y/2));
            right.Height = EdgeGame.WindowSize.Y;
            right.Width = 200;
            right.Style.Color = Color.White;
            sprites.Add(right);

            PlatformStatic left = new PlatformStatic(string.Format("{0}_leftBox", ID), "Pixel", new Vector2(EdgeGame.WindowSize.X + 100, EdgeGame.WindowSize.Y / 2));
            left.Height = EdgeGame.WindowSize.Y;
            left.Width = 200;
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

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].UpdatePlatform(gameTime, Gravity, sprites);
                sprites[i].Update(gameTime);

                if (sprites[i].MarkedForPlatformRemoval)
                {
                    sprites.RemoveAt(i);
                    i--;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            sprites = sprites.OrderBy(X => X.DrawLayer).ToList();

            foreach (PlatformSprite sprite in sprites)
            {
                sprite.Draw(gameTime);
            }
        }

        public override void DrawDebug(GameTime gameTime)
        {
            foreach (PlatformSprite sprite in sprites)
            {
                sprite.DebugDraw(EdgeGame.DebugDrawColor);
            }
        }
    }
}
