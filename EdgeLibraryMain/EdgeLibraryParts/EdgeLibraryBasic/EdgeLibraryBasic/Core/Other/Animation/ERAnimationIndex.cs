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

namespace EdgeLibrary.Basic
{
    //To be used with a spritesheet and an xml document showing the position of textures on that spritesheet
    //Normally used with "Shoebox" spritesheets - these spritesheets have the same rectangle system as XNA
    //Put XML documents in content
    public class ERAnimationIndex : EAnimationBase
    {
        public Texture2D SpriteSheet;
        public XDocument TexturePositions;

        public int StartTexture { get { return _startTexture; } set { _startTexture = value; recalculateStartFinishTextures(); } }
        public int FinishTexture { get { return _finishTexture; } set { _finishTexture = value; recalculateStartFinishTextures(); } }
        private int _finishTexture;
        private int _startTexture;
        public int LoopRate;
        public string SpriteSheetData;

        public ERAnimationIndex(int loopRate, string spriteSheet, string xmlPath) : base()
        {
            LoopRate = loopRate;
            string completePath = string.Format("{0}\\{1}", EMath.ContentRootDirectory, xmlPath);
            TexturePositions = XDocument.Load(completePath);
            SpriteSheetData = spriteSheet;
            _finishTexture = TexturePositions.Root.Elements().Count();
        }

        public override void Reset()
        {
            HasRunThrough = false;
            currentTexture = _startTexture;
        }

        public override void FillTexture(EData eData)
        {
            try
            {
                SpriteSheet = eData.getTexture(SpriteSheetData);
                recalculateStartFinishTextures();
            }
            catch
            { }
        }

        private void recalculateStartFinishTextures()
        {
            if (_finishTexture > TexturePositions.Root.Elements().Count())
            {
                _finishTexture = TexturePositions.Root.Elements().Count();
            }

            if (_startTexture < 1)
            {
                _startTexture = 1;
            }

            if (_startTexture > _finishTexture)
            {
                int temp = _startTexture;
                _startTexture = _finishTexture;
                _finishTexture = temp;
            }
        }

        public override Rectangle getTextureBox()
        {
            return getRectangleOfXmlElement(currentTexture);
        }

        private Rectangle getRectangleOfXmlElement(int index)
        {
            XElement element = new List<XElement>(TexturePositions.Root.Elements())[index];
           return new Rectangle(int.Parse(element.Attribute("x").Value), int.Parse(element.Attribute("y").Value), int.Parse(element.Attribute("width").Value), int.Parse(element.Attribute("height").Value));
        }
        
        public override Texture2D Update(EUpdateArgs updateArgs)
        {
            if (!HasRunThrough || ShouldRepeat)
            {
                elapsedSinceLastSwitch += updateArgs.gameTime.ElapsedGameTime.Milliseconds;

                if (elapsedSinceLastSwitch >= LoopRate)
                {
                    if (currentTexture >= _finishTexture)
                    {
                        currentTexture = _startTexture;
                        HasRunThrough = true;
                    }
                    else
                    {
                        currentTexture++;
                    }

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
