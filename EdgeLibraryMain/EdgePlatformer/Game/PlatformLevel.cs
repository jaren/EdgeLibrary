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
        public Vector2 Gravity { get { return _gravity; } set { _gravity = value; reloadGravity(); } }
        private Vector2 _gravity;

        public PlatformLevel(string id, Vector2 gravity) : base(id)
        {
            _gravity = gravity;
            reloadGravity();
        }

        private void reloadGravity()
        {
            foreach (Element element in elements)
            {
                if (element is PlatformSprite)
                {
                    ((PlatformSprite)element).gravity = _gravity;
                }
            }
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

        public override void AddElement(Element element)
        {
            if (element is PlatformSprite)
            {
                ((PlatformSprite)element).gravity = _gravity;
            }
            base.AddElement(element);
        }

        public void CreateScreenBox()
        {
            PlatformStatic top = new PlatformStatic(string.Format("{0}_topBox", ID), "Pixel", new Vector2(EdgeGame.WindowSize.X/2, -100));
            top.Scale = new Vector2(EdgeGame.WindowSize.X, 100);
            top.Style.Color = Color.White;

            PlatformStatic bottom = new PlatformStatic(string.Format("{0}_bottomBox", ID), "Pixel", new Vector2(EdgeGame.WindowSize.X / 2, EdgeGame.WindowSize.Y + 100));
            bottom.Scale = new Vector2(EdgeGame.WindowSize.X, 100);
            bottom.Style.Color = Color.White;

            PlatformStatic right = new PlatformStatic(string.Format("{0}_rightBox", ID), "Pixel", new Vector2(-100, EdgeGame.WindowSize.Y/2));
            right.Scale = new Vector2(100, EdgeGame.WindowSize.X);
            right.Style.Color = Color.White;

            PlatformStatic left = new PlatformStatic(string.Format("{0}_leftBox", ID), "Pixel", new Vector2(EdgeGame.WindowSize.X + 100, EdgeGame.WindowSize.Y / 2));
            left.Scale = new Vector2(100, EdgeGame.WindowSize.X);
            left.Style.Color = Color.White;
        }
    }
}
