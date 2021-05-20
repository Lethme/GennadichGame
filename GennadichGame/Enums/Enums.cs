namespace GennadichGame.Enums
{
    public enum GameState
    {
        StartScreen,
        MainMenu,
        GameLobby,
        Game,
        Score
    }
    public enum Cursor
    {
        Arrow,
        Pointer,
        Dart
    }
    public enum BackgroundImage
    {
        None,
        Clouds
    }
    public enum Textures
    {
        Darts,
        ArrowCursor,
        PointerCursor,
        DartCursor,
        Background1
    }
    public enum AngleNormalizationFactor
    {
        PositiveOnly,
        AllowNegative
    }
    public enum ActionType
    {
        Update,
        Draw
    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight,
        None = -1,
    }
    public enum MouseButton
    {
        Left,
        Right,
        Middle,
        XButton1,
        XButton2,
        Any = -1
    }
}
