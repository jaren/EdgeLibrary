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

namespace EdgeLibrary
{
    //Base for all "game elements" - stuff that needs to be updated and drawn
    public class Element : Object
    {
        public virtual bool ClampedToMouse { get; set; }
        public CollisionBody CollisionBody { get; set; }
        public virtual Vector2 Position { get; set; }
        public virtual string Data { get; set; }
        public virtual bool IsVisible { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool SupportsCollision { get; set; }
        public virtual int DrawLayer { get; set; }

        protected Element clampedObject;
        protected Vector2 clampPos;

        public Texture2D Texture { get; set; }
        public SpriteFont Font { get; set; }

        public Element()
        {
            IsActive = true;
            IsVisible = true;
            SupportsCollision = false;
            DrawLayer = 0;
        }

        public void clampTo(Element Element)
        {
            clampedObject = Element;
            clampPos = Vector2.Zero;
        }

        public void clampToAt(Element Element, Vector2 eClampPos)
        {
            clampedObject = Element;
            clampPos = eClampPos;
        }

        public void unclampFromElement() { clampedObject = null; }

        public virtual void FillTexture() { }

        public void Update(UpdateArgs updateArgs)
        {
            if (IsActive) { if (ClampedToMouse) { Position = new Vector2(updateArgs.mouseState.X, updateArgs.mouseState.Y); } else if (clampedObject != null) { Position = clampedObject.Position + clampPos; } updatElement(updateArgs); }
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsVisible) { drawElement(spriteBatch, gameTime); }
        }

        public virtual void OnAddToLayer(Layer layer) { }

        public virtual void updatElement(UpdateArgs updateArgs) { }
        public virtual void drawElement(SpriteBatch spriteBatch, GameTime gameTime) { }

        public virtual void UpdateCollision(List<Element> elements) { }


        protected void DrawStringToSpriteBatch(SpriteBatch spriteBatch, SpriteFont font, string text, Color color, float Rotation, Vector2 scale, SpriteEffects effects)
        {
            try
            {
                EdgeGame.DrawString(font, text, Position, color, MathHelper.ToRadians(Rotation), scale, Vector2.Zero, effects, 0);
            }
            catch
            {
            }
        }

        protected void DrawToSpriteBatch(SpriteBatch spriteBatch, Rectangle? origin, Texture2D texture, Rectangle bounds, Color color, float Rotation, SpriteEffects effects)
        {
            try
            {
            EdgeGame.DrawTexture(texture, bounds, origin, color, MathHelper.ToRadians(Rotation), Vector2.Zero, effects, 0);
            }
            catch
            {
            }
        }

        protected void DrawToSpriteBatchWithScale(SpriteBatch spriteBatch, Rectangle? origin, Texture2D texture, float scale, Color color, float Rotation, SpriteEffects effects)
        {
            try
            {
            if (origin == null)
            {
                EdgeGame.DrawTexture(texture, new Rectangle((int)(Position.X - texture.Width / 2 * scale), (int)(Position.Y - texture.Height / 2 * scale), (int)(texture.Width * scale), (int)(texture.Height * scale)), origin, color, MathHelper.ToRadians(Rotation), Vector2.Zero, effects, 0);
            }
            else
            {
                EdgeGame.DrawTexture(texture, new Rectangle((int)(Position.X - ((Rectangle)origin).Width / 2 * scale), (int)(Position.Y - ((Rectangle)origin).Height / 2 * scale), (int)(((Rectangle)origin).Width * scale), (int)(((Rectangle)origin).Height * scale)), origin, color, MathHelper.ToRadians(Rotation), Vector2.Zero, effects, 0);
            }
            }
            catch
            {
            }
        }

        protected void DrawToSpriteBatchWithWidth(SpriteBatch spriteBatch, Rectangle? origin, Texture2D texture, float width, Color color, float Rotation, SpriteEffects effects)
        {
            try
            {
            float ratioWH = texture.Width / texture.Height;
            EdgeGame.DrawTexture(texture, new Rectangle((int)(Position.X - width / 2), (int)(Position.Y - width * ratioWH / 2), (int)width, (int)(width * ratioWH)), origin, color, MathHelper.ToRadians(Rotation), Vector2.Zero, effects, 0);
            }
            catch
            {
            }
        }

        protected void DrawToSpriteBatchWithHeight(SpriteBatch spriteBatch, Rectangle? origin, Texture2D texture, float height, Color color, float Rotation, SpriteEffects effects)
        {
            try
            {
            float ratioHW = texture.Height / texture.Width;
            EdgeGame.DrawTexture(texture, new Rectangle((int)(Position.X - height * ratioHW / 2), (int)(Position.Y - height / 2), (int)(height * ratioHW), (int)height), origin, color, MathHelper.ToRadians(Rotation), Vector2.Zero, effects, 0);
            }
            catch
            {
            }
        }
    }
}
