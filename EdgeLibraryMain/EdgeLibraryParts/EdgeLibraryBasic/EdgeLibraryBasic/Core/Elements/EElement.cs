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

namespace EdgeLibrary.Basic
{
    //Base for all "game elements" - stuff that needs to be updated and drawn
    public class EElement : EObject
    {
        public virtual bool ClampedToMouse { get; set; }
        public ECollisionBody CollisionBody { get; set; }
        public virtual Vector2 Position { get; set; }
        public virtual string Data { get; set; }
        public virtual bool IsVisible { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool SupportsCollision { get; set; }
        public virtual int DrawLayer { get; set; }

        public Texture2D Texture { get; set; }
        public SpriteFont Font { get; set; }

        public EElement()
        {
            IsActive = true;
            IsVisible = true;
            SupportsCollision = false;
            DrawLayer = 0;
        }

        public virtual void FillTexture(EData eData) { }

        public void Update(EUpdateArgs updateArgs)
        {
            if (IsActive) { updateElement(updateArgs); }
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsVisible) { drawElement(spriteBatch, gameTime); }
        }

        public virtual void OnAddToLayer(ELayer layer) { }

        public virtual void updateElement(EUpdateArgs updateArgs) { }
        public virtual void drawElement(SpriteBatch spriteBatch, GameTime gameTime) { }

        public virtual void UpdateCollision(List<EElement> elements) { }


        protected void DrawStringToSpriteBatch(SpriteBatch spriteBatch, SpriteFont font, string text, Color color, float Rotation, Vector2 scale, SpriteEffects effects)
        {
            spriteBatch.DrawString(font, text, Position, color, MathHelper.ToRadians(Rotation), Vector2.Zero, scale, effects, 0);
        }

        protected void DrawToSpriteBatch(SpriteBatch spriteBatch, Rectangle? origin, Texture2D texture, Rectangle bounds, Color color, float Rotation, SpriteEffects effects)
        {
            spriteBatch.Draw(texture, bounds, origin, color, MathHelper.ToRadians(Rotation), Vector2.Zero, effects, 0);
        }

        protected void DrawToSpriteBatchWithScale(SpriteBatch spriteBatch, Rectangle? origin, Texture2D texture, float scale, Color color, float Rotation, SpriteEffects effects)
        {
            if (origin == null)
            {
                spriteBatch.Draw(texture, new Rectangle((int)(Position.X - texture.Width / 2 * scale), (int)(Position.Y - texture.Height / 2 * scale), (int)(texture.Width * scale), (int)(texture.Height * scale)), origin, color, MathHelper.ToRadians(Rotation), Vector2.Zero, effects, 0);
            }
            else
            {
                spriteBatch.Draw(texture, new Rectangle((int)(Position.X - ((Rectangle)origin).Width / 2 * scale), (int)(Position.Y - ((Rectangle)origin).Height / 2 * scale), (int)(((Rectangle)origin).Width * scale), (int)(((Rectangle)origin).Height * scale)), origin, color, MathHelper.ToRadians(Rotation), Vector2.Zero, effects, 0);
            }
        }

        protected void DrawToSpriteBatchWithWidth(SpriteBatch spriteBatch, Rectangle? origin, Texture2D texture, float width, Color color, float Rotation, SpriteEffects effects)
        {
            float ratioWH = texture.Width / texture.Height;
            spriteBatch.Draw(texture, new Rectangle((int)(Position.X - width / 2), (int)(Position.Y - width * ratioWH / 2), (int)width, (int)(width * ratioWH)), origin, color, MathHelper.ToRadians(Rotation), Vector2.Zero, effects, 0);
        }

        protected void DrawToSpriteBatchWithHeight(SpriteBatch spriteBatch, Rectangle? origin, Texture2D texture, float height, Color color, float Rotation, SpriteEffects effects)
        {
            float ratioHW = texture.Height / texture.Width;
            spriteBatch.Draw(texture, new Rectangle((int)(Position.X - height * ratioHW / 2), (int)(Position.Y - height / 2), (int)(height * ratioHW), (int)height), origin, color, MathHelper.ToRadians(Rotation), Vector2.Zero, effects, 0);
        }
    }
}
