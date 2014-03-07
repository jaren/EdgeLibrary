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
    public struct SpriteStyle
    {
        public SpriteEffects Effects;
        public float Rotation { get { return _rotation; } set { _rotation = value; reloadRotation(); } }
        private float _rotation;
        public Color Color;

        public SpriteStyle(SpriteEffects effects, float rotation, Color color)
        {
            Effects = effects;
            _rotation = rotation;
            Color = color;
        }

        private void reloadRotation()
        {
            if (_rotation > 360)
            {
                _rotation %= 360;
            }
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

        public override Vector2 Position { get; set; }
        public float Width { get { return _width; } set { _width = value; reloadOriginPoint(); } }
        public float Height { get { return _height; } set { _height = value; reloadOriginPoint(); } }
        public Vector2 Scale { get { return _scale; } set { _scale = value; reloadActualScale(); } }
        public Effect TextureEffect;
        protected Vector2 originPoint { get; set; }
        public virtual CollisionBody CollisionBody { get; set; }
        public virtual ShapeTypes CollisionBodyType { get; set; }
        public Texture2D Texture { get { return _texture; } set { _texture = value; reloadDimensions(); } }
        public StyleCapability StyleChanger;
        private Texture2D _texture;
        public SpriteStyle Style;
        protected Vector2 actualScale;
        protected Vector2 _scale;
        protected float _width;
        protected float _height;

        protected List<string> currentlyCollidingWithIDs;

        public delegate void CollisionEvent(CollisionEventArgs e);
        public event CollisionEvent CollisionStart;
        public event CollisionEvent Collision;

        public Sprite(string eTextureName, Vector2 ePosition) : this(MathTools.RandomID(), eTextureName, ePosition) { }

        public Sprite(string id, string eTextureName, Vector2 ePosition) : base(id)
        {
            Style = new SpriteStyle(SpriteEffects.None, 0f, Color.White);
            Position = ePosition;
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

                reloadOriginPoint();
            }

            CollisionBody = CollisionBody.BodyWithSprite(ShapeTypes.rectangle, this, ID);
            CollisionBodyType = CollisionBody.Shape.ShapeType;
            CollisionBody.collidesWithAll = true;
        }

        public Sprite(string id, string eTextureName, Vector2 ePosition, int eWidth, int eHeight) : this(id, eTextureName, ePosition)
        {
            _width = eWidth;
            _height = eHeight;
            reloadDimensions();
        }

        public Sprite(string id, string eTextureName, Vector2 ePosition, int eWidth, int eHeight, Color eColor, float eRotation, Vector2 eScale) : this(id, eTextureName, ePosition, eWidth, eHeight)
        {
            Style.Color = eColor;
            Style.Rotation = eRotation;
            Scale = eScale;
        }

        public virtual Rectangle GetBoundingBox()
        {
            return new Rectangle((int)(Position.X - _width / 2), (int)(Position.Y - _height / 2), (int)_width, (int)_height);
        }

        public virtual void reloadDimensions()
        {
            if (Texture != null)
            {
                _width = Texture.Width;
                _height = Texture.Height;
                reloadOriginPoint();
            }
        }

        protected virtual void reloadOriginPoint()
        {
            if (Texture != null)
            {
                originPoint = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
                reloadActualScale();
            }
        }

        protected virtual void reloadActualScale()
        {
            if (Texture != null)
            {
                actualScale = new Vector2(_width / Texture.Width, _height / Texture.Height);
                actualScale *= Scale;
            }
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
            if (TextureEffect != null)
            {
                TextureEffect.ApplyEffect(Texture);
            }

            EdgeGame.drawTexture(Texture, Position, null, Style.Color, actualScale, Style.Rotation, originPoint, Style.Effects);
        }

        public virtual void DebugDraw(Color color)
        {
            CollisionBody.Shape.DebugDraw(color);
        }
    }
}
