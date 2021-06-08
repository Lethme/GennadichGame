using System;
using System.Collections.Generic;
using System.Text;

namespace GennadichGame.Controls
{
    public abstract class Control
    {
        private static GennadichGame _game;
        public static GennadichGame Game => _game;
        public static void Initialize(GennadichGame game)
        {
            if (game == null) throw new ArgumentNullException($"{nameof(game)} must not be null reference!");
            _game = game;
        }
    }
}
