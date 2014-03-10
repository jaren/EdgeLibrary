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
        public virtual bool MarkedForRemoval { get; set; }
        public virtual Vector2 Position { get; set; }
        public virtual bool Visible { get; set; }
        public virtual int DrawLayer { get; set; }
        public virtual BlendState BlendState { get; set; }
        public virtual MovementCapability Movement { get; set; }

        protected List<Capability> Capabilities;

        public delegate void ElementUpdateEvent(Element e, GameTime gameTime);
        public ElementUpdateEvent update;
        public ElementUpdateEvent draw;

        public Element(string id)
        {
            ID = id;

            Visible = true;
            BlendState = BlendState.AlphaBlend; 

            //Each element has a new list of capabilities, not just references to the old ones
            Capabilities = new List<Capability>() {};
            Movement = new MovementCapability();
            Capabilities.Add(Movement);

            //Automatically adds the element to the game
            EdgeGame.SelectedScene.AddElement(this);
        }

        public override string ToString()
        {
            return ID;
        }

        public void REMOVE()
        {
            MarkedForRemoval = true;
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
            if (Visible) 
            { 
                foreach (Capability capability in Capabilities)
                { 
                    capability.Update(gameTime, this); 
                }
                updateElement(gameTime);

                if (update != null) { update(this, gameTime); }
            }
        }
        public void Draw(GameTime gameTime)
        {
            if (Visible) 
            { 
                if (BlendState != BlendState.AlphaBlend)
                {
                    EdgeGame.RestartSpriteBatch(SpriteSortMode.Deferred, BlendState);
                }
                
                drawElement(gameTime);
                
                if (BlendState != BlendState.AlphaBlend)
                {
                    EdgeGame.RestartSpriteBatch();
                }
                
                if (draw != null)
                {
                    draw(this, gameTime);
                }
            }
        }

        protected virtual void updateElement(GameTime gameTime) { }
        protected virtual void drawElement(GameTime gameTime) { }
    }
}
