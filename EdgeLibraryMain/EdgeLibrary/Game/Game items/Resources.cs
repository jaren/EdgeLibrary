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
    /// <summary>
    /// Stores all the textures and fonts - this is global
    /// </summary>
    public static class Resources
    {
        private static Dictionary<string, Texture2D> Textures;
        private static Dictionary<string, SpriteFont> Fonts;
        private static ContentManager Content;

        public static string ContentRootDirectory { get { return Content.RootDirectory; } set { } }

        public static void Init(ContentManager c)
        {
            Content = c;
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();

            Texture2D pixel = new Texture2D(EdgeGame.Instance.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[]{Color.White});
            addTexture("Pixel", pixel);

            Texture2D blank = new Texture2D(EdgeGame.Instance.GraphicsDevice, 1, 1);
            blank.SetData<Color>(new Color[] { Color.Transparent });
            addTexture("Blank", blank);
        }

        #region LOAD
        //Loads all the textures in a spritesheet
        public static void LoadTexturesInSpritesheet(string xmlPath, string spriteSheetLocation)
        {
            foreach (var kvp in TextureTools.SplitSpritesheet(spriteSheetLocation, xmlPath))
            {
                addTexture(kvp.Key, kvp.Value);
            }
        }
        //Loads a texture
        public static void LoadTexture(string path)
        {
            addTexture(MathTools.LastPortionOfPath(path), Content.Load<Texture2D>(path));
        }
        public static void LoadTexture(string path, string name)
        {
            addTexture(name, Content.Load<Texture2D>(path));
        }

        //Loads a font
        public static void LoadFont(string path)
        {
            addFont(MathTools.LastPortionOfPath(path), Content.Load<SpriteFont>(path));
        }
        public static void LoadFont(string path, string name)
        {
            addFont(name, Content.Load<SpriteFont>(path));
        }
        #endregion

        #region OTHER
        //Gets a texture from Content.Load() with the given path
        public static Texture2D TextureFromString(string texturePath)
        {
            return Content.Load<Texture2D>(texturePath);
        }
        //Gets a font from Content.Load() with the given path
        public static SpriteFont FontFromString(string fontPath)
        {
            return Content.Load<SpriteFont>(fontPath);
        }
        //Adds an already-generated texture to the index
        public static void addTexture(string textureName, Texture2D texture)
        {
            Textures.Add(textureName, texture);
        }
        //Adds an already-generated font to the index
        public static void addFont(string fontName, SpriteFont font)
        {
            Fonts.Add(fontName, font);
        }

        public static Texture2D GetTexture(string textureName)
        {
            foreach (var texture in Textures)
            {
                if (texture.Key == textureName)
                {
                    return texture.Value;
                }
            }
            return null;
        }
        public static SpriteFont GetFont(string fontName)
        {
            foreach (var font in Fonts)
            {
                if (font.Key == fontName)
                {
                    return font.Value;
                }
            }
            return null;
        }
        #endregion
    }
}
