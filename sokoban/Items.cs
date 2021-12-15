using System;

namespace Sokoban
{
    public abstract class Item : IItem
    {
        private int x;
        private int y;

        public Item(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X
        {
            get => x;
            set
            {
                if (value < 0 || value > Map.MapWidth - 1)
                    throw new IndexOutOfRangeException();
                x = value;
            }
        }

        public int Y
        {
            get => y;
            set
            {
                if (value < 0 || value > Map.MapHeight - 1)
                    throw new IndexOutOfRangeException();
                y = value;
            }
        }

        public abstract char GetChar();
        public abstract Map.Item GetItem();
    }

    public abstract class MovableItem : Item
    {
        public MovableItem(int x, int y) : base(x, y) { }

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

        public override char GetChar() => OnLot == null ? 'p' : 'P';

        public override Map.Item GetItem() => OnLot == null ? Map.Item.Player : Map.Item.PlayerOnLot;

    }

    public class Lot : Item
    {
        public Lot(int x, int y) : base(x, y) { }

        public bool IsItemOn { get; private set; }

        public override char GetChar() => '+';

        public override Map.Item GetItem() => Map.Item.Lot;

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
        public override char GetChar() => OnLot == null ? 'o' : 'O';

        public override Map.Item GetItem() => OnLot == null ? Map.Item.Box : Map.Item.BoxOnLot;
    }

    public class Wall : Item
    {
        public Wall(int x, int y) : base(x, y) { }
        public override char GetChar() => '#';

        public override Map.Item GetItem() => Map.Item.Wall;
    }
}
