using System;
using System.Collections.Generic;

namespace Sokoban
{
    public static class FunctionalItems
    {
        public static Player Player;
        public static List<Box> Boxes = new List<Box>();
        public static List<Lot> Lots = new List<Lot>();

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
    }
}
