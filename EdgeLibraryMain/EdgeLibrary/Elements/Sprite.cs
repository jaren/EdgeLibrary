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
        KeepHeight
    }

    public struct SpriteStyle
    {
        public SpriteEffects Effects;
        public SpriteDrawType DrawType;
        public float Rotation;
        public Color Color;

        public SpriteStyle(SpriteEffects effects, SpriteDrawType drawType, float rotation, Color color)
        {
            Effects = effects;
            DrawType = drawType;
            Rotation = rotation;
            Color = color;
        }
    }

    public class CollisionEventArgs : EventArgs
    {
        public Sprite Sprite1;
        public Sprite Sprite2;

        public CollisionEventArgs(Sprite sprite1, Sprite sprite2)
        {
            Sprite1 = sprite1;
            Sprite2 = sprite2;
        }
    }

    //Provides a base textured game object
    public class Sprite : Element
    {
        public Rectangle BoundingBox { get; set; }
        public override Vector2 Position { get { return _position; } set { _position = value; reloadBoundingBox(); } }
        public float Width { get { return _width; } set { _width = value; reloadBoundingBox(); } }
        public float Height { get { return _height; } set { _height = value; reloadBoundingBox(); } }
        public Vector2 Scale { get { return _scale; } set { _scale = value; reloadBoundingBox(); } }
        public virtual CollisionBody CollisionBody { get; set; }
        public virtual ShapeTypes CollisionBodyType { get; set; }
        public Texture2D Texture { get { return _texture; } set { _texture = value; reloadDimensions(); } }
        public StyleCapability StyleChanger;
        private Texture2D _texture;
        public SpriteStyle Style;
        protected Vector2 _position;
        protected float _width;
        protected float _height;
        protected Vector2 _scale;

        protected List<string> currentlyCollidingWithIDs;

        public delegate void CollisionEvent(CollisionEventArgs e);
        public event CollisionEvent CollisionStart;
        public event CollisionEvent Collision;

        public Sprite(string eTextureName, Vector2 ePosition) : this(MathTools.RandomID(), eTextureName, ePosition) { }

        public Sprite(string id, string eTextureName, Vector2 ePosition) : base(id)
        {
            Style = new SpriteStyle(SpriteEffects.None, SpriteDrawType.NoRatio, 0f, Color.White);
            _position = ePosition;
            _width = 0;
            _height = 0;

            Scale = Vector2.One;

            StyleChanger = new StyleCapability();
            AddCapability(StyleChanger);

            currentlyCollidingWithIDs = new List<string>();


            if (eTextureName != null)
            {
                Texture = ResourceManager.getTexture(eTextureName);
            }
            if (Texture != null)
            {
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

            CollisionBody = CollisionBody.BodyWithSprite(ShapeTypes.rectangle, this, ID);
            CollisionBodyType = CollisionBody.Shape.ShapeType;
            CollisionBody.collidesWithAll = true;
        }

        public Sprite(string id, string eTextureName, Vector2 ePosition, int eWidth, int eHeight) : this(id, eTextureName, ePosition)
        {
            _width = eWidth;
            _height = eHeight;
            reloadBoundingBox();
        }

        public Sprite(string id, string eTextureName, Vector2 ePosition, int eWidth, int eHeight, Color eColor, float eRotation, Vector2 eScale) : this(id, eTextureName, ePosition, eWidth, eHeight)
        {
            Style.Color = eColor;
            Style.Rotation = eRotation;
            Scale = eScale;
        }

        public virtual void reloadDimensions()
        {
            if (Texture != null)
            {
                _width = Texture.Width;
                _height = Texture.Height;
                reloadBoundingBox();
            }
        }

        public virtual void reloadBoundingBox()
        {
            BoundingBox = new Rectangle((int)_position.X - ((int)_width / 2 * (int)Scale.X), (int)_position.Y - ((int)_height / 2 * (int)Scale.Y), (int)_width * (int)Scale.X, (int)_height * (int)Scale.Y);
            reloadOriginPoint();
        }

        protected void reloadOriginPoint()
        {
            OriginPoint = new Vector2(BoundingBox.Width / 2, BoundingBox.Height / 2);
            BoundingBox = new Rectangle(BoundingBox.X + (int)OriginPoint.X, BoundingBox.Y + (int)OriginPoint.Y, BoundingBox.Width, BoundingBox.Height);
        }

        protected override void updateElement(GameTime gameTime)
        {
            UpdateCollision();
            base.updateElement(gameTime);
        }

        protected virtual void UpdateCollision()
        {
            if (CollisionBody != null)
            {
                CollisionBody.Position = new Vector2(Position.X, Position.Y);
                CollisionBody.ScaleWith(this, CollisionBodyType);

                foreach (Element element in EdgeGame.SelectedScene.elements)
                {
                    if (element is Sprite)
                    {
                        if (!(element is TextSprite) || EdgeGame.CollisionsInTextSprites)
                        {
                            Sprite elementAsSprite = (Sprite)element;

                            if (elementAsSprite != this && elementAsSprite.CollisionBody != null && (Collision != null || CollisionStart != null))
                            {
                                if (CollisionBody.CheckForCollide(elementAsSprite.CollisionBody))
                                {
                                    if (Collision != null) { Collision(new CollisionEventArgs(this, elementAsSprite)); }
                                    if (CollisionStart != null && !currentlyCollidingWithIDs.Contains(elementAsSprite.CollisionBody.ID))
                                    {
                                        CollisionStart(new CollisionEventArgs(this, elementAsSprite));
                                        currentlyCollidingWithIDs.Add(elementAsSprite.CollisionBody.ID);
                                    }
                                }
                                //Checks if it's not colliding with the element, then removes it from the colliding list
                                else if (currentlyCollidingWithIDs.Contains(elementAsSprite.CollisionBody.ID))
                                {
                                    currentlyCollidingWithIDs.Remove(elementAsSprite.CollisionBody.ID);
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void drawElement(GameTime gameTime)
        {
            switch (Style.DrawType)
            {
                case SpriteDrawType.NoRatio:
                    base.DrawTexture(null, Texture, BoundingBox, Style.Color, Style.Rotation, Style.Effects);
                    break;
                case SpriteDrawType.KeepHeight:
                    base.DrawWithHeight(null, Texture, Height, Style.Color, Style.Rotation, Style.Effects);
                    break;
                case SpriteDrawType.KeepWidth:
                    base.DrawWithWidth(null, Texture, Width, Style.Color, Style.Rotation, Style.Effects);
                    break;
            }
        }
    }
}
