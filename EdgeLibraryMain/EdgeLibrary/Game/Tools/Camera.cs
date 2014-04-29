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
    //Used for drawing the game to the screen
    public class Camera
    {
        //The camera data
        public Vector2 Position;
        public float Scale { get { return _scale; } set { _scale = value; if (!SupportsZeroScale && _scale <= 0) { _scale = 0.1f; } } }
        private float _scale;
        //In radians
        public float Rotation;

        //If set to true, then the camera will accept values of 0 and under for the scale
        public bool SupportsZeroScale;

        public RenderTarget2D Target;
        private Vector2 HalfScreenSize;

        public Camera(Vector2 position, GraphicsDevice graphicsDevice)
        {
            Position = position;
            Scale = 1;
            Rotation = 0f;

            SupportsZeroScale = false;

            ReloadSize(graphicsDevice);
        }

        //Returns the spritebatch transformation used with this camera
        public Matrix GetTransform()
        {
            //Creates the transform matrix
            Matrix matrix = Matrix.Identity
                //Adds the position
                * Matrix.CreateTranslation(-Position.X, -Position.Y, 0)
                //Adds the rotation
                * Matrix.CreateRotationZ(Rotation)
                //Adds the scale
                * Matrix.CreateScale(new Vector3(Scale))
                //Adds the origin
                * Matrix.CreateTranslation(HalfScreenSize.X, HalfScreenSize.Y, 0);
            return matrix;
        }

        //Reloads the camera size
        public void ReloadSize(GraphicsDevice graphicsDevice)
        {
            Target = new RenderTarget2D(graphicsDevice, (int)graphicsDevice.PresentationParameters.BackBufferWidth, (int)graphicsDevice.PresentationParameters.BackBufferHeight);
            HalfScreenSize = new Vector2(Target.Width, Target.Height) / 2;
        }

        //Draws the game to a spritebatch
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draws the render target to the middle of the screen with the rotation and scale
            spriteBatch.Begin();
            spriteBatch.Draw(Target, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
