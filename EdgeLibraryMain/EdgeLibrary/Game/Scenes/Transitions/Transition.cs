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
    //Creates a transition between two scenes
    public class Transition : Scene
    {
        //Generates two textures from rendering scenes
        protected Texture2D Texture1;
        protected Texture2D Texture2;

        //The total time the transition should take to run
        protected double Time;
        protected double elapsedTime;

        public Transition(string id, Scene a, Scene b, float time) : base(id)
        {
            Texture1 = a.RenderToTexture(EdgeGame.Instance.GameTime, EdgeGame.Instance.GraphicsDevice, EdgeGame.Instance.SpriteBatch);
            Texture2 = a.RenderToTexture(EdgeGame.Instance.GameTime, EdgeGame.Instance.GraphicsDevice, EdgeGame.Instance.SpriteBatch);
            Background = Texture1;
            Time = time;
            elapsedTime = 0;

            if ((Texture1.Width != Texture2.Width) || (Texture1.Height != Texture2.Height))
            {
                throw new Exception("Texture sizes do not match");
            }
        }
        public virtual void UpdateTransition(GameTime gameTime) { }
        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            UpdateTransition(gameTime);
            base.Update(gameTime);
        }
    }
}
