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
    public class KeyEventArgs : EventArgs
    {
        public UpdateArgs UpdateArgs;
        public Keys key;
        public bool pressed;

        public KeyEventArgs(UpdateArgs updateArgs, Keys keyName, bool ispressed)
        {
            UpdateArgs = updateArgs;
            key = keyName;
            pressed = ispressed;
        }
    }

    //Basically a timer - sends off an event whenever a certain time has passed
    public class KeyEvent : Element
    {
        public Keys Key;
        public bool Pressed;
        public bool continueActivating;

        private KeyboardState previousState;

        public delegate void KeyEventHandler(KeyEventArgs e);
        public event KeyEventHandler Activate;

        public KeyEvent(Keys key)
        {
            Key = key;
            Pressed = true;
            continueActivating = false;
            previousState = Keyboard.GetState();
        }

        public KeyEvent(Keys key, bool pressed) : this(key)
        {
            Pressed = pressed;
        }

        public override void updatElement(UpdateArgs updateArgs)
        {
            if (Pressed)
            {
                if (updateArgs.keyboardState.IsKeyDown(Key) && (previousState.IsKeyUp(Key) || continueActivating) && Activate != null)
                {
                    Activate(new KeyEventArgs(updateArgs, Key, Pressed));
                }
            }
            else
            {
                if (updateArgs.keyboardState.IsKeyUp(Key) && (previousState.IsKeyDown(Key) || continueActivating) && Activate != null)
                {
                    Activate(new KeyEventArgs(updateArgs, Key, Pressed));
                }
            }

            previousState = updateArgs.keyboardState;
        }
    }
}
