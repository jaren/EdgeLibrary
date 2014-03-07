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

        private static Random random;
        private static int previousGiven;
        public static Sprite MouseSprite;
        public static Vector2 MousePosition;

        public static void Init() { random = new Random(); MouseSprite = new Sprite("MouseSprite", "Pixel", Vector2.Zero); MouseSprite.DrawLayer = 100; MouseSprite.Visible = false; previousGiven = 0; }

        public static void Update(GameTime gameTime)
        {
            previousKeyboard = keyboard;
            previousMouse = mouse;
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();
            MouseSprite.Position = getMousePos();
            MousePosition = getMousePos();
            MouseSprite.Update(gameTime);
        }

        //Used for Vector2, etc. when two random numbers need to be generated at the same time
        public static int AccurateRandomInt(int min, int max)
        {
            //Checks if previous call was the same as this one
           if(previousGiven == RandomInt(min, max))
           {
               //Chooses whether to check for a lower or higher number first
               if (RandomInt(1, 3) == 2)
               {
                   //Checks if lower or higher number is possible
                   if (previousGiven + 1 >= min && previousGiven + 1 <= max)
                   {
                       return previousGiven += 1;
                   }
                   else if (previousGiven - 1 >= min && previousGiven - 1 <= max)
                   {
                       return previousGiven -= 1;
                   }
               }
               else
               {
                   //Checks if lower or higher number is possible
                   if (previousGiven - 1 >= min && previousGiven - 1 <= max)
                   {
                       return previousGiven -= 1;
                   }
                   else if (previousGiven + 1 >= min && previousGiven + 1 <= max)
                   {
                       return previousGiven += 1;
                   }
               }
           }
           previousGiven = RandomInt(min, max);
           return previousGiven;
        }
        public static int RandomInt(int min, int max)
        {
            return random.Next(min, max);
        }
        public static int RandomInt(int max)
        {
            return random.Next(max);
        }
        public static int RandomInt()
        {
            return random.Next();
        }
        public static float RandomFloat(float min, float max)
        {
            if (min + 1 > max)
            {
                return (float)MathHelper.Lerp(min, max, 0.5f);
            }
            else
            {
                return RandomInt((int)min, (int)max - 1) + (float)RandomDouble();
            }
        }
        public static double RandomDouble()
        {
            return random.NextDouble();
        }

        public static Keys[] KeysPressed()
        {
            return keyboard.GetPressedKeys();
        }

        private static Vector2 getMousePos()
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
