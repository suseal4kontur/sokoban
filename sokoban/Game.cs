using System;

namespace Sokoban
{
    public static class Game
    {
        public static void Main()
        {
            Map.Load("map1.txt");
            GetFunctionalItems();
            while (true)
            {
                Console.Clear();
                Map.Print();

                var keyInfo = Console.ReadKey(true);
                if (keyInfo.KeyChar == 'q')
                    break;
                if (keyInfo.Key == ConsoleKey.UpArrow
                    || keyInfo.Key == ConsoleKey.DownArrow
                    || keyInfo.Key == ConsoleKey.LeftArrow
                    || keyInfo.Key == ConsoleKey.RightArrow)
                    MovePlayer(keyInfo.Key);

                RewriteMap();
            }

        }

        private static void GetFunctionalItems()
        {
            Lot lot;
            Box box;

            for (var x = 0; x < Map.Width; x++)
            {
                for (var y = 0; y < Map.Height; y++)
                {
                    switch (Map.ReadItem(x, y))
                    {
                        case Map.ItemName.Player:
                            FunctionalItems.Player = new Player(x, y);
                            break;
                        case Map.ItemName.PlayerOnLot:
                            FunctionalItems.Player = new Player(x, y);
                            lot = new Lot(x, y);
                            FunctionalItems.Player.MoveOnLot(lot);
                            FunctionalItems.Lots.Add(lot);
                            break;
                        case Map.ItemName.Box:
                            FunctionalItems.Boxes.Add(new Box(x, y));
                            break;
                        case Map.ItemName.BoxOnLot:
                            box = new Box(x, y);
                            lot = new Lot(x, y);
                            box.MoveOnLot(lot);
                            FunctionalItems.Boxes.Add(box);
                            FunctionalItems.Lots.Add(lot);
                            break;
                        case Map.ItemName.Lot:
                            FunctionalItems.Lots.Add(new Lot(x, y));
                            break;
                    }
                }
            }
        }

        private static int[] GetTargetCoords(int currentX, int currentY, ConsoleKey key)
        {
            var targetCoords = new int[2];
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    targetCoords[0] = currentX;
                    targetCoords[1] = currentY - 1;
                    break;
                case ConsoleKey.DownArrow:
                    targetCoords[0] = currentX;
                    targetCoords[1] = currentY + 1;
                    break;
                case ConsoleKey.LeftArrow:
                    targetCoords[0] = currentX - 1;
                    targetCoords[1] = currentY;
                    break;
                case ConsoleKey.RightArrow:
                    targetCoords[0] = currentX + 1;
                    targetCoords[1] = currentY;
                    break;
            }
            return targetCoords;
        }

        private static void MovePlayer(ConsoleKey key)
        {
            var targetCoords = GetTargetCoords(FunctionalItems.Player.X,
                                               FunctionalItems.Player.Y,
                                               key);
            var targetedItem = Map.ReadItem(targetCoords[0], targetCoords[1]);

            switch (targetedItem)
            {
                case Map.ItemName.Empty:
                    MovePlayer(targetCoords);
                    break;
                case Map.ItemName.Lot:
                    MovePlayerOnLot(targetCoords);
                    break;
                case Map.ItemName.Box:
                    if (MoveBox(targetCoords, key))
                        MovePlayer(targetCoords);
                    break;
                case Map.ItemName.BoxOnLot:
                    if (MoveBox(targetCoords, key))
                        MovePlayerOnLot(targetCoords);
                    break;
            }
        }

        private static void MovePlayer(int[] coords)
        {
            FunctionalItems.Player.X = coords[0];
            FunctionalItems.Player.Y = coords[1];
            if (FunctionalItems.Player.OnLot != null)
                FunctionalItems.Player.MoveOffLot();
        }

        private static void MovePlayerOnLot(int[] coords)
        {
            MovePlayer(coords);
            var lot = FunctionalItems.GetLot(coords[0], coords[1]);
            FunctionalItems.Player.MoveOnLot(lot);
        }

        private static bool MoveBox(int[] currentCoords, ConsoleKey key)
        {
            var box = FunctionalItems.GetBox(currentCoords[0], currentCoords[1]);
            var targetCoords = GetTargetCoords(box.X, box.Y, key);
            var targetedItem = Map.ReadItem(targetCoords[0], targetCoords[1]);

            switch (targetedItem)
            {
                case Map.ItemName.Empty:
                    MoveBox(box, targetCoords);
                    return true;
                case Map.ItemName.Lot:
                    MoveBoxOnLot(box, targetCoords);
                    return true;
                default:
                    return false;
            }
        }

        private static void MoveBox(Box box, int[] coords)
        {
            box.X = coords[0];
            box.Y = coords[1];
            if (box.OnLot != null)
                box.MoveOffLot();
        }

        private static void MoveBoxOnLot(Box box, int[] coords)
        {
            MoveBox(box, coords);
            var lot = FunctionalItems.GetLot(coords[0], coords[1]);
            box.MoveOnLot(lot);
        }

        private static void RewriteMap()
        {
            foreach (var box in FunctionalItems.Boxes)
            {
                EraseItemIfMoved(box);
                RewriteItem(box);
            }

            EraseItemIfMoved(FunctionalItems.Player);
            RewriteItem(FunctionalItems.Player);

            foreach (var lot in FunctionalItems.Lots)
            {
                if (!lot.IsItemOn)
                    RewriteItem(lot);
            }
        }

        private static void EraseItemIfMoved(MovableItem item)
        {
            if (item.X != item.PreviousX || item.Y != item.PreviousY)
            {
                Map.WriteItem(item.PreviousX, item.PreviousY, Map.ItemName.Empty);
                item.PreviousX = item.X;
                item.PreviousY = item.Y;
            }
        }

        private static void RewriteItem(FunctionalItem item)
        {
            Map.WriteItem(item.X, item.Y, item.GetItemName());
        }
    }
}
