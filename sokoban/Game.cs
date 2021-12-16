using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.Immutable;

namespace Sokoban
{
    public static class Game
    {
        public enum Keys
        {
            Up,
            Down,
            Left,
            Right,
            Exit,
            Choose
        }

        private static readonly ImmutableDictionary<Keys, ConsoleKey> KeyMap = new Dictionary<Keys, ConsoleKey>
        {
            { Keys.Up, ConsoleKey.UpArrow },
            { Keys.Down, ConsoleKey.DownArrow },
            { Keys.Left, ConsoleKey.LeftArrow },
            { Keys.Right, ConsoleKey.RightArrow },
            { Keys.Exit, ConsoleKey.Escape },
            { Keys.Choose, ConsoleKey.Enter },
        }.ToImmutableDictionary();

        private static readonly ImmutableArray<FileInfo> Maps = new DirectoryInfo("maps/").GetFiles().ToImmutableArray();

        public static void Main()
        {
            var index = 0;
            Console.CursorVisible = false;

            if (Maps.Length == 0)
                throw new FileNotFoundException("There are no maps in /maps");
            
            for (var i = Maps.Length - 1; i >= 0; i--)
                Map.Load(Maps[i].Name);

            PrintMenu();

            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == KeyMap[Keys.Exit])
                    return;
                if (keyInfo.Key == KeyMap[Keys.Left] && index > 0)
                    index--;
                else if (keyInfo.Key == KeyMap[Keys.Right] && index < Maps.Length - 1)
                    index++;
                else if (keyInfo.Key == KeyMap[Keys.Choose])
                    break;
                else
                    continue;
                Map.Load(Maps[index].Name);
                PrintMenu();
            }

            switch (PlayMap(index))
            {
                case 1:
                    ShowWinningScreen();
                    break;
                case -1:
                    ShowLosingScreen();
                    break;
            }
        }

        private static void PrintMenu()
        {
            var topText = "Choose a map:";
            var bottomText = $"{KeyMap[Keys.Left]} - previous, {KeyMap[Keys.Right]} - next," +
                            $"\n{KeyMap[Keys.Choose]} - choose, {KeyMap[Keys.Exit]} - exit";

            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.WriteLine(topText);

            while (Console.CursorTop != Console.WindowTop + Console.WindowHeight - 2)
            {
                while(Console.CursorLeft < Console.WindowWidth - 1)
                    Console.Write(" ");
                Console.WriteLine();
            }

            Console.CursorLeft = 0;
            Console.CursorTop = 1;
            Map.Print();

            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 2;
            Console.Write(bottomText);

            Console.CursorLeft = 0;
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 6;
        }

        private static void PrintGame(int index)
        {
            var topText = $"Map {index + 1}";
            var bottomText = $"{KeyMap[Keys.Up]} - up, {KeyMap[Keys.Down]} - down, {KeyMap[Keys.Left]} - left," +
                           $"\n{KeyMap[Keys.Right]} - right, {KeyMap[Keys.Exit]} - exit";

            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.WriteLine(topText);
            Map.Print();

            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 2;
            Console.Write(bottomText);

            Console.CursorLeft = 0;
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 6;
        }

        private static int PlayMap(int index)
        {
            Console.Clear();
            
            FunctionalItems.GetFunctionalItems();

            while (true)
            {
                PrintGame(index);

                var keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == KeyMap[Keys.Exit])
                    return 0;
                if (keyInfo.Key == KeyMap[Keys.Up])
                    Movement.MovePlayer(Keys.Up); 
                if (keyInfo.Key == KeyMap[Keys.Down])
                    Movement.MovePlayer(Keys.Down);
                if (keyInfo.Key == KeyMap[Keys.Left])
                    Movement.MovePlayer(Keys.Left);
                if (keyInfo.Key == KeyMap[Keys.Right])
                    Movement.MovePlayer(Keys.Right);

                Map.Update();
                if (AreLoosingConditionsMet())
                    return -1;
                if (AreWinningConditionsMet())
                    return 1;
            }
        }

        private static bool AreWinningConditionsMet()
        {
            var boxesCount = FunctionalItems.Boxes.Count;
            if (boxesCount == 0)
                throw new NullReferenceException("Functional items are not initialised");

            foreach (var box in FunctionalItems.Boxes)
            {
                if (box.OnLot != null)
                    boxesCount--;
            }

            return boxesCount == 0;
        }

        private static bool AreLoosingConditionsMet()
        {
            foreach (var thorns in FunctionalItems.Thorns)
            {
                if (thorns.IsPlayerOn(FunctionalItems.Player))
                    return true;
            }

            return false;
        }

        private static void ShowWinningScreen()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.WriteLine("WINNER!");
            Map.Print();
            Console.WriteLine("CONGRATULATIONS!");
            Console.ReadKey();
        }

        private static void ShowLosingScreen()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.WriteLine("You lost!");
            Map.Print();
            Console.WriteLine("What a shame.");
            Console.ReadKey();
        }
    }
}
