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
        public Vector2 Scale;
        public float Rotation;
        public RenderTarget2D Target;
        private Vector2 TargetOriginPoint;

        //Used for clamping the camera position/rotation/scale to the element
        private Element clampedElement;
        private bool keepRotation;
        private bool keepScale;

        public Camera(Vector2 position, GraphicsDevice graphicsDevice)
        {
            Position = position;
            Scale = Vector2.One;
            Rotation = 0f;

            ReloadSize(graphicsDevice);
        }

        //Reloads the camera size
        public void ReloadSize(GraphicsDevice graphicsDevice)
        {
            Target = new RenderTarget2D(graphicsDevice, (int)graphicsDevice.PresentationParameters.BackBufferWidth, (int)graphicsDevice.PresentationParameters.BackBufferHeight);
            TargetOriginPoint = new Vector2(Target.Width, Target.Height) / 2;
        }

        //Updates the position/rotation/scale with the clamped element
        public void Update(GameTime gameTime)
        {
            if (clampedElement != null)
            {
                Position = clampedElement.Position;
                if (keepRotation && clampedElement is Sprite) { Rotation = ((Sprite)clampedElement).Rotation; }
                if (keepScale && clampedElement is Sprite) { Scale = ((Sprite)clampedElement).Scale; }
            }
        }

        //Draws the game to a spritebatch
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Target, Position, null, Color.White, MathHelper.ToRadians(Rotation), TargetOriginPoint, Scale, SpriteEffects.None, 0);
            spriteBatch.End();
        }
        
        //Clamps to an element
        public void ClampTo(Element element, bool clampRotation = false, bool clampScale = false)
        {
            clampedElement = element;
            keepRotation = clampRotation;
            keepScale = clampScale;
        }

        //Unclamps from the element
        public void Unclamp()
        {
            clampedElement = null;
        }
    }
}
