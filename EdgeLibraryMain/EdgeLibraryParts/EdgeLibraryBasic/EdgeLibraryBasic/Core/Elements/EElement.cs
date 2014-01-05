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

        public void Update(EUpdateArgs updateArgs)
        {
            if (IsActive) { updateElement(updateArgs); }
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsVisible) { drawElement(spriteBatch, gameTime); }
        }

        public virtual void OnAddToScene(EScene scene) { }

        public virtual void updateElement(EUpdateArgs updateArgs) { }
        public virtual void drawElement(SpriteBatch spriteBatch, GameTime gameTime) { }

        public virtual void UpdateCollision(List<EElement> elements) { }

        protected void DrawStringToSpriteBatch(SpriteBatch spriteBatch, SpriteFont font, string text, Color color, float Rotation, Vector2 scale)
        {
            spriteBatch.DrawString(font, text, Position, color, (float)EMath.RadiansToDegrees(Rotation), Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        protected void DrawToSpriteBatch(SpriteBatch spriteBatch, Texture2D texture, Rectangle bounds, Color color, float Rotation)
        {
            spriteBatch.Draw(texture, bounds, null, color, (float)EMath.RadiansToDegrees(Rotation), Vector2.Zero, SpriteEffects.None, 0);
        }

        protected void DrawToSpriteBatchWithScale(SpriteBatch spriteBatch, Vector2 centerPos, Texture2D texture, float scale, Color color, float Rotation)
        {
            spriteBatch.Draw(texture, new Rectangle((int)(centerPos.X - texture.Width/2 * scale), (int)(centerPos.Y - texture.Height/2 * scale), (int)(texture.Width*scale), (int)(texture.Height*scale)), null, color, (float)EMath.RadiansToDegrees(Rotation), Vector2.Zero, SpriteEffects.None, 0);
        }

        protected void DrawToSpriteBatchWithWidth(SpriteBatch spriteBatch, Vector2 centerPos, Texture2D texture, float width, Color color, float Rotation)
        {
            float ratioWH = texture.Width / texture.Height;
            spriteBatch.Draw(texture, new Rectangle((int)(centerPos.X - width / 2), (int)(centerPos.Y - width * ratioWH / 2), (int)width, (int)(width * ratioWH)), null, color, (float)EMath.RadiansToDegrees(Rotation), Vector2.Zero, SpriteEffects.None, 0);
        }

        protected void DrawToSpriteBatchWithHeight(SpriteBatch spriteBatch, Vector2 centerPos, Texture2D texture, float height, Color color, float Rotation)
        {
            float ratioHW = texture.Height / texture.Width;
            spriteBatch.Draw(texture, new Rectangle((int)(centerPos.X - height * ratioHW / 2), (int)(centerPos.Y - height / 2), (int)(height * ratioHW), (int)height), null, color, (float)EMath.RadiansToDegrees(Rotation), Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
