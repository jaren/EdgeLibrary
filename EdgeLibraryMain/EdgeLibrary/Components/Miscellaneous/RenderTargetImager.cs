using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeLibrary
{
    public class RenderTargetImager : Scene
    {
        private RenderTarget2D Target;

        public RenderTargetImager() : base()
        {
            EdgeGame.OnResized += EdgeGame_OnResized;
            EdgeGame_OnResized();
        }

        void EdgeGame_OnResized()
        {
            Target = new RenderTarget2D(EdgeGame.Game.GraphicsDevice, (int)EdgeGame.WindowSize.X, (int)EdgeGame.WindowSize.Y);
        }

        public Texture2D RenderToTarget(Color clearColor)
        {
            EdgeGame.Game.SpriteBatch.GraphicsDevice.SetRenderTarget(Target);
            EdgeGame.Game.GraphicsDevice.Clear(clearColor);
            EdgeGame.Game.SpriteBatch.Begin();

            foreach (DrawableGameComponent component in Components.OfType<DrawableGameComponent>())
            {
                component.Draw(EdgeGame.GameTime);
            }

            EdgeGame.Game.SpriteBatch.End();
            EdgeGame.Game.SpriteBatch.GraphicsDevice.SetRenderTarget(null);

            return Target;
        }
    }
}
