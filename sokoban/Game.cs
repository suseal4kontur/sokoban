using System;

namespace Sokoban
{
    public static class Game
    {
        public static void Main()
        {
            Map.Load("map2.txt");
            FunctionalItems.GetFunctionalItems();
            while (true)
            {
                Console.CursorLeft = 0;
                Console.CursorTop = 0;
                Map.Print();

                var keyInfo = Console.ReadKey(true);
                if (keyInfo.KeyChar == 'q')
                    break;
                if (keyInfo.Key == ConsoleKey.UpArrow
                    || keyInfo.Key == ConsoleKey.DownArrow
                    || keyInfo.Key == ConsoleKey.LeftArrow
                    || keyInfo.Key == ConsoleKey.RightArrow)
                    Movement.MovePlayer(keyInfo.Key);

                Map.Update();
            }
        }
    }
}
