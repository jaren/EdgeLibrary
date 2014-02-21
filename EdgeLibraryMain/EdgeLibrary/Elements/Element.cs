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
    public class Element
    {
        public virtual string ID { get; set; }
        public virtual Vector2 Position { get; set; }
        public virtual bool Visible { get; set; }
        public virtual int LayerDepth { get; set; }
        public virtual Vector2 OriginPoint { get; set; }
        public virtual ClampCapability Clamp { get; set; }
        public virtual SimpleMovementCapability SimpleMovement { get; set; }
        public virtual FollowCapability Follow { get; set; }
        public virtual PointRotationCapability PointRotation { get; set; }

        private List<Capability> Capabilities;

        public Element(string id)
        {
            ID = id;

            Visible = true;

            //XNA Default
            OriginPoint = Vector2.Zero;

            //Each element has a new list of capabilities, not just references to the old ones
            Capabilities = new List<Capability>() {};
            Clamp = new ClampCapability();
            Capabilities.Add(Clamp);
            SimpleMovement = new SimpleMovementCapability();
            Capabilities.Add(SimpleMovement);
            Follow = new FollowCapability();
            Capabilities.Add(Follow);
            PointRotation = new PointRotationCapability();
            Capabilities.Add(PointRotation);

            //Automatically adds the element to the game
            EdgeGame.SelectedScene.AddElement(this);
        }

        public void REMOVE()
        {
            EdgeGame.RemoveElement(this);
        }

        public void AddCapability(Capability capability)
        {
            if (capability != null)
            {
                Capabilities.Add(capability);
            }
        }

        public Capability Capability(string id)
        {
            foreach (Capability capability in Capabilities)
            {
                if (capability.ID == id)
                {
                    return capability;
                }
            }
            return null;
        }

        public bool HasCapability(string id)
        {
            foreach (Capability capability in Capabilities)
            {
                if (capability.ID == id)
                {
                    return true;
                }
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            if (Visible) { foreach (Capability capability in Capabilities) { capability.Update(gameTime, this); } updateElement(gameTime); }
        }
        public void Draw(GameTime gameTime)
        {
            if (Visible) { drawElement(gameTime); }
        }

        public virtual void updateElement(GameTime gameTime) { }
        public virtual void drawElement(GameTime gameTime) { }

        protected void DrawString(SpriteFont font, string text, Color color, float Rotation, Vector2 scale, SpriteEffects effects)
        {
            EdgeGame.drawString(font, text, Position, color, MathHelper.ToRadians(Rotation), OriginPoint, scale, effects, LayerDepth);
        }

        protected void DrawTexture(Rectangle? origin, Texture2D texture, Rectangle bounds, Color color, float Rotation, SpriteEffects effects)
        {
            EdgeGame.drawTexture(texture, bounds, origin, color, MathHelper.ToRadians(Rotation), OriginPoint, effects, LayerDepth);
        }

        protected void DrawWithScale(Rectangle? origin, Texture2D texture, float scale, Color color, float Rotation, SpriteEffects effects)
        {
                if (origin == null)
                {
                    EdgeGame.drawTexture(texture, new Rectangle((int)(Position.X - texture.Width / 2 * scale), (int)(Position.Y - texture.Height / 2 * scale), (int)(texture.Width * scale), (int)(texture.Height * scale)), origin, color, MathHelper.ToRadians(Rotation), OriginPoint, effects, LayerDepth);
                }
                else
                {
                    EdgeGame.drawTexture(texture, new Rectangle((int)(Position.X - ((Rectangle)origin).Width / 2 * scale), (int)(Position.Y - ((Rectangle)origin).Height / 2 * scale), (int)(((Rectangle)origin).Width * scale), (int)(((Rectangle)origin).Height * scale)), origin, color, MathHelper.ToRadians(Rotation), OriginPoint, effects, LayerDepth);
                }
        }

        protected void DrawWithWidth(Rectangle? origin, Texture2D texture, float width, Color color, float Rotation, SpriteEffects effects)
        {
                float ratioWH = texture.Width / texture.Height;
                EdgeGame.drawTexture(texture, new Rectangle((int)(Position.X - width / 2), (int)(Position.Y - width * ratioWH / 2), (int)width, (int)(width * ratioWH)), origin, color, MathHelper.ToRadians(Rotation), OriginPoint, effects, LayerDepth);
        }

        protected void DrawWithHeight(Rectangle? origin, Texture2D texture, float height, Color color, float Rotation, SpriteEffects effects)
        {
                float ratioHW = texture.Height / texture.Width;
                EdgeGame.drawTexture(texture, new Rectangle((int)(Position.X - height * ratioHW / 2), (int)(Position.Y - height / 2), (int)(height * ratioHW), (int)height), origin, color, MathHelper.ToRadians(Rotation), OriginPoint, effects, LayerDepth);
        }
    }
}
