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
    public class EAnimationBase : EObject
    {
        //Unfinished
        public List<string> TextureData;
        public List<Texture2D> Textures;
        public bool ShowBlankOnFinish;
        public bool ShouldRepeat;
        public bool HasRunThrough { get; protected set; }
        public int currentTexture;
        protected float elapsedSinceLastSwitch;

        public EAnimationBase()
        {
            TextureData = new List<string>();
            Textures = new List<Texture2D>();
            ShowBlankOnFinish = false;
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

        public virtual void Reset() { }

        public virtual Rectangle getTextureBox() { return new Rectangle(0,0,0,0); }

        public virtual Texture2D Update(EUpdateArgs updateArgs) { return Textures[0]; }
    }

    //An animation index which doesn't support spritesheets
    //Advantages of this - loopRate can be specified between frames
    //Disadvantages - textures must be loaded individually
    public class EAnimationIndex : EAnimationBase
    {
        public List<int> TextureTimes;

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

        public override Rectangle getTextureBox()
        {
            return new Rectangle(0, 0, Textures[currentTexture].Width, Textures[currentTexture].Height);
        }

        public override void Reset()
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

                return Textures[currentTexture];
            }
            else
            {
                if (ShowBlankOnFinish)
                {
                    return EMath.Blank;
                }
                else
                {
                    return Textures[0];
                }
            }
        }
    }

    //An animation index which supports spritesheets
    //Advantages of this - animations can be loaded from a single spritesheet
    //Disadvantages - no specifying loopRate for different frames
    public class ESpriteSheetAnimationIndex : EAnimationBase
    {
        public Texture2D SpriteSheet;
        public string textureData;
        public int TextureWidth;
        public int TextureHeight;
        public int FinishTexture;
        public int StartTexture { get { return _startTexture; } set { _startTexture = value; setStartTextureToCurrent(); } }
        private int _startTexture;
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
            elapsedSinceLastSwitch = 0;
            currentTexture = 1;
            HasRunThrough = false;
            ShouldRepeat = true;
            FinishTexture = 1;
            _startTexture = 1;
            resetTexturePosition();
        }

        public ESpriteSheetAnimationIndex(int loopRate, string spriteSheet, int textureWidth, int textureHeight) : this()
        {
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

                if (FinishTexture == 1 || FinishTexture > TextureRows * TextureColumns)
                {
                    FinishTexture = TextureRows * TextureColumns;
                }

                resetTexturePosition();
            }
            catch 
            { }

            reloadTextureBox();
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
                HasRunThrough = true;
            }
        }

        public override void Reset()
        {
            HasRunThrough = false;
            currentTexture = _startTexture;
            resetTexturePosition();
        }

        private void resetTexturePosition()
        {
            currentTexture = _startTexture;

            CurrentRow = ((_startTexture - (_startTexture % TextureColumns)) / TextureColumns) + 1;
            CurrentColumn = _startTexture % CurrentRow + 1;
        }

        private void setStartTextureToCurrent()
        {
            if (currentTexture < _startTexture)
            {
                currentTexture = _startTexture;
            }
        }

        private void reloadTextureBox()
        {
            textureBox = new Rectangle((CurrentColumn - 1) * TextureWidth, (CurrentRow-1)*TextureHeight, TextureWidth, TextureHeight);
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
                        if (ShouldRepeat)
                        {
                            resetTexturePosition();
                        }
                        HasRunThrough = true;
                    }
                    else
                    {
                        currentTexture++;
                        addPositionOfTexture();
                    }

                    reloadTextureBox();
                    elapsedSinceLastSwitch = 0;
                }

                return SpriteSheet;
            }
            else
            {
                if (ShowBlankOnFinish)
                {
                    return EMath.Blank;
                }
                else
                {
                    return SpriteSheet;
                }
            }
        }
    }

}
