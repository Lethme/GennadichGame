using System;
using System.Collections.Generic;
using System.Text;

namespace GennadichGame
{
    public abstract class GameModule
    {
        private static GennadichGame _game;
        protected static GennadichGame Game => _game;
        public static void Initialize(GennadichGame game)
        {
            if (game == null) throw new ArgumentNullException($"{nameof(game)} must not be null reference!");
            _game = game;
        }
    }
}
