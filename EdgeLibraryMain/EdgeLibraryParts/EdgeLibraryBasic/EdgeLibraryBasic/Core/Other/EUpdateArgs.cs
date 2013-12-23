using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;

namespace EdgeLibrary.Basic
{
    public class EUpdateArgs
    {
        public GameTime gameTime;
        public KeyboardState keyboardState;
        public MouseState mouseState;

        public EUpdateArgs(GameTime eGameTime, KeyboardState eKeyboardState, MouseState eMouseState)
        {
            gameTime = eGameTime;
            keyboardState = eKeyboardState;
            mouseState = eMouseState;
        }
    }
}
