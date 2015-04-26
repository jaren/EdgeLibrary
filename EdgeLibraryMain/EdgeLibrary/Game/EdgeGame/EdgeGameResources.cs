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
    /// Stores all the textures, fonts, and models for EdgeGame
    /// </summary>
    public static partial class EdgeGame
    {
        private static Dictionary<string, Texture2D> Textures;
        private static Dictionary<string, Model> Models;
        private static Dictionary<string, SpriteFont> Fonts;
        private static Dictionary<string, BitmapFont> BitmapFonts;

        public static string ContentRootDirectory { get { return Game.Content.RootDirectory; } set { } }

        private static void InitializeResources()
        {
            Textures = new Dictionary<string, Texture2D>();
            Models = new Dictionary<string, Model>();
            Fonts = new Dictionary<string, SpriteFont>();
            BitmapFonts = new Dictionary<string, BitmapFont>();
        }

        private static void InitializeBasicTextures()
        {
            Texture2D pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            AddTexture("Pixel", pixel);

            Texture2D blank = new Texture2D(Game.GraphicsDevice, 1, 1);
            blank.SetData<Color>(new Color[] { Color.Transparent });
            AddTexture("Blank", blank);
        }

        #region LOAD
        //Loads all the textures in a spritesheet
        public static void LoadTexturesInSpritesheet(string xmlPath, string spriteSheetLocation)
        {
            foreach (var kvp in TextureGeneratorTools.SplitSpritesheet(spriteSheetLocation, xmlPath))
            {
                AddTexture(kvp.Key, kvp.Value);
            }
        }
        //Loads a texture
        public static void LoadTexture(string path)
        {
            AddTexture(path.LastSplit('/'), Game.Content.Load<Texture2D>(path));
        }
        public static void LoadTexture(string path, string name)
        {
            AddTexture(name, Game.Content.Load<Texture2D>(path));
        }

        //Loads a font
        public static void LoadFont(string path)
        {
            AddFont(path.LastSplit('/'), Game.Content.Load<SpriteFont>(path));
        }
        public static void LoadFont(string path, string name)
        {
            AddFont(name, Game.Content.Load<SpriteFont>(path));
        }

        //Loads a bitmap font
        public static void LoadBitmapFont(string xmlPath, string imagePath)
        {
            AddBitmapFont(imagePath.LastSplit('/'), new BitmapFont(xmlPath, Game.Content.Load<Texture2D>(imagePath)));
        }
        public static void LoadBitmapFont(string xmlPath, string imagePath, string name)
        {
            AddBitmapFont(name, new BitmapFont(xmlPath, Game.Content.Load<Texture2D>(imagePath)));
        }

        //Loads a model
        public static void LoadModel(string path)
        {
            AddModel(path.LastSplit('/'), Game.Content.Load<Model>(path));
        }
        public static void LoadModel(string path, string name)
        {
            AddModel(name, Game.Content.Load<Model>(path));
        }
        #endregion

        #region OTHER
        //Gets a texture from Content.Load() with the given path
        public static Texture2D TextureFromString(string texturePath)
        {
            return Game.Content.Load<Texture2D>(texturePath);
        }
        //Gets a font from Content.Load() with the given path
        public static SpriteFont FontFromString(string fontPath)
        {
            return Game.Content.Load<SpriteFont>(fontPath);
        }
        //Gets a model from Content.Load() with the given path
        public static Model ModelFromString(string modelPath)
        {
            return Game.Content.Load<Model>(modelPath);
        }

        //Adds an already-generated texture to the index
        public static void AddTexture(string textureName, Texture2D texture)
        {
            Textures.Add(textureName, texture);
        }
        //Adds an already-generated font to the index
        public static void AddFont(string fontName, SpriteFont font)
        {
            Fonts.Add(fontName, font);
        }
        //Adds an already-generated bitmap font to the index
        public static void AddBitmapFont(string fontName, BitmapFont font)
        {
            BitmapFonts.Add(fontName, font);
        }
        //Adds an already-generated model to the index
        public static void AddModel(string modelName, Model model)
        {
            Models.Add(modelName, model);
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
        public static BitmapFont GetBitmapFont(string fontName)
        {
            foreach (var font in BitmapFonts)
            {
                if (font.Key == fontName)
                {
                    return font.Value;
                }
            }
            return null;
        }
        public static Model GetModel(string modelName)
        {
            foreach (var model in Models)
            {
                if (model.Key == modelName)
                {
                    return model.Value;
                }
            }
            return null;
        }
        #endregion
    }
}
