using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using GennadichGame.Enums;

namespace GennadichGame.Input
{
    public delegate void MouseEventHandler(MouseState state);
    public delegate void MouseMoveEventHandler(MouseState state, Direction direction);
    public static class GMouse
    {
        private static MouseState currentState = Mouse.GetState();
        private static MouseState previousState;
        private static Direction currentDirection = Direction.Up;
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
        public static event MouseMoveEventHandler OnMouseMove;
        private static Random Rnd { get; } = new Random();
        public static bool AlkashCursor { get; set; } = false;
        public static Point Position => Mouse.GetState().Position;
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

            if (OnLeftButtonDown != null && IsLeftButtonDown()) OnLeftButtonDown.Invoke(currentState);
            if (OnLeftButtonPressed != null && IsLeftButtonPressed()) OnLeftButtonPressed.Invoke(currentState);
            if (OnLeftButtonReleased != null && currentState.LeftButton != ButtonState.Pressed && previousState.LeftButton == ButtonState.Pressed) OnLeftButtonReleased.Invoke(currentState);

            if (OnRightButtonDown != null && IsRightButtonDown()) OnRightButtonDown.Invoke(currentState);
            if (OnRightButtonPressed != null && IsRightButtonPressed()) OnRightButtonPressed.Invoke(currentState);
            if (OnRightButtonReleased != null && currentState.RightButton != ButtonState.Pressed && previousState.RightButton == ButtonState.Pressed) OnRightButtonReleased.Invoke(currentState);

            if (OnMiddleButtonDown != null && IsMiddleButtonDown()) OnMiddleButtonDown.Invoke(currentState);
            if (OnMiddleButtonPressed != null && IsMiddleButtonPressed()) OnMiddleButtonPressed.Invoke(currentState);
            if (OnMiddleButtonReleased != null && currentState.MiddleButton != ButtonState.Pressed && previousState.MiddleButton == ButtonState.Pressed) OnMiddleButtonReleased.Invoke(currentState);

            if (OnXButton1Down != null && IsXButton1Down()) OnXButton1Down.Invoke(currentState);
            if (OnXButton1Pressed != null && IsXButton1Pressed()) OnXButton1Pressed.Invoke(currentState);
            if (OnXButton1Released != null && currentState.XButton1 != ButtonState.Pressed && previousState.XButton1 == ButtonState.Pressed) OnXButton1Released.Invoke(currentState);

            if (OnXButton2Down != null && IsXButton2Down()) OnXButton2Down.Invoke(currentState);
            if (OnXButton2Pressed != null && IsXButton2Pressed()) OnXButton2Pressed.Invoke(currentState);
            if (OnXButton2Released != null && currentState.XButton2 != ButtonState.Pressed && previousState.XButton2 == ButtonState.Pressed) OnXButton2Released.Invoke(currentState);

            if (OnMouseMove != null && IsMouseMoving()) OnMouseMove.Invoke(currentState, GetMouseMoveDirection());

            if (AlkashCursor) Alkashize();
        }
        private static void Alkashize()
        {
            currentDirection = (Direction)Rnd.Next(0, 8);
            MoveCursor(currentDirection, 2);
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
        public static Direction GetMouseMoveDirection()
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
        public static bool IsMouseMoving()
        {
            return currentState.Position != previousState.Position;
        }
        public static bool IsLeftButtonDown()
        {
            return currentState.LeftButton == ButtonState.Pressed && !IsLeftButtonPressed();
        }
        public static bool IsLeftButtonPressed()
        {
            return currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton != ButtonState.Pressed;
        }
        public static bool IsRightButtonDown()
        {
            return currentState.RightButton == ButtonState.Pressed && !IsRightButtonPressed();
        }
        public static bool IsRightButtonPressed()
        {
            return currentState.RightButton == ButtonState.Pressed && previousState.RightButton != ButtonState.Pressed;
        }
        public static bool IsMiddleButtonDown()
        {
            return currentState.MiddleButton == ButtonState.Pressed && !IsMiddleButtonPressed();
        }
        public static bool IsMiddleButtonPressed()
        {
            return currentState.MiddleButton == ButtonState.Pressed && previousState.MiddleButton != ButtonState.Pressed;
        }
        public static bool IsXButton1Down()
        {
            return currentState.XButton1 == ButtonState.Pressed && !IsXButton1Pressed();
        }
        public static bool IsXButton1Pressed()
        {
            return currentState.XButton1 == ButtonState.Pressed && previousState.XButton1 != ButtonState.Pressed;
        }
        public static bool IsXButton2Down()
        {
            return currentState.XButton2 == ButtonState.Pressed;
        }
        public static bool IsXButton2Pressed()
        {
            return currentState.XButton2 == ButtonState.Pressed && previousState.XButton2 != ButtonState.Pressed;
        }
    }
}
