using System;

namespace Sokoban
{
    public static class Movement
    {
        public static void MovePlayer(ConsoleKey key)
        {
            if (FunctionalItems.Player == null
                || FunctionalItems.Boxes.Count == 0
                || FunctionalItems.Lots.Count == 0)
                throw new NullReferenceException("Functional items are not initialised");

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

        private static int[] GetTargetCoords(int currentX, int currentY, ConsoleKey key)
        {
            var targetCoords = new int[] { currentX, currentY };
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    targetCoords[1]--;
                    break;
                case ConsoleKey.DownArrow:
                    targetCoords[1]++;
                    break;
                case ConsoleKey.LeftArrow:
                    targetCoords[0]--;
                    break;
                case ConsoleKey.RightArrow:
                    targetCoords[0]++;
                    break;
                default:
                    throw new ArgumentException("The key is not an arrow key");
            }
            return targetCoords;
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
    }
}
