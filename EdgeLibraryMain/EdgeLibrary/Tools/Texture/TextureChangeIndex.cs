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
    /// Decides how a sprite or particle will change textures
    /// </summary>
    public class TextureChangeIndex
    {
        //The colors and times showing how to change the color and at how long
        public List<Texture2D> Textures;
        public List<float> Times;
        public int index;
        protected double elapsedTime;
        public bool HasFinished { get; protected set; }
        public int FrameIncrement = 1;

        public TextureChangeIndex(params KeyValuePair<Texture2D, float>[] pairs)
        {
            Textures = new List<Texture2D>();
            Times = new List<float>();
            index = 0;
            elapsedTime = 0;

            HasFinished = false;

            foreach (KeyValuePair<Texture2D, float> pair in pairs)
            {
                Add(pair.Key, pair.Value);
            }
        }

        public TextureChangeIndex(float time, params Texture2D[] textures)
        {
            Textures = new List<Texture2D>();
            Times = new List<float>();
            index = 0;
            elapsedTime = 0;

            HasFinished = false;

            foreach (Texture2D texture in textures)
            {
                Add(texture, time);
            }
        }

        public static TextureChangeIndex FromXMLSpriteSheet(float time, string xmlPath, string texturePath, int frameIncrement = 1)
        {
            TextureChangeIndex index = new TextureChangeIndex(time) { FrameIncrement = frameIncrement };
            foreach (var kvp in TextureGeneratorTools.SplitSpritesheet(texturePath, xmlPath))
            {
                index.Add(kvp.Value, time);
            }
            return index;
        }


        //Resets the elapsed time
        public void ResetTime()
        {
            elapsedTime = 0;
            index = 0;
            HasFinished = false;
        }

        //Clears the colors and times
        public void Clear()
        {
            Textures.Clear();
            Times.Clear();
        }

        //Set the specific index
        public void Set(int index, Texture2D texture, float time)
        {
            Textures[index] = texture;
            Times[index] = time;
        }

        //Set the time at a specific index
        public void SetTime(int index, float time)
        {
            Times[index] = time;
        }

        //Set the color at a specific index
        public void SetTexture(int index, Texture2D texture)
        {
            Textures[index] = texture;
        }

        //Adds a new texture/time
        public void Add(Texture2D texture, float time)
        {
            Textures.Add(texture);
            Times.Add(time);
        }

        //Removes a certain texture
        public void Remove(Texture2D texture)
        {
            Textures.Remove(texture);
        }

        //Updates the texture index
        public virtual Texture2D Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds * EdgeGame.GameSpeed;

            if (elapsedTime >= Times[index])
            {
                if (index < Textures.Count - 1)
                {
                    index += FrameIncrement;
                }
                elapsedTime = 0;
            }

            if (index >= Textures.Count - 1)
            {
                index = Textures.Count - 1;

                HasFinished = true;
                return Textures[index];
            }
            else
            {
                HasFinished = false;
            }
            return Textures[index];
        }

        public TextureChangeIndex Clone()
        {
            TextureChangeIndex index = new TextureChangeIndex(0) { FrameIncrement = this.FrameIncrement };
            for(int i = 0; i < Textures.Count; i++)
            {
                index.Add(Textures[i], Times[i]);
            }
            return index;
        }
    }
}
