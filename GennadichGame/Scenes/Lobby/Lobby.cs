using System;
using System.Collections.Generic;

namespace GennadichGame.Scenes.Lobby
{
    public sealed class Lobby
    {
        private static int _count = 0;
        public static int MaxSize => 3;
        public String Name { get; set; } = $"Game lobby #{_count}";
        public List<Player> Players { get; } = new List<Player>();
        public int PlayersCount => Players.Count;
        public Lobby() { _count++; }
        public Lobby(params Player[] players)
        {
            if (players.Length > MaxSize) throw new ArgumentException($"Attempted to add more than {MaxSize} players.");

            foreach (var player in players) Players.Add(player);
            _count++;
        }
        ~Lobby() { _count--; }
    }
}
