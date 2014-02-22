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
        private static KeyboardState previousKeyboard;
        private static MouseState mouse;
        private static MouseState previousMouse;

        public static Random Random;
        public static Sprite MouseSprite;

        public static void Init() { Random = new Random(); MouseSprite = new Sprite("MouseSprite", "Pixel", Vector2.Zero); MouseSprite.Visible = true; }

        public static void Update(GameTime gameTime)
        {
            previousKeyboard = keyboard;
            previousMouse = mouse;
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();
            MouseSprite.Position = MousePos();
            MouseSprite.updateElement(gameTime);
        }

        public static Keys[] KeysPressed()
        {
            return keyboard.GetPressedKeys();
        }

        public static Vector2 MousePos()
        {
            return new Vector2(mouse.X, mouse.Y);
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

        public static bool JustUnLeftClicked()
        {
            return (mouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed);
        }

        public static bool JustUnRightClicked()
        {
            return (mouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed);
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
