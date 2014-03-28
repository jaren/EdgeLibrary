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

namespace EdgeLibrary
{
    public class Sprite : Element
    {
        //Gets the texture size of the sprite
        public virtual float Width { get { return Texture == null ? 0 : Texture.Width; } protected set { } }
        public virtual float Height { get { return Texture == null ? 0 : Texture.Height; } protected set { } }

        //Sets the scale with a Vector2
        public Vector2 Scale { get; set; }

        //Optional visual effects
        public Color Color { get; set; }
        public float Rotation { get; set; }
        public SpriteEffects SpriteEffects { get; set; }

        //Gets the origin point of the sprite, which is either (0, 0) or half of the texture size
        public Vector2 OriginPoint { get; protected set; }

        //If set to true, origin will be top left; if not, origin will be center
        public bool CenterAsOrigin { get { return _centerAsOrigin; } set { _centerAsOrigin = value; reloadOriginPoint(); } }
        protected bool _centerAsOrigin;

        //Used for detecting collisions
        public virtual CollisionBody CollisionBody { get; set; }
        public virtual ShapeTypes CollisionBodyType { get; set; }

        //The texture for drawing
        public Texture2D Texture { get { return _texture; } set { _texture = value; reloadOriginPoint(); } }
        protected Texture2D _texture;

        //Used for OnCollideStart
        protected List<string> currentlyCollidingWithIDs;

        //Used for collisions
        public delegate void CollisionEvent(Sprite sender, Sprite collided, GameTime gameTime);
        public event CollisionEvent OnCollideStart = delegate { };
        public event CollisionEvent OnCollide = delegate { };

        public Sprite(string textureName, Vector2 position)
        {
            Position = position;

            //Sets the default visual effects
            Scale = Vector2.One;
            Color = Color.White;
            Rotation = 0f;
            SpriteEffects = SpriteEffects.None;

            _centerAsOrigin = true;

            currentlyCollidingWithIDs = new List<string>();

            CollisionBodyType = ShapeTypes.rectangle;
            CollisionBody = CollisionBody.BodyWithSprite(this, CollisionLayers.All);

            if (textureName != null)
            {
                Texture = EdgeGame.GetCurrentGame().Resources.GetTexture(textureName);
            }
            reloadOriginPoint();
        }
        public Sprite(string textureName, Vector2 position, Color color, Vector2 scale, float rotation = 0f, SpriteEffects effects = SpriteEffects.None) : this(textureName, position)
        {
            Color = color;
            Rotation = rotation;
            Scale = scale;
            SpriteEffects = effects;
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

        //Checks if this is colliding with any other sprite
        public virtual void UpdateCollision(GameTime gameTime, List<Element> elements)
        {
            if (CollisionBody != null)
            {
                //Scales the collision body
                CollisionBody.ScaleWith(this);
                
                //If not using center as origin, change the CollisionBody position
                if (_centerAsOrigin)
                {
                    CollisionBody.Position = new Vector2(Position.X, Position.Y);
                }
                else
                {
                    CollisionBody.Position = new Vector2(Position.X + Width / 2, Position.Y + Height / 2);
                }

                //Loops through all the elements and checks if they're colliding
                foreach (Element element in elements)
                {
                    if (element is Sprite)
                    {
                        Sprite elementAsSprite = (Sprite)element;

                        if (elementAsSprite != this && elementAsSprite.CollisionBody != null)
                        {
                            if (CollisionBody.CheckForCollide(elementAsSprite.CollisionBody))
                            {
                                OnCollide(this, elementAsSprite, gameTime);
                                if (!currentlyCollidingWithIDs.Contains(elementAsSprite.ID))
                                {
                                    OnCollideStart(this, elementAsSprite, gameTime);
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

        //Draws to the spritebatch
        protected override void DrawObject(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color, Rotation, OriginPoint, Scale, SpriteEffects, 0);
        }

        //Draws the area of the collision body
        public virtual void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            if (CollisionBody != null)
            {
                CollisionBody.Shape.DrawDebug(gameTime, spriteBatch, color);
            }
        }
    }
}