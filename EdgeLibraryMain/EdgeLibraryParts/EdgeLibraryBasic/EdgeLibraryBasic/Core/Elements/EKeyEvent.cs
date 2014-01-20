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

namespace EdgeLibrary.Basic
{
    public class EKeyEventArgs : EventArgs
    {
        public EUpdateArgs UpdateArgs;
        public Keys key;
        public bool pressed;

        public EKeyEventArgs(EUpdateArgs eUpdateArgs, Keys keyName, bool ispressed)
        {
            UpdateArgs = eUpdateArgs;
            key = keyName;
            pressed = ispressed;
        }
    }

    //Basically a timer - sends off an event whenever a certain time has passed
    public class EKeyEvent : EElement
    {
        public Keys Key;
        public bool Pressed;
        public bool continueActivating;

        private KeyboardState previousState;

        public delegate void EKeyEventHandler(EKeyEventArgs e);
        public event EKeyEventHandler Activate;

        public EKeyEvent(Keys key)
        {
            Key = key;
            Pressed = true;
            continueActivating = false;
            previousState = Keyboard.GetState();
        }

        public EKeyEvent(Keys key, bool pressed) : this(key)
        {
            Pressed = pressed;
        }

        public override void updateElement(EUpdateArgs updateArgs)
        {
            if (Pressed)
            {
                if (updateArgs.keyboardState.IsKeyDown(Key) && (previousState.IsKeyUp(Key) || continueActivating) && Activate != null)
                {
                    Activate(new EKeyEventArgs(updateArgs, Key, Pressed));
                }
            }
            else
            {
                if (updateArgs.keyboardState.IsKeyUp(Key) && (previousState.IsKeyDown(Key) || continueActivating) && Activate != null)
                {
                    Activate(new EKeyEventArgs(updateArgs, Key, Pressed));
                }
            }

            previousState = updateArgs.keyboardState;
        }
    }
}
