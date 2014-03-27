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
    //Currently, the camera cannot be moved because of render target boundaries
    public static class Camera
    {
        public static Vector2 Position;
        private static Element clampedElement;

        public static void UpdateWithGame()
        {
            if (clampedElement == null)
            {
                Position.X = EdgeGame.WindowSize.X / 2;
                Position.Y = EdgeGame.WindowSize.Y / 2;
            }
        }

        public static void ClampTo(Element element)
        {
            clampedElement = element;
        }

        public static void Update(GameTime gameTime)
        {
            if (clampedElement != null)
            {
                Position = clampedElement.Position;
            }
        }
    }
}
