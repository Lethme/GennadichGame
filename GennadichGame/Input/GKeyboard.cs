using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;

namespace GennadichGame.Input
{
    #region KeyboardEventDelegates
    public delegate void KeyboardEventHandler(KeyboardState state, IEnumerable<Keys> keys);
    #endregion
    public static class GKeyboard
    {
        #region Events
        public static event KeyboardEventHandler OnKeyDown;
        public static event KeyboardEventHandler OnKeyPressed;
        public static event KeyboardEventHandler OnKeyReleased;
        #endregion
        #region Data
        private static KeyboardState currentState;
        private static KeyboardState previousState;
        #endregion
        #region PublicMethods
        public static KeyboardState GetState()
        {
            previousState = currentState;
            currentState = Keyboard.GetState();
            return currentState;
        }
        public static void UpdateState()
        {
            previousState = currentState;
            currentState = Keyboard.GetState();

            if (OnKeyDown != null && GetDownKeys().Count() > 0) OnKeyDown.Invoke(currentState, GetDownKeys());
            if (OnKeyPressed != null && GetPressedKeys().Count() > 0) OnKeyPressed.Invoke(currentState, GetPressedKeys());
            if (OnKeyReleased != null && GetReleasedKeys().Count() > 0) OnKeyReleased.Invoke(currentState, GetReleasedKeys());
        }
        public static bool IsKeyDown(Keys key)
        {
            return currentState.IsKeyDown(key);
        }
        public static bool IsKeyPressed(Keys key)
        {
            return currentState.IsKeyDown(key) && !previousState.IsKeyDown(key);
        }
        public static bool IsKeyReleased(Keys key)
        {
            return !currentState.IsKeyDown(key) && previousState.IsKeyDown(key);
        }
        public static IEnumerable<Keys> GetPressedKeys()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (IsKeyPressed(key)) yield return key;
            }
        }
        public static IEnumerable<Keys> GetDownKeys()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (IsKeyDown(key)) yield return key;
            }
        }
        public static IEnumerable<Keys> GetReleasedKeys()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (IsKeyReleased(key)) yield return key;
            }
        }
        #endregion
        #region PublicMethods
        #endregion
    }
}
