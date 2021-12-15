using System;

namespace Sokoban
{
    public abstract class FunctionalItem : IFunctionalItem
    {
        private int x;
        private int y;

        public FunctionalItem(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X
        {
            get => x;
            set
            {
                if (value < 0 || value > Map.Width - 1)
                    throw new IndexOutOfRangeException();
                x = value;
            }
        }

        public int Y
        {
            get => y;
            set
            {
                if (value < 0 || value > Map.Height - 1)
                    throw new IndexOutOfRangeException();
                y = value;
            }
        }

        public abstract Map.ItemName GetItemName();
    }

    public abstract class MovableItem : FunctionalItem
    {
        private int previousX;
        private int previousY;

        public int PreviousX
        {
            get => previousX;
            set
            {
                if (value < 0 || value > Map.Width - 1)
                    throw new IndexOutOfRangeException();
                previousX = value;
            }
        }

        public int PreviousY
        {
            get => previousY;
            set
            {
                if (value < 0 || value > Map.Height - 1)
                    throw new IndexOutOfRangeException();
                previousY = value;
            }
        }
        public MovableItem(int x, int y) : base(x, y) 
        {
            PreviousX = x;
            PreviousY = y;
        }

        public Lot OnLot { get; private set; }

        public void MoveOnLot(Lot lot)
        {
            if (lot == null)
                throw new ArgumentNullException();
            lot.PutItemOn(this);
            OnLot = lot;
        }

        public void MoveOffLot()
        {
            if (OnLot == null)
                throw new NullReferenceException();
            OnLot.PutItemOff(this);
            OnLot = null;
        }
    }

    public class Player : MovableItem
    {
        public Player(int x, int y) : base(x, y) { }

        public override Map.ItemName GetItemName() => OnLot == null ? Map.ItemName.Player : Map.ItemName.PlayerOnLot;

    }

    public class Lot : FunctionalItem
    {
        public Lot(int x, int y) : base(x, y) { }

        public bool IsItemOn { get; private set; }

        public override Map.ItemName GetItemName() => Map.ItemName.Lot;

        public void PutItemOn(MovableItem item)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (item.X == X && item.Y == Y)
                IsItemOn = true;
            else
                throw new ArgumentException($"The item {item} is not on the lot");
        }

        public void PutItemOff(MovableItem item)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (item.X != X || item.Y != Y)
                IsItemOn = false;
            else
                throw new ArgumentException($"The item {item} is still on the lot");
        }
    }

    public class Box : MovableItem
    {
        public Box(int x, int y) : base(x, y) { }

        public override Map.ItemName GetItemName() => OnLot == null ? Map.ItemName.Box : Map.ItemName.BoxOnLot;
    }
}
