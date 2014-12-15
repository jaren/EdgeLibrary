using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeLibrary
{
    public class ModifiedSpriteBatch : SpriteBatch
    {
        public bool IsStarted { get; private set; }

        public ModifiedSpriteBatch(GraphicsDevice graphicsDevice) : base(graphicsDevice) { }

        public new void Begin()
        {
            if (!IsStarted)
            {
                IsStarted = true;
                base.Begin();
            }
        }

        public new void Begin(SpriteSortMode sortMode, BlendState blendState)
        {
            if (!IsStarted)
            {
                IsStarted = true;
                base.Begin(sortMode, blendState);
            }
        }

        public new void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState)
        {
            if (!IsStarted)
            {
                IsStarted = true;
                base.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState);
            }
        }

        public new void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect)
        {
            if (!IsStarted)
            {
                IsStarted = true;
                base.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect);
            }
        }

        public new void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Microsoft.Xna.Framework.Matrix transformMatrix)
        {
            if (!IsStarted)
            {
                IsStarted = true;
                base.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
            }
        }

        public new void End()
        {
            if (IsStarted)
            {
                IsStarted = false;
                base.End();
            }
        }
    }
}
