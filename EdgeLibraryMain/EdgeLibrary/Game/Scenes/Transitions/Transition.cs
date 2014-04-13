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
        //The list of frames
        protected List<Color[]> Colors;
        //The first scene's color data
        protected Color[] ColorArray1;
        //The second scene's color data
        protected Color[] ColorArray2;
        //Used for GenerateFrame
        protected Color[] CurrentColors;

        //Called when the transition finishes
        public delegate void TransitionEvent(Transition t, GameTime gameTime);
        public event TransitionEvent OnFinish = delegate { };

        public Scene StartScene;
        public Scene FinishScene;
        //The time per "frame" - a texture
        //Frames are generated before running the game; it's too expensive generating it every frame
        protected double TimePerFrame;
        //The number of frames
        protected int Frames;
        protected int currentFrame;
        protected double elapsedTime;

        public Transition(Scene a, Scene b, float timePerFrame, int frames) : base("")
        {
            ID = this.GenerateID();
            StartScene = a;
            FinishScene = b;
            TimePerFrame = timePerFrame;
            Frames = frames;
            currentFrame = 0;
            elapsedTime = 0;
        }
        public override void WhenSwitched()
        {
            Background = StartScene.RenderToTexture(EdgeGame.Instance.GameTime, EdgeGame.Instance.GraphicsDevice, EdgeGame.Instance.SpriteBatch);

            //Renders the scenes to textures and generates color arrays
            Texture1 = StartScene.RenderToTexture(EdgeGame.Instance.GameTime, EdgeGame.Instance.GraphicsDevice, EdgeGame.Instance.SpriteBatch);
            Texture2 = FinishScene.RenderToTexture(EdgeGame.Instance.GameTime, EdgeGame.Instance.GraphicsDevice, EdgeGame.Instance.SpriteBatch);
            Background = Texture1;

            //Throw exception if texture sizes don't match
            if ((Texture1.Width != Texture2.Width) || (Texture1.Height != Texture2.Height))
            {
                throw new Exception("Texture sizes do not match");
            }

            //Gets the color data
            ColorArray1 = new Color[Texture1.Width * Texture1.Height];
            ColorArray2 = new Color[Texture1.Width * Texture1.Height];
            CurrentColors = ColorArray1;
            Texture1.GetData<Color>(ColorArray1);
            Texture2.GetData<Color>(ColorArray2);
            Colors = new List<Color[]>();

            for (int i = 0; i < Frames; i++)
            {
                GenerateFrame(i);
                Colors.Add((Color[])CurrentColors.Clone());
            }
        }
        public virtual void GenerateFrame(int frame) { }
        public override void Update(GameTime gameTime)
        {
            //Check if the frame should switch and changes the texture
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds * EdgeGame.GetFrameTimeMultiplier(gameTime);
            if (elapsedTime >= TimePerFrame)
            {
                elapsedTime = 0;
                currentFrame++;
                if (currentFrame > Frames - 1)
                {
                    OnFinish(this, gameTime);
                    EdgeGame.Instance.SceneHandler.SwitchScene(FinishScene);
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
