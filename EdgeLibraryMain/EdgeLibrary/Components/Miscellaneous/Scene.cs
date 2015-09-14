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
    /// <summary>
    /// A container of game components - could be used like a "screen" in a game
    /// </summary>
    public class Scene : DrawableGameComponent, ICloneable
    {
        public BlendState BlendState = BlendState.AlphaBlend;
        public SamplerState SamplerState = SamplerState.LinearClamp;
        public GameComponentCollection Components { get; protected set; }

        public Scene(List<GameComponent> components) : base(EdgeGame.Game)
        {
            Components = new GameComponentCollection();
            foreach (GameComponent component in components)
            {
                Components.Add(component);
            }
        }

        public Scene(params GameComponent[] components) : this(new List<GameComponent>(components)) { }

        public virtual void UpdateObject(GameTime gameTime)
        {

        }

        public override sealed void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                foreach (GameComponent component in Components)
                {
                    component.Update(gameTime);
                }
                base.Update(gameTime);
                UpdateObject(gameTime);
            }
        }

        public virtual void DrawObject(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                RestartSpriteBatch();

                foreach (DrawableGameComponent component in Components.OfType<DrawableGameComponent>())
                {
                    component.Draw(gameTime);
                }
                base.Draw(gameTime);
                DrawObject(gameTime);

                RestartSpriteBatch();
            }
        }

        //Restarts the spritebatch if the blend state is not AlphaBlend
        //Should be called before and after drawing
        protected void RestartSpriteBatch(bool forceRestart = false)
        {
            if (forceRestart || BlendState != BlendState.AlphaBlend || SamplerState != SamplerState.LinearClamp)
            {
                EdgeGame.Game.SpriteBatch.End();
                EdgeGame.Game.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState, SamplerState, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            }

            if (!EdgeGame.Game.SpriteBatch.IsStarted)
            {
                EdgeGame.Game.SpriteBatch.Begin();
            }
        }

        public object Clone()
        {
            Scene scene = new Scene();
            foreach (GameComponent component in Components)
            {
                if (component is ICloneable)
                {
                    scene.Components.Add((GameComponent)((ICloneable)component).Clone());
                }
            }
            return scene;
        }
    }
}
