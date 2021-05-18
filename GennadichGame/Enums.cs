using System;
using System.Collections.Generic;
using System.Text;

namespace GennadichGame
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
        Pointer
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
}
