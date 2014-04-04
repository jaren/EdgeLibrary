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
        public float Scale;
        public float Rotation;
        public RenderTarget2D Target;
        private Vector2 HalfScreenSize;

        //Used for clamping the camera position/rotation/scale to the element
        private Element clampedElement;
        private bool keepPosition;
        private bool keepRotation;
        private bool keepScale;

        public Camera(Vector2 position, GraphicsDevice graphicsDevice)
        {
            Position = position;
            Scale = 1;
            Rotation = 0f;

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
                * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation))
                //Adds the origin
                * Matrix.CreateTranslation(HalfScreenSize.X * Scale, HalfScreenSize.Y * Scale, 0)
                //Adds the scale
                * Matrix.CreateScale(new Vector3(Scale));
            return matrix;
        }

        //Reloads the camera size
        public void ReloadSize(GraphicsDevice graphicsDevice)
        {
            Target = new RenderTarget2D(graphicsDevice, (int)graphicsDevice.PresentationParameters.BackBufferWidth, (int)graphicsDevice.PresentationParameters.BackBufferHeight);
            HalfScreenSize = new Vector2(Target.Width, Target.Height) / 2;
        }

        //Updates the position/rotation/scale with the clamped element
        public void Update(GameTime gameTime)
        {
            if (clampedElement != null)
            {
                if (keepPosition) { Position = clampedElement.Position; }
                if (keepRotation && clampedElement is Sprite) { Rotation = ((Sprite)clampedElement).Rotation; }
                if (keepScale && clampedElement is Sprite) { Scale = (((Sprite)clampedElement).Scale.X + ((Sprite)clampedElement).Scale.X)/2f; }
            }
        }

        //Draws the game to a spritebatch
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draws the render target to the middle of the screen with the rotation and scale
            spriteBatch.Begin();
            spriteBatch.Draw(Target, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
        
        //Clamps to an element
        public void ClampTo(Element element, bool clampRotation = false, bool clampScale = false, bool clampPosition = true)
        {
            clampedElement = element;
            keepRotation = clampRotation;
            keepScale = clampScale;
            keepPosition = clampPosition;
        }

        //Unclamps from the element
        public void Unclamp()
        {
            clampedElement = null;
        }
    }
}
