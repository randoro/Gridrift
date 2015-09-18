using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Gridrift.GUIs
{
    static class KeyMouseReader
    {
        public static KeyboardState keyState, oldKeyState = Keyboard.GetState();
        public static MouseState mouseState, oldMouseState = Mouse.GetState();


        public static bool KeyPressed(Keys key)
        {
            return keyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key);
        }

        public static bool KeyHold(Keys key)
        {
            return keyState.IsKeyDown(key); //&& oldKeyState.IsKeyUp(key);
        }

        public static bool LeftClick()
        {
            return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
        }
        public static bool LeftHold()
        {
            return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed;
        }


        public static bool RightHold()
        {
            return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Pressed;
        }
        public static bool RightClick()
        {
            return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
        }

        public static Point MousePosition()
        {
            return new Point(mouseState.X, mouseState.Y);
        }


         //public static bool HoldClick()
         //{
         //    return mouseState.RightButton == ButtonState.Pressed &&  mouseState.LeftButton == ButtonState.Pressed;
         //}

        //Should be called at beginning of Update in Game
        public static void update(GameTime gameTime)
        {
            oldKeyState = keyState;
            keyState = Keyboard.GetState();
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();
        }
    }
}
