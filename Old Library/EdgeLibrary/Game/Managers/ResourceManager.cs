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
    public class ResourceManager
    {
        private static Dictionary<string, Texture2D> textures;
        private static Dictionary<string, SpriteFont> fonts;
        private static GraphicsDevice graphicsDevice;
        private static ContentManager Content;

        public static string ContentRootDirectory;

        public static void Init(ContentManager c)
        {
            Content = c;
            ContentRootDirectory = c.RootDirectory;
            textures = new Dictionary<string, Texture2D>();
            fonts = new Dictionary<string, SpriteFont>();
        }

        #region LOAD
        public static void LoadTexturesInSpritesheet(string xmlPath, string spriteSheetLocation)
        {
            foreach (var kvp in TextureTools.SplitSpritesheet(spriteSheetLocation, xmlPath))
            {
                addTexture(kvp.Key, kvp.Value);
            }
        }
        public static void LoadTexture(string path)
        {
            addTexture(MathTools.LastPortionOfPath(path), Content.Load<Texture2D>(path));
        }

        public static void LoadTexture(string path, string name)
        {
            addTexture(name, Content.Load<Texture2D>(path));
        }

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
        public static Texture2D textureFromString(string texturePath)
        {
                return Content.Load<Texture2D>(texturePath);
        }
        public static void addTexture(string textureName, Texture2D texture)
        {
            textures.Add(textureName, texture);
            DebugLogger.LogAdd("Texture { Name:" + textureName + " }");
        }

        public static void addFont(string fontName, SpriteFont font)
        {
            fonts.Add(fontName, font);
            DebugLogger.LogAdd("Font { Name:" + fontName + " }");
        }

        public static Texture2D getTexture(string textureName)
        {
            foreach (var texture in textures)
            {
                if (texture.Key == textureName)
                {
                    return texture.Value;
                }
            }
            return null;
        }
        public static SpriteFont getFont(string fontName)
        {
            foreach (var font in fonts)
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
