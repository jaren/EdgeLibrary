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
    public static class InputManager
    {
        private static KeyboardState keyboard;
        public static KeyboardState previousKeyboard { get; private set; }
        private static MouseState mouse;

        public static Random Random;
        public static Element MouseElement;

        public static void Init() { Random = new Random(); MouseElement = new Element(false); }

        public static void Update()
        {
            previousKeyboard = keyboard;
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();
            MouseElement.Position = MousePos();
        }

        public static Vector2 MousePos()
        {
            return new Vector2(mouse.X, mouse.Y);
        }

        public static bool IsKeyDown(Keys k)
        {
            return keyboard.IsKeyDown(k);
        }

        public static bool IsKeyUp(Keys k)
        {
            return keyboard.IsKeyUp(k);
        }

        public static bool LeftClick()
        {
            return mouse.LeftButton == ButtonState.Pressed;
        }

        public static bool RightClick()
        {
            return mouse.RightButton == ButtonState.Pressed;
        }
    }
}
