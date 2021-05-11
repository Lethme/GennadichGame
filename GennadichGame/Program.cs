using System;

namespace GennadichGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var segmant = new GDartsSegment((5, 10), (0f, -15f));
            return;

            using (var game = new GennadichGame(1200, 700))
                game.Run();
        }
    }
}
