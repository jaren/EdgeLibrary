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

namespace EdgeLibrary.Basic
{
    public enum ESpriteDrawType
    {
        NoRatio,
        KeepWidth,
        KeepHeight,
        Scaled
    }

    //The sprite's position is the center
    public class ESpriteCollisionArgs : EventArgs
    {
        public ESprite Sprite;
        public EElement Element;

        public ESpriteCollisionArgs(ESprite sprite, EElement element)
        {
            Sprite = sprite;
            Element = element;
        }
    }

    //Provides a base textured game object
    public class ESprite : EElement
    {
        //The texture name is stored in "Data"
        //The texture is stored in "ETexture" - CANNOT BE TEXTURE BECAUSE THERE IS A CLASS CALLED TEXTURE

        //Required for bounding box
        public Rectangle BoundingBox { get; set; }
        public override Vector2 Position { get { return _position; } set { _position = value; reloadBoundingBox(); } }
        public float Width { get { return _width; } set { _width = value; reloadBoundingBox(); } }
        public float Height { get { return _height; } set { _height = value; reloadBoundingBox(); } }
        public Vector2 Scale { get { return _scale; } set { _scale = value; reloadBoundingBox(); } }
        public bool ScaleCollisionBody;
        public ESpriteDrawType DrawType;
        public float ScaledDrawScale;
        protected Vector2 _position;
        protected float _width;
        protected float _height;
        protected Vector2 _scale;
        protected List<string> currentlyCollidingWithIDs;

        //Extra
        public float Rotation;
        public Color Color;

        protected List<EAction> Actions;
        protected List<int> ActionsToRemove;

        public delegate void SpriteCollisionEvent(ESpriteCollisionArgs e);
        public event SpriteCollisionEvent CollisionStart;
        public event SpriteCollisionEvent Collision;

        public ESprite(string eTextureName, Vector2 ePosition) : base()
        {
            DrawType = ESpriteDrawType.NoRatio;
            ScaleCollisionBody = true;
            ScaledDrawScale = 1f;
            Data = eTextureName;
            _position = ePosition;
            _width = 0;
            _height = 0;

            Color = Color.White;
            Rotation = 0;
            Scale = Vector2.One;

            currentlyCollidingWithIDs = new List<string>();

            Actions = new List<EAction>();
            ActionsToRemove = new List<int>();
        }

        public ESprite(string eTextureName, Vector2 ePosition, int eWidth, int eHeight) : this(eTextureName, ePosition)
        {
            _width = eWidth;
            _height = eHeight;
            reloadBoundingBox();
        }

        public ESprite(string eTextureName, Vector2 ePosition, int eWidth, int eHeight, Color eColor, float eRotation, Vector2 eScale) : this(eTextureName, ePosition, eWidth, eHeight)
        {
            Color = eColor;
            Rotation = eRotation;
            Scale = eScale;
        }

        public override void FillTexture(EData eData)
        {
            Texture = eData.getTexture(Data);
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

        public void AddCollision(ECollisionBody collisionBody)
        {
            CollisionBody = collisionBody;
            SupportsCollision = true;
        }

        public override void UpdateCollision(List<EElement> elements)
        {
            foreach (EElement element in elements)
            {
                if (element != this && element.SupportsCollision && element.CollisionBody != null)
                {
                    if (CollisionBody.CheckForCollide(element.CollisionBody))
                    {
                        if (Collision != null) { Collision(new ESpriteCollisionArgs(this, element)); }
                        if (CollisionStart != null && !currentlyCollidingWithIDs.Contains(element.CollisionBody.ID))
                        {
                            CollisionStart(new ESpriteCollisionArgs(this, element));
                            currentlyCollidingWithIDs.Add(element.CollisionBody.ID);
                        }
                    }
                    //Checks if it's not colliding with the element, then removes it from the colliding list
                    else if (currentlyCollidingWithIDs.Contains(element.CollisionBody.ID))
                    {
                        currentlyCollidingWithIDs.Remove(element.CollisionBody.ID);
                    }
                }
            }
        }

        public void reloadBoundingBox()
        {
            BoundingBox = new Rectangle((int)_position.X - ((int)_width / 2 * (int)Scale.X), (int)_position.Y - ((int)_height / 2 * (int)Scale.Y), (int)_width * (int)Scale.X, (int)_height * (int)Scale.Y);
        }

        public void runAction(EAction action)
        {
            action.PerformAction(this);
            if (action.RequiresUpdate)
            {
                Actions.Add(action);
            }
        }

        public void ClampToMouse() { ClampedToMouse = true; }
        public void UnclampFromMouse() { ClampedToMouse = false; }

        public override void updateElement(EUpdateArgs updateArgs)
        {
            ActionsToRemove.Clear();

            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].Update(this))
                {
                    ActionsToRemove.Add(i);
                }
            }

            for (int index = 0; index < ActionsToRemove.Count; index++ )
            {
                Actions.RemoveAt(ActionsToRemove[index]);
                for (int i = 0; i < ActionsToRemove.Count; i++ )
                {
                    ActionsToRemove[i]--;
                }
            }

            if (CollisionBody != null)
            {
                CollisionBody.Position = Position;
                if (ScaleCollisionBody)
                {
                    switch (CollisionBody.Shape.ShapeType)
                    {
                        case EShapeTypes.circle:
                            ((EShapeCircle)CollisionBody.Shape).Radius = (_width+_height)/4; //It's the average over 2, because the average of width+height is the diameter and this is the radius
                            break;
                        case EShapeTypes.rectangle:
                            ((EShapeRectangle)CollisionBody.Shape).Width = _width;
                            ((EShapeRectangle)CollisionBody.Shape).Height = _height;
                            break;
                    }
                }
            }

            if (ClampedToMouse) { _position.X = updateArgs.mouseState.X; _position.Y = updateArgs.mouseState.Y; reloadBoundingBox(); }
        }

        public override void drawElement(SpriteBatch spriteBatch, GameTime gameTime)
        {
            switch (DrawType)
            {
                case ESpriteDrawType.NoRatio:
                    base.DrawToSpriteBatch(spriteBatch, null, Texture, BoundingBox, Color, Rotation);
                    break;
                case ESpriteDrawType.KeepHeight:
                    base.DrawToSpriteBatchWithHeight(spriteBatch, null, Texture, Height, Color, Rotation);
                    break;
                case ESpriteDrawType.KeepWidth:
                    base.DrawToSpriteBatchWithHeight(spriteBatch, null, Texture, Width, Color, Rotation);
                    break;
                case ESpriteDrawType.Scaled:
                    base.DrawToSpriteBatchWithScale(spriteBatch, null, Texture, ScaledDrawScale, Color, Rotation);
                    break;
            }
        }
    }
}
