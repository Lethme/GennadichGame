using System;

namespace GennadichGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GennadichGame(1200, 700))
                game.Run();
        }
    }
}
