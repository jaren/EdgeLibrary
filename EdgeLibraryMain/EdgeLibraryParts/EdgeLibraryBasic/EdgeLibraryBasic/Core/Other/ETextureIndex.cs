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

namespace EdgeLibrary.Basic
{
    //Base for animations
    public class ETextureIndex : EObject
    {
        //Unfinished
        public List<string> TextureData;
        public List<Texture2D> Textures;

        public ETextureIndex()
        {
            TextureData = new List<string>();
            Textures = new List<Texture2D>();
        }

        public virtual void FillTexture(EData eData)
        {
            try
            {
                Textures.Clear();

                foreach (string textureData in TextureData)
                {
                    Textures.Add(eData.getTexture(textureData));
                }
            }
            catch 
            { }
        }

        public virtual Texture2D Update(EUpdateArgs updateArgs) { return Textures[0]; }
    }

    //Advantages of this - loopRate can be specified between frames
    //Disadvantages - textures must be loaded individually
    public class EAnimationIndex : ETextureIndex
    {
        public List<int> TextureTimes;
        public int currentTexture;
        protected float elapsedSinceLastSwitch;
        public bool HasRunThrough { get; protected set; }
        public bool ShouldRepeat;

        public EAnimationIndex() : base()
        {
            TextureTimes = new List<int>();
            elapsedSinceLastSwitch = 0;
            currentTexture = 0;
            HasRunThrough = false;
            ShouldRepeat = true;
        }

        public EAnimationIndex(int loopRate, List<string> textures) : this()
        {
            for (int i = 0; i < textures.Count; i++)
            {
                TextureData.Add(textures[i]);
                TextureTimes.Add(loopRate);
            }
        }

        public EAnimationIndex(int loopRate, params string[] textures) : this(loopRate, new List<string>(textures))
        {
        }

        public virtual Rectangle getTextureBox()
        {
            return new Rectangle(0, 0, Textures[currentTexture].Width, Textures[currentTexture].Height);
        }

        public void Reset()
        {
            HasRunThrough = false;
            currentTexture = 0;
        }

        public override Texture2D Update(EUpdateArgs updateArgs)
        {
            if (!HasRunThrough || ShouldRepeat)
            {
                elapsedSinceLastSwitch += updateArgs.gameTime.ElapsedGameTime.Milliseconds;

                if (elapsedSinceLastSwitch >= TextureTimes[currentTexture])
                {
                    if (currentTexture >= Textures.Count - 1)
                    {
                        currentTexture = 0;
                        HasRunThrough = true;
                    }
                    else
                    {
                        currentTexture++;
                    }

                    elapsedSinceLastSwitch = 0;
                }
            }

            return Textures[currentTexture];
        }
    }

    //Advantages of this - animations can be loaded from a single spritesheet
    //Disadvantages - no specifying loopRate for different frames
    public class ESpriteSheetAnimationIndex : EAnimationIndex
    {
        public Texture2D SpriteSheet;
        public string textureData;
        public int TextureWidth;
        public int TextureHeight;
        public int FinishTexture;
        public int LoopRate;

        private int TextureRows;
        private int TextureColumns;

        private int CurrentRow;
        private int CurrentColumn;

        private Rectangle textureBox;

        public ESpriteSheetAnimationIndex() : base()
        {
            TextureWidth = 0;
            TextureHeight = 0;
            TextureRows = 1;
            TextureColumns = 1;
            resetTexturePosition();
        }

        public ESpriteSheetAnimationIndex(int loopRate, string spriteSheet, int textureWidth, int textureHeight, int finishTextureNumber) : this()
        {
            FinishTexture = finishTextureNumber;
            LoopRate = loopRate;
            TextureWidth = textureWidth;
            TextureHeight = textureHeight;
            textureData = spriteSheet;
        }

        public override void FillTexture(EData eData)
        {
            try
            {
                SpriteSheet = eData.getTexture(textureData);
                TextureColumns = ((SpriteSheet.Width-(SpriteSheet.Width % TextureWidth)) / TextureWidth);
                TextureRows = ((SpriteSheet.Height-(SpriteSheet.Height % TextureHeight)) / TextureHeight);

                if (FinishTexture > (TextureRows * TextureColumns))
                {
                    FinishTexture = TextureRows * TextureColumns;
                }
            }
            catch 
            { }
        }

        private void addPositionOfTexture()
        {
            CurrentColumn++;

            if (CurrentColumn > TextureColumns)
            {
                CurrentColumn = 1;
                CurrentRow++;
            }

            //This should never be called
            if (CurrentRow > TextureRows && CurrentColumn > TextureColumns)
            {
                resetTexturePosition();
                currentTexture = 0;
                HasRunThrough = true;
            }
        }

        private void resetTexturePosition()
        {
            CurrentRow = 1;
            CurrentColumn = 1;
        }

        private void reloadTextureBox()
        {
            textureBox = new Rectangle(((CurrentColumn - 1) * TextureWidth) + 1, CurrentRow*TextureHeight, TextureWidth, TextureHeight);
        }

        public override Rectangle getTextureBox()
        {
            return textureBox;
        }

        public override Texture2D Update(EUpdateArgs updateArgs)
        {
            if (!HasRunThrough || ShouldRepeat)
            {
                elapsedSinceLastSwitch += updateArgs.gameTime.ElapsedGameTime.Milliseconds;

                if (elapsedSinceLastSwitch >= LoopRate)
                {
                    if (currentTexture >= FinishTexture)
                    {
                        currentTexture = 0;
                        HasRunThrough = true;
                        resetTexturePosition();
                    }
                    else
                    {
                        currentTexture++;
                        addPositionOfTexture();
                    }

                    reloadTextureBox();
                    elapsedSinceLastSwitch = 0;
                }
            }

            return SpriteSheet;
        }
    }

}
