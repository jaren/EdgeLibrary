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
using EdgeLibrary.Basic;

namespace EdgeLibrary.Unsupported
{
    //An animation index which supports spritesheets
    //Advantages of this - animations can be loaded from a single spritesheet
    //Disadvantages - no specifying loopRate for different frames
    public class ESAnimationIndex : EAnimationBase
    {
        public Texture2D SpriteSheet;
        public string texturEData;
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

        public ESAnimationIndex()
            : base()
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

        public ESAnimationIndex(int loopRate, string spriteSheet, int textureWidth, int textureHeight)
            : this()
        {
            LoopRate = loopRate;
            TextureWidth = textureWidth;
            TextureHeight = textureHeight;
            texturEData = spriteSheet;
        }

        public override void FillTexture()
        {
            try
            {
                SpriteSheet = EData.getTexture(texturEData);
                TextureColumns = ((SpriteSheet.Width - (SpriteSheet.Width % TextureWidth)) / TextureWidth);
                TextureRows = ((SpriteSheet.Height - (SpriteSheet.Height % TextureHeight)) / TextureHeight);

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
            textureBox = new Rectangle((CurrentColumn - 1) * TextureWidth, (CurrentRow - 1) * TextureHeight, TextureWidth, TextureHeight);
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
