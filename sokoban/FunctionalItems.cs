using System;
using System.Collections.Generic;

namespace Sokoban
{
    public static class FunctionalItems
    {
        public static Player Player { get; private set; }
        public static List<Box> Boxes { get; private set; }
        public static List<Lot> Lots { get; private set; }
        public static List<Thorns> Thorns { get; private set; }

        public static void GetFunctionalItems()
        {
            Lot lot;
            Box box;

            ClearFunctionalItems();

            for (var x = 0; x < Map.Width; x++)
            {
                for (var y = 0; y < Map.Height; y++)
                {
                    switch (Map.ReadItem(x, y))
                    {
                        case Map.ItemName.Player:
                            Player = new Player(x, y);
                            break;
                        case Map.ItemName.PlayerOnLot:
                            Player = new Player(x, y);
                            lot = new Lot(x, y);
                            Player.MoveOnLot(lot);
                            Lots.Add(lot);
                            break;
                        case Map.ItemName.Box:
                            Boxes.Add(new Box(x, y));
                            break;
                        case Map.ItemName.BoxOnLot:
                            box = new Box(x, y);
                            lot = new Lot(x, y);
                            box.MoveOnLot(lot);
                            Boxes.Add(box);
                            Lots.Add(lot);
                            break;
                        case Map.ItemName.Lot:
                            Lots.Add(new Lot(x, y));
                            break;
                        case Map.ItemName.Thorns:
                            Thorns.Add(new Thorns(x, y));
                            break;
                    }
                }
            }
        }

        public static void ClearFunctionalItems()
        {
            Player = null;
            Boxes = new List<Box>();
            Lots = new List<Lot>();
            Thorns = new List<Thorns>();
        }

        public static Box GetBox(int x, int y)
        {
            foreach (var box in Boxes)
            {
                if (box.X == x && box.Y == y)
                    return box;
            }

            throw new ArgumentException($"No box found with coords ({x},{y})");
        }

        public static Lot GetLot(int x, int y)
        {
            foreach (var lot in Lots)
            {
                if (lot.X == x && lot.Y == y)
                    return lot;
            }

            throw new ArgumentException($"No lot found with coords ({x},{y})");
        }

        public static Thorns GetThorns(int x, int y)
        {
            foreach (var thorns in Thorns)
            {
                if (thorns.X == x && thorns.Y == y)
                    return thorns;
            }

            throw new ArgumentException($"No thorns found with coords ({x},{y})");
        }
    }
}
