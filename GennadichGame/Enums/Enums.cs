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
        None,
        Arrow,
        Pointer,
        Dart
    }
    public enum BackgroundImage
    {
        None,
        Clouds,
        Evening
    }
    public enum Textures
    {
        Logo,
        Dart,
        Darts,
        ArrowCursor,
        PointerCursor,
        DartCursor,
        Background1,
        Background2
    }
    public enum Fonts
    {
        RegularConsolas8,
        RegularConsolas10,
        RegularConsolas12,
        RegularConsolas14,
        RegularConsolas16,
        RegularConsolas18,
        RegularConsolas20,
        RegularConsolas22,
        RegularConsolas24,
        RegularConsolas26,
        RegularConsolas28,
        RegularConsolas30,
        RegularConsolas32,
        RegularConsolas40,
        RegularConsolas48,
        RegularConsolas56,
        RegularConsolas64,
        RegularConsolas72,
        RegularConsolas80
    }
    public enum Position
    {
        None,
        Left,
        Right,
        Top,
        Bottom,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Center
    }
    public enum Align
    {
        Left,
        Right,
        Center
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
