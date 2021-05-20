using System;
using Microsoft.Xna.Framework.Input;

namespace GennadichGame.Input
{
    public static class GKeyboard
    {
        static KeyboardState currentState;
        static KeyboardState previousState;
        public static KeyboardState GetState()
        {
            previousState = currentState;
            currentState = Keyboard.GetState();
            return currentState;
        }
        public static bool IsPressed(Keys key)
        {
            return currentState.IsKeyDown(key);
        }
        public static bool HasBeenPressed(Keys key)
        {
            return currentState.IsKeyDown(key) && !previousState.IsKeyDown(key);
        }
    }
}
