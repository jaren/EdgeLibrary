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
using System.Xml;
using System.Xml.Linq;


namespace EdgeLibrary
{
    public static class Input
    {
        private static KeyboardState keyboard;
        private static KeyboardState previousKeyboard;
        private static MouseState mouse;
        private static MouseState previousMouse;

        //Returns the mouse position with camera transformations
        public static Vector2 MousePosition { get { return new Vector2(mouse.X, mouse.Y) + EdgeGame.Camera.Position - EdgeGame.WindowSize/2; } set { } }
        //Returns the previous mouse position with CURRENT camera transformations
        public static Vector2 PreviousMousePosition { get { return new Vector2(previousMouse.X, previousMouse.Y) + EdgeGame.Camera.Position - EdgeGame.WindowSize / 2; } set { } }
        public static int MouseWheelValue { get { return mouse.ScrollWheelValue; } set { } }
        public static int PreviousMouseWheelValue { get { return previousMouse.ScrollWheelValue; } set { } }

        public static delegate void MouseEvent(Vector2 mousePosition, Vector2 previousMousePosition);
        public static event MouseEvent OnMouseMove;
        public static event MouseEvent OnClick;
        public static event MouseEvent OnReleaseClick;
        public static event MouseEvent OnRightClick;
        public static event MouseEvent OnReleaseRightClick;
        public static event MouseEvent OnMiddleClick;
        public static event MouseEvent OnReleaseMiddleClick;
        public static delegate void MouseWheelEvent(int mouseWheelValue, int previousMouseWheelValue);
        public static event MouseWheelEvent OnMouseWheelChanged;
        public static delegate void KeyboardEvent(Keys key);
        public static event KeyboardEvent OnKeyPress;
        public static event KeyboardEvent OnKeyRelease;

        public static void Update(GameTime gameTime)
        {
            previousKeyboard = keyboard;
            previousMouse = mouse;
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();

            if (MousePosition != PreviousMousePosition)
            {
                if (OnMouseMove != null)
                {
                    OnMouseMove(MousePosition, PreviousMousePosition);
                }
            }
            if (JustLeftClicked())
            {
                if (OnClick != null)
                {
                    OnClick(MousePosition, PreviousMousePosition);
                }
            }
            if (JustReleasedLeftClick())
            {
                if (OnReleaseClick != null)
                {
                    OnReleaseClick(MousePosition, PreviousMousePosition);
                }
            }
            if (JustRightClicked())
            {
                if (OnRightClick != null)
                {
                    OnRightClick(MousePosition, PreviousMousePosition);
                }
            }
            if (JustReleasedRightClick())
            {
                if (OnReleaseRightClick != null)
                {
                    OnReleaseRightClick(MousePosition, PreviousMousePosition);
                }
            }
            if (JustMiddleClicked())
            {
                if (OnMiddleClick != null)
                {
                    OnMiddleClick(MousePosition, PreviousMousePosition);
                }
            }
            if (JustReleasedMiddleClick())
            {
                if (OnReleaseMiddleClick != null)
                {
                    OnReleaseMiddleClick(MousePosition, PreviousMousePosition);
                }
            }

            if (keyboard.GetPressedKeys() != previousKeyboard.GetPressedKeys())
            {
                foreach(Keys key in keyboard.GetPressedKeys())
                {
                    if (!previousKeyboard.GetPressedKeys().Contains(key))
                    {
                        OnKeyPress(key);
                    }
                }

                foreach(Keys key in previousKeyboard.GetPressedKeys())
                {
                    if (!keyboard.GetPressedKeys().Contains(key))
                    {
                        OnKeyRelease(key);
                    }
                }
            }
        }

        public static Keys[] KeysPressed()
        {
            return keyboard.GetPressedKeys();
        }

        public static bool KeyJustPressed(Keys k)
        {
            return (keyboard.IsKeyDown(k) && previousKeyboard.IsKeyUp(k));
        }

        public static bool KeyJustReleased(Keys k)
        {
            return (keyboard.IsKeyUp(k) && previousKeyboard.IsKeyDown(k));
        }

        public static bool JustLeftClicked()
        {
            return (mouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released);
        }

        public static bool JustRightClicked()
        {
            return (mouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Released);
        }

        public static bool JustMiddleClicked()
        {
            return (mouse.MiddleButton == ButtonState.Pressed && previousMouse.MiddleButton == ButtonState.Released);
        }

        public static bool JustReleasedLeftClick()
        {
            return (mouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed);
        }

        public static bool JustReleasedRightClick()
        {
            return (mouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed);
        }

        public static bool JustReleasedMiddleClick()
        {
            return (mouse.MiddleButton == ButtonState.Released && previousMouse.MiddleButton == ButtonState.Pressed);
        }

        public static bool IsKeyDown(Keys k)
        {
            return keyboard.IsKeyDown(k);
        }

        public static bool IsKeyUp(Keys k)
        {
            return keyboard.IsKeyUp(k);
        }

        public static bool IsLeftClicking()
        {
            return mouse.LeftButton == ButtonState.Pressed;
        }

        public static bool IsRightClicking()
        {
            return mouse.RightButton == ButtonState.Pressed;
        }
    }
}
