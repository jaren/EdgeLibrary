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

    //Provides a base textured game object
    public class Sprite : Element
    {

        public override Vector2 Position { get; set; }
        public virtual float Width { get { return Texture == null ? 0 : Texture.Width; } protected set { } }
        public virtual float Height { get { return Texture == null ? 0 : Texture.Height; } protected set { } }
        public Vector2 Scale { get; set; }
        public Effect TextureEffect;
        public Vector2 OriginPoint { get; set; }
        //If set to true, origin will be top left; if not, origin will be center
        public bool CenterAsOrigin { get { return _centerAsOrigin; } set { _centerAsOrigin = value; reloadOriginPoint(); } }
        protected bool _centerAsOrigin;
        public virtual CollisionBody CollisionBody { get; set; }
        public virtual ShapeTypes CollisionBodyType { get; set; }
        public Texture2D Texture { get { return _texture; } set { _texture = value; reloadOriginPoint(); } }
        protected Texture2D _texture;
        public StyleCapability StyleChanger;
        public SpriteStyle Style;

        protected List<string> currentlyCollidingWithIDs;

        public delegate void CollisionEvent(Sprite sender, Sprite sprite2, GameTime gameTime);
        public event CollisionEvent CollisionStart;
        public event CollisionEvent Collision;

        public Sprite(string eTextureName, Vector2 ePosition) : this(MathTools.RandomID(typeof(Sprite)), eTextureName, ePosition) { }

        public Sprite(string id, string eTextureName, Vector2 ePosition) : base(id)
        {

            Style = new SpriteStyle(SpriteEffects.None, 0f, Color.White);
            Position = ePosition;

            Scale = Vector2.One;

            _centerAsOrigin = true;

            StyleChanger = new StyleCapability();
            AddCapability(StyleChanger);

            currentlyCollidingWithIDs = new List<string>();

            CollisionBodyType = ShapeTypes.rectangle;
            CollisionBody = CollisionBody.BodyWithSprite(this, CollisionLayers.All);

            if (eTextureName != null)
            {
                Texture = ResourceManager.getTexture(eTextureName);
            }
            reloadOriginPoint();
        }

        public Sprite(string id, string eTextureName, Vector2 ePosition, Color eColor, float eRotation, Vector2 eScale) : this(id, eTextureName, ePosition)
        {
            Style.Color = eColor;
            Style.Rotation = eRotation;
            Scale = eScale;
        }

        public virtual void InitializeCollision()
        {
            CollisionBody = CollisionBody.BodyWithSprite(this, CollisionLayers.All);
        }
        public virtual void InitializeCollision(ShapeTypes shapeType, CollisionLayers layers)
        {
            CollisionBodyType = shapeType;
            CollisionBody = CollisionBody.BodyWithSprite(this, layers);
        }

        public virtual Rectangle GetBoundingBox()
        {
            return new Rectangle((int)(Position.X - Width / 2 * Scale.X), (int)(Position.Y - Height / 2 * Scale.Y), (int)(Width * Scale.X), (int)(Height * Scale.Y));
        }

        protected virtual void reloadOriginPoint()
        {
            if (Texture != null)
            {
                if (_centerAsOrigin)
                {
                    OriginPoint = new Vector2(Width / 2f, Height / 2f);
                }
                else
                {
                    OriginPoint = Vector2.Zero;
                }
            }
        }

        protected override void updateElement(GameTime gameTime)
        {
            UpdateCollision(gameTime);
            base.updateElement(gameTime);
        }

        protected virtual void UpdateCollision(GameTime gameTime)
        {
            if (CollisionBody != null)
            {
                CollisionBody.ScaleWith(this);
                if (_centerAsOrigin)
                {
                    CollisionBody.Position = new Vector2(Position.X, Position.Y);
                }
                else
                {
                    CollisionBody.Position = new Vector2(Position.X + Width / 2, Position.Y + Height / 2);
                }

                foreach (Element element in EdgeGame.SelectedScene.elements)
                {
                    if (element is Sprite)
                    {
                            Sprite elementAsSprite = (Sprite)element;

                            if (elementAsSprite != this && elementAsSprite.CollisionBody != null && (Collision != null || CollisionStart != null))
                            {
                                if (CollisionBody.CheckForCollide(elementAsSprite.CollisionBody))
                                {
                                    if (Collision != null) { Collision(this, elementAsSprite, gameTime); }
                                    if (CollisionStart != null && !currentlyCollidingWithIDs.Contains(elementAsSprite.ID))
                                    {
                                        CollisionStart(this, elementAsSprite, gameTime);
                                        currentlyCollidingWithIDs.Add(elementAsSprite.ID);
                                    }
                                }
                                //Checks if it's not colliding with the element, then removes it from the colliding list
                                else if (currentlyCollidingWithIDs.Contains(elementAsSprite.ID))
                                {
                                    currentlyCollidingWithIDs.Remove(elementAsSprite.ID);
                                }
                            }
                    }
                }
            }
        }

        protected override void drawElement(GameTime gameTime)
        {
            EdgeGame.drawTexture(Texture, Position, null, Style.Color, Scale, Style.Rotation, OriginPoint, Style.Effects);
        }

        public virtual void DebugDraw(Color color)
        {
            if (CollisionBody != null)
            {
                CollisionBody.Shape.DebugDraw(color);
            }
        }
    }
}
