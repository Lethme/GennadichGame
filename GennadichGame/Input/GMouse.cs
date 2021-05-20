using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using GennadichGame.Enums;
using System.Collections;
using System.Collections.Generic;

namespace GennadichGame.Input
{
    #region MouseEventDelegates
    public delegate void MouseEventHandler(MouseState state);
    public delegate void MouseMoveEventHandler(MouseState state, Direction direction);
    public delegate void MouseButtonEventHandler(MouseState state, IEnumerable<MouseButton> buttons);
    #endregion
    public static class GMouse
    {
        #region Data
        private static MouseState currentState = Mouse.GetState();
        private static MouseState previousState;
        private static Direction currentDirection = Direction.Up;
        private static Random Rnd { get; } = new Random();
        #endregion
        #region Properties
        public static bool AlkashCursor { get; set; } = false;
        public static Point Position => Mouse.GetState().Position;
        #endregion
        #region Events
        public static event MouseEventHandler OnLeftButtonDown;
        public static event MouseEventHandler OnLeftButtonPressed;
        public static event MouseEventHandler OnLeftButtonReleased;
        public static event MouseEventHandler OnRightButtonDown;
        public static event MouseEventHandler OnRightButtonPressed;
        public static event MouseEventHandler OnRightButtonReleased;
        public static event MouseEventHandler OnMiddleButtonDown;
        public static event MouseEventHandler OnMiddleButtonPressed;
        public static event MouseEventHandler OnMiddleButtonReleased;
        public static event MouseEventHandler OnXButton1Down;
        public static event MouseEventHandler OnXButton1Pressed;
        public static event MouseEventHandler OnXButton1Released;
        public static event MouseEventHandler OnXButton2Down;
        public static event MouseEventHandler OnXButton2Pressed;
        public static event MouseEventHandler OnXButton2Released;
        public static event MouseButtonEventHandler OnButtonDown;
        public static event MouseButtonEventHandler OnButtonPressed;
        public static event MouseButtonEventHandler OnButtonReleased;
        public static event MouseMoveEventHandler OnMouseMove;
        #endregion
        #region PublicMethods
        public static MouseState GetState()
        {
            previousState = currentState;
            currentState = Mouse.GetState();
            return currentState;
        }
        public static void UpdateState()
        {
            previousState = currentState;
            currentState = Mouse.GetState();

            if (OnLeftButtonDown != null && IsButtonDown(MouseButton.Left)) OnLeftButtonDown.Invoke(currentState);
            if (OnLeftButtonPressed != null && IsButtonPressed(MouseButton.Left)) OnLeftButtonPressed.Invoke(currentState);
            if (OnLeftButtonReleased != null && IsButtonReleased(MouseButton.Left)) OnLeftButtonReleased.Invoke(currentState);

            if (OnRightButtonDown != null && IsButtonDown(MouseButton.Right)) OnRightButtonDown.Invoke(currentState);
            if (OnRightButtonPressed != null && IsButtonPressed(MouseButton.Right)) OnRightButtonPressed.Invoke(currentState);
            if (OnRightButtonReleased != null && IsButtonReleased(MouseButton.Right)) OnRightButtonReleased.Invoke(currentState);

            if (OnMiddleButtonDown != null && IsButtonDown(MouseButton.Middle)) OnMiddleButtonDown.Invoke(currentState);
            if (OnMiddleButtonPressed != null && IsButtonPressed(MouseButton.Middle)) OnMiddleButtonPressed.Invoke(currentState);
            if (OnMiddleButtonReleased != null && IsButtonReleased(MouseButton.Middle)) OnMiddleButtonReleased.Invoke(currentState);

            if (OnXButton1Down != null && IsButtonDown(MouseButton.XButton1)) OnXButton1Down.Invoke(currentState);
            if (OnXButton1Pressed != null && IsButtonPressed(MouseButton.XButton1)) OnXButton1Pressed.Invoke(currentState);
            if (OnXButton1Released != null && IsButtonReleased(MouseButton.XButton1)) OnXButton1Released.Invoke(currentState);

            if (OnXButton2Down != null && IsButtonDown(MouseButton.XButton2)) OnXButton2Down.Invoke(currentState);
            if (OnXButton2Pressed != null && IsButtonPressed(MouseButton.XButton2)) OnXButton2Pressed.Invoke(currentState);
            if (OnXButton2Released != null && IsButtonReleased(MouseButton.XButton2)) OnXButton2Released.Invoke(currentState);

            if (OnButtonDown != null && IsButtonDown(MouseButton.Any)) OnButtonDown.Invoke(currentState, GetDownButtons());
            if (OnButtonPressed != null && IsButtonPressed(MouseButton.Any)) OnButtonPressed.Invoke(currentState, GetPressedButtons());
            if (OnButtonReleased != null && IsButtonReleased(MouseButton.Any)) OnButtonReleased.Invoke(currentState, GetReleasedButtons());

            if (OnMouseMove != null && IsMouseMoving()) OnMouseMove.Invoke(currentState, GetMoveDirection());

            if (AlkashCursor) Alkashize();
        }
        public static void MoveCursor(Direction direction, int pixels = 1)
        {
            var position = Position;
            switch (direction)
            {
                case Direction.Up:
                    {
                        SetPosition(position.X, position.Y - pixels);
                        break;
                    }
                case Direction.Down:
                    {
                        SetPosition(position.X, position.Y + pixels);
                        break;
                    }
                case Direction.Left:
                    {
                        SetPosition(position.X - pixels, position.Y);
                        break;
                    }
                case Direction.Right:
                    {
                        SetPosition(position.X + pixels, position.Y);
                        break;
                    }
                case Direction.UpLeft:
                    {
                        SetPosition(position.X - pixels, position.Y - pixels);
                        break;
                    }
                case Direction.UpRight:
                    {
                        SetPosition(position.X + pixels, position.Y - pixels);
                        break;
                    }
                case Direction.DownLeft:
                    {
                        SetPosition(position.X - pixels, position.Y + pixels);
                        break;
                    }
                case Direction.DownRight:
                    {
                        SetPosition(position.X + pixels, position.Y + pixels);
                        break;
                    }
            }
        }
        public static void SetPosition(Point position)
        {
            Mouse.SetPosition(position.X, position.Y);
        }
        public static void SetPosition(int x, int y)
        {
            Mouse.SetPosition(x, y);
        }
        public static Direction GetMoveDirection()
        {
            var posDiff = currentState.Position - previousState.Position;
            if (posDiff.X == 0 && posDiff.Y < 0) return Direction.Up;
            if (posDiff.X == 0 && posDiff.Y > 0) return Direction.Down;
            if (posDiff.X < 0 && posDiff.Y == 0) return Direction.Left;
            if (posDiff.X > 0 && posDiff.Y == 0) return Direction.Right;
            if (posDiff.X < 0 && posDiff.Y < 0) return Direction.UpLeft;
            if (posDiff.X > 0 && posDiff.Y < 0) return Direction.UpRight;
            if (posDiff.X < 0 && posDiff.Y > 0) return Direction.DownLeft;
            if (posDiff.X > 0 && posDiff.Y > 0) return Direction.DownRight;

            return Direction.None;
        }
        public static ButtonState GetButtonState(MouseButton button)
        {
            var mouseState = GetState();
            switch (button)
            {
                case MouseButton.Left: { return mouseState.LeftButton; }
                case MouseButton.Right: { return mouseState.RightButton; }
                case MouseButton.Middle: { return mouseState.MiddleButton; }
                case MouseButton.XButton1: { return mouseState.XButton1; }
                case MouseButton.XButton2: { return mouseState.XButton2; }
                default: return ButtonState.Released;
            }
        }
        public static ButtonState GetCurrentButtonState(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left: { return currentState.LeftButton; }
                case MouseButton.Right: { return currentState.RightButton; }
                case MouseButton.Middle: { return currentState.MiddleButton; }
                case MouseButton.XButton1: { return currentState.XButton1; }
                case MouseButton.XButton2: { return currentState.XButton2; }
                default: return ButtonState.Released;
            }
        }
        public static ButtonState GetPreviousButtonState(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left: { return previousState.LeftButton; }
                case MouseButton.Right: { return previousState.RightButton; }
                case MouseButton.Middle: { return previousState.MiddleButton; }
                case MouseButton.XButton1: { return previousState.XButton1; }
                case MouseButton.XButton2: { return previousState.XButton2; }
                default: return ButtonState.Released;
            }
        }
        public static IEnumerable<MouseButton> GetDownButtons()
        {
            foreach (MouseButton btn in Enum.GetValues(typeof(MouseButton)))
            {
                if (btn != MouseButton.Any && IsButtonDown(btn)) yield return btn;
            }
        }
        public static IEnumerable<MouseButton> GetPressedButtons()
        {
            foreach (MouseButton btn in Enum.GetValues(typeof(MouseButton)))
            {
                if (btn != MouseButton.Any && IsButtonPressed(btn)) yield return btn;
            }
        }
        public static IEnumerable<MouseButton> GetReleasedButtons()
        {
            foreach (MouseButton btn in Enum.GetValues(typeof(MouseButton)))
            {
                if (btn != MouseButton.Any && IsButtonReleased(btn)) yield return btn;
            }
        }
        public static bool IsButtonPressed(MouseButton button)
        {
            if (button == MouseButton.Any)
            {
                return CheckButtonState(btn => GetCurrentButtonState(btn) == ButtonState.Pressed && GetPreviousButtonState(btn) != ButtonState.Pressed);
            }

            return GetCurrentButtonState(button) == ButtonState.Pressed && GetPreviousButtonState(button) != ButtonState.Pressed;
        }
        public static bool IsButtonDown(MouseButton button)
        {
            if (button == MouseButton.Any)
            {
                return CheckButtonState(btn => GetCurrentButtonState(btn) == ButtonState.Pressed && !IsButtonPressed(btn));
            }

            return GetCurrentButtonState(button) == ButtonState.Pressed && !IsButtonPressed(button);
        }
        public static bool IsButtonReleased(MouseButton button)
        {
            if (button == MouseButton.Any)
            {
                return CheckButtonState(btn => GetCurrentButtonState(btn) != ButtonState.Pressed && GetPreviousButtonState(btn) == ButtonState.Pressed);
            }

            return GetCurrentButtonState(button) != ButtonState.Pressed && GetPreviousButtonState(button) == ButtonState.Pressed;
        }
        public static bool IsMouseMoving()
        {
            return currentState.Position != previousState.Position;
        }
        #endregion
        #region PrivateMethods
        private static void Alkashize()
        {
            currentDirection = (Direction)Rnd.Next(0, 8);
            MoveCursor(currentDirection, 2);
        }
        private static bool CheckButtonState(Func<MouseButton, bool> func)
        {
            foreach (MouseButton btn in Enum.GetValues(typeof(MouseButton)))
            {
                var flg = func.Invoke(btn);
                if (flg) return true;
            }

            return false;
        }
        #endregion
    }
}
