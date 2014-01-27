﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EdgeLibrary.Basic
{
    //Just a label - can have actions run on it thanks to a sprite inside it
    public class ELabel : EElement
    {
        //Font name is stored in "Data"

        public string Text;
        public Vector2 Scale { get { return _scale; } set { sprite.Scale = value; loadFromSprite(); } }
        public override Vector2 Position { get { return _position; } set { sprite.Position = value; loadFromSprite(); } }
        public Color Color { get { return _color; } set { sprite.Color = value; loadFromSprite(); } }
        public float Rotation { get { return _rotation; } set { sprite.Rotation = value; loadFromSprite(); } }
        public SpriteEffects spriteEffects;

        //Used for actions
        protected Vector2 _scale;
        protected Vector2 _position;
        protected Color _color;
        protected float _rotation;

        //Used for running actions
        protected ESprite sprite;

        public ELabel(string eFontName, Vector2 ePosition, string eText, Color eColor) : base()
        {
            sprite = new ESprite("", Vector2.Zero, 0, 0);
            Data = eFontName;
            spriteEffects = SpriteEffects.None;
            sprite.Position = ePosition;
            Text = eText;
            Color = eColor;
        }

        public override void FillTexture()
        {
            Font = EData.getFont(Data);
        }

        public void CenterX()
        {
            Position = new Vector2(EMath.CenterXString(this), Position.Y);
        }

        public void runAction(EAction action)
        {
            sprite.runAction(action);
        }
        
        public void setFont(SpriteFont eFont)
        {
            Font = eFont;
        }

        public void loadFromSprite()
        {
            _position = sprite.Position;
            _rotation = sprite.Rotation;
            _color = sprite.Color;
            _scale = sprite.Scale;
        }

        public override void updateElement(EUpdateArgs updateArgs)
        {
            sprite.Update(updateArgs);
            loadFromSprite();
        }

        public override void drawElement(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawStringToSpriteBatch(spriteBatch, Font, Text, Color, Rotation, Scale, spriteEffects);
        }
    }
}
