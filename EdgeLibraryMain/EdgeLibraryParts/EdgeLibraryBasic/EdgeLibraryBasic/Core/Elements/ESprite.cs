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
        protected Vector2 _position;
        protected float _width;
        protected float _height;
        protected Vector2 _scale;

        //Extra
        public float Rotation;
        public Color Color;

        protected List<EAction> Actions;
        protected List<int> ActionsToRemove;

        public delegate void SpriteCollisionEvent(ESpriteCollisionArgs e);
        public event SpriteCollisionEvent CollisionStart;

        public ESprite(string eTextureName, Vector2 ePosition, int eWidth, int eHeight) : base()
        {
            Data = eTextureName;
            _position = ePosition;
            _width = eWidth;
            _height = eHeight;
            reloadBoundingBox();

            Color = Color.White;
            Rotation = 0;
            Scale = Vector2.One;

            Actions = new List<EAction>();
            ActionsToRemove = new List<int>();
        }

        public ESprite(string eTextureName, Vector2 ePosition, int eWidth, int eHeight, Color eColor, float eRotation, Vector2 eScale) : this(eTextureName, ePosition, eWidth, eHeight)
        {
            Color = eColor;
            Rotation = eRotation;
            Scale = eScale;
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
                if (element.SupportsCollision && element.CollisionBody != null && CollisionStart != null)
                {
                    if (CollisionBody.CheckForCollide(element.CollisionBody))
                    {
                        CollisionStart(new ESpriteCollisionArgs(this, element));
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

        public void Delete()
        {
            //Don't know what to do here
        }

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
                CollisionBody.Position = new Vector2(Position.X + Width/2, Position.Y + Height/2);
            }

            if (ClampedToMouse) { _position.X = updateArgs.mouseState.X; _position.Y = updateArgs.mouseState.Y; reloadBoundingBox(); }
        }

        public override void drawElement(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawToSpriteBatch(spriteBatch, Texture, BoundingBox, Color, Rotation, Scale);
        }
    }
}
