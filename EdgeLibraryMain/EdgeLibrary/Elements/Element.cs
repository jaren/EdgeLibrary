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
    //Base for all "game elements" - stuff that needs to be updated or drawn
    public class Element
    {
        //Used to identify the element, there can only be one of each ID per scene
        public string ID { get; set; }

        //If set to true, the element will be removed the next frame
        public bool toRemove { get; protected set; }

        //Location of the element
        public virtual Vector2 Position { get; set; }

        //Which blend state to use when drawing the element
        public BlendState BlendState { get; set; }

        //If set to false, the element will not update or draw
        public bool Visible { get; set; }

        //Where to draw the element; the higher it is, the higher it will appear on the screen
        public int DrawLayer { get; set; }

        //Events for the element updating
        public delegate void ElementUpdateEvent(Element element, GameTime gameTime);
        public ElementUpdateEvent OnUpdate = delegate { };
        public ElementUpdateEvent OnDraw = delegate { };

        public Element()
        {
            ID = MathTools.GenerateID(this);

            Visible = true;
            BlendState = BlendState.AlphaBlend;
        }

        //Override of object.ToString() to return ID
        public override string ToString()
        {
            return ID;
        }

        //Marks the element to be removed next frame
        public void Remove()
        {
            toRemove = true;
        }

        //Updates the element
        public void Update(GameTime gameTime)
        {
            if (Visible)
            {
                UpdateObject(gameTime);
                OnUpdate(this, gameTime);
            }
        }

        //Adds the element to the game, basically a shortcut for EdgeGame.Instance.SceneHandler.CurrentScene.AddElement(element)
        public void AddToGame()
        {
            EdgeGame.Instance.SceneHandler.CurrentScene.AddElement(this);
        }

        //Prepares the element for drawing
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                if (BlendState != BlendState.AlphaBlend)
                {
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, EdgeGame.Instance.Camera.GetTransform());
                }

                DrawObject(gameTime, spriteBatch);

                if (BlendState != BlendState.AlphaBlend)
                {
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, EdgeGame.Instance.Camera.GetTransform());
                }

                OnDraw(this, gameTime);
            }
        }

        protected virtual void UpdateObject(GameTime gameTime) { }
        protected virtual void DrawObject(GameTime gameTime, SpriteBatch spriteBatch) { }
    }
}
