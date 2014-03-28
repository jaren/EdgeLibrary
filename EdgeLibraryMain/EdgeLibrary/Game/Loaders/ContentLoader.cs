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
    /// Stores all the textures and fonts for an EdgeGame
    /// </summary>
    public class ContentLoader
    {
        private Dictionary<string, Texture2D> Textures;
        private Dictionary<string, SpriteFont> Fonts;
        private ContentManager Content;

        public ContentLoader(ContentManager c)
        {
            Content = c;
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();
        }

        #region LOAD
        /* TOINCLUDE
        public void LoadTexturesInSpritesheet(string xmlPath, string spriteSheetLocation)
        {
            foreach (var kvp in TextureTools.SplitSpritesheet(spriteSheetLocation, xmlPath))
            {
                addTexture(kvp.Key, kvp.Value);
            }
        }
         */
        public void LoadTexture(string path)
        {
            addTexture(MathTools.LastPortionOfPath(path), Content.Load<Texture2D>(path));
        }

        public void LoadTexture(string path, string name)
        {
            addTexture(name, Content.Load<Texture2D>(path));
        }

        public void LoadFont(string path)
        {
            addFont(MathTools.LastPortionOfPath(path), Content.Load<SpriteFont>(path));
        }

        public void LoadFont(string path, string name)
        {
            addFont(name, Content.Load<SpriteFont>(path));
        }
        #endregion

        #region OTHER
        public Texture2D TextureFromString(string texturePath)
        {
            return Content.Load<Texture2D>(texturePath);
        }
        public SpriteFont FontFromString(string fontPath)
        {
            return Content.Load<SpriteFont>(fontPath);
        }
        public void addTexture(string textureName, Texture2D texture)
        {
            Textures.Add(textureName, texture);
        }

        public void addFont(string fontName, SpriteFont font)
        {
            Fonts.Add(fontName, font);
        }

        public Texture2D GetTexture(string textureName)
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
        public SpriteFont GetFont(string fontName)
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
