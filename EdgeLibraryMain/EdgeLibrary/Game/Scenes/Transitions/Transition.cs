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
        protected List<Color[]> Colors;
        protected Color[] ColorArray1;
        protected Color[] ColorArray2;

        //Called when the transition finishes
        public delegate void TransitionEvent(Transition t, GameTime gameTime);
        public event TransitionEvent OnFinish = delegate { };

        //The scene to switch to on finishing
        public Scene SceneOnFinish;
        //The time per "frame" - a texture
        //Frames are generated before running the game; it's too expensive generating it every frame
        protected double TimePerFrame;
        //The number of frames
        protected int Frames;
        protected int currentFrame;
        protected double elapsedTime;

        public Transition(string id, Scene a, Scene b, float timePerFrame, int frames) : base(id)
        {
            //Sets the second scene to be run on finishing
            SceneOnFinish = b;
            //Renders the scenes to textures and generates color arrays
            Texture1 = a.RenderToTexture(EdgeGame.Instance.GameTime, EdgeGame.Instance.GraphicsDevice, EdgeGame.Instance.SpriteBatch);
            Texture2 = b.RenderToTexture(EdgeGame.Instance.GameTime, EdgeGame.Instance.GraphicsDevice, EdgeGame.Instance.SpriteBatch);
            Background = Texture1;
            TimePerFrame = timePerFrame;
            Frames = frames;
            currentFrame = 0;
            elapsedTime = 0;

            if ((Texture1.Width != Texture2.Width) || (Texture1.Height != Texture2.Height))
            {
                throw new Exception("Texture sizes do not match");
            }

            ColorArray1 = new Color[Texture1.Width * Texture1.Height];
            ColorArray2 = new Color[Texture1.Width * Texture1.Height];
            Texture1.GetData<Color>(ColorArray1);
            Texture2.GetData<Color>(ColorArray2);
            Colors = new List<Color[]>();
            GenerateFrames();
            Background.SetData<Color>(Colors[0]);
        }
        public virtual void GenerateFrames() { }
        public override void Update(GameTime gameTime)
        {
            //Check if the frame should switch and changes the texture
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsedTime >= TimePerFrame)
            {
                elapsedTime = 0;
                currentFrame++;
                if (currentFrame > Frames - 1)
                {
                    OnFinish(this, gameTime);
                    EdgeGame.Instance.SceneHandler.SwitchScene(SceneOnFinish);
                }
                else
                {
                    Background.SetData<Color>(Colors[currentFrame]);
                }
            }
            base.Update(gameTime);
        }
    }
}
