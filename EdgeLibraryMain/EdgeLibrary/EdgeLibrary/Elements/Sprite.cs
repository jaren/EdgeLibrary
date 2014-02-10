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
    public enum SpriteDrawType
    {
        NoRatio,
        KeepWidth,
        KeepHeight,
        Scaled
    }

    //Provides a base textured game object
    public class Sprite : Element
    {
        public Rectangle BoundingBox { get; set; }
        public override Vector2 Position { get { return _position; } set { _position = value; reloadBoundingBox(); } }
        public float Width { get { return _width; } set { _width = value; reloadBoundingBox(); } }
        public float Height { get { return _height; } set { _height = value; reloadBoundingBox(); } }
        public Vector2 Scale { get { return _scale; } set { _scale = value; reloadBoundingBox(); } }
        public Texture2D Texture { get; set; }
        public SpriteEffects spriteEffects;
        public SpriteDrawType DrawType;
        public float ScaledDrawScale;
        protected Vector2 _position;
        protected float _width;
        protected float _height;
        protected Vector2 _scale;

        //Extra
        public float Rotation;
        public Color Color;

        public Sprite(string eTextureName, Vector2 ePosition): base(true)
        {
            DrawType = SpriteDrawType.NoRatio;
            spriteEffects = SpriteEffects.None;
            ScaledDrawScale = 1f;
            _position = ePosition;
            _width = 0;
            _height = 0;

            Color = Color.White;
            Rotation = 0;
            Scale = Vector2.One;


            if (eTextureName != null)
            {
                Texture = ResourceManager.getTexture(eTextureName);
            }
            if (_width == 0)
            {
                _width = Texture.Width;
            }
            if (_height == 0)
            {
                _height = Texture.Height;
            }

            reloadBoundingBox();
        }

        public Sprite(string eTextureName, Vector2 ePosition, int eWidth, int eHeight) : this(eTextureName, ePosition)
        {
            _width = eWidth;
            _height = eHeight;
            reloadBoundingBox();
        }

        public Sprite(string eTextureName, Vector2 ePosition, int eWidth, int eHeight, Color eColor, float eRotation, Vector2 eScale) : this(eTextureName, ePosition, eWidth, eHeight)
        {
            Color = eColor;
            Rotation = eRotation;
            Scale = eScale;
        }

        public void reloadBoundingBox()
        {
            BoundingBox = new Rectangle((int)_position.X - ((int)_width / 2 * (int)Scale.X), (int)_position.Y - ((int)_height / 2 * (int)Scale.Y), (int)_width * (int)Scale.X, (int)_height * (int)Scale.Y);
        }

        public override void drawElement(GameTime gameTime)
        {
            switch (DrawType)
            {
                case SpriteDrawType.NoRatio:
                    base.DrawTexture(null, Texture, BoundingBox, Color, Rotation, spriteEffects);
                    break;
                case SpriteDrawType.KeepHeight:
                    base.DrawWithHeight(null, Texture, Height, Color, Rotation, spriteEffects);
                    break;
                case SpriteDrawType.KeepWidth:
                    base.DrawWithWidth(null, Texture, Width, Color, Rotation, spriteEffects);
                    break;
                case SpriteDrawType.Scaled:
                    base.DrawWithScale(null, Texture, ScaledDrawScale, Color, Rotation, spriteEffects);
                    break;
            }
        }
    }
}
