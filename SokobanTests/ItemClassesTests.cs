using NUnit.Framework;
using FluentAssertions;
using System;

namespace SokobanTests
{
    [TestFixture]
    public static class ItemClassesTests
    {
        [TestCase(-1, 0, "testmap1.txt")]
        [TestCase(7, 0, "testmap1.txt")]
        [TestCase(0, -2, "testmap1.txt")]
        [TestCase(0, 6, "testmap1.txt")]
        public static void ItemWrongCoordsTest(int x, int y, string fileName)
        {
            Sokoban.Map.Load(fileName);
            Action action = () => new Sokoban.Player(x, y);
            action.Should().Throw<IndexOutOfRangeException>();
        }

        [Test]
        public static void MovableItemMoveOnLotNullTest()
        {
            Sokoban.Map.Load("testmap1.txt");
            var player = new Sokoban.Player(1, 1);
            Sokoban.Lot lot = null;
            Action action = () => player.MoveOnLot(lot);
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public static void MovableItemMoveOffLotNullTest()
        {
            Sokoban.Map.Load("testmap1.txt");
            var player = new Sokoban.Player(1, 1);
            Action action = () => player.MoveOffLot();
            action.Should().Throw<NullReferenceException>();
        }

        [Test]
        public static void LotPutItemOnNullTest()
        {
            Sokoban.Map.Load("testmap1.txt");
            Sokoban.Player player = null;
            var lot = new Sokoban.Lot(1, 1);
            Action action = () => lot.PutItemOn(player);
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public static void LotPutItemOffNullTest()
        {
            Sokoban.Map.Load("testmap1.txt");
            Sokoban.Player player = null;
            var lot = new Sokoban.Lot(1, 1);
            Action action = () => lot.PutItemOff(player);
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public static void LotPutItemOnItemNotOnLotTest()
        {
            Sokoban.Map.Load("testmap1.txt");
            var player = new Sokoban.Player(1, 2);
            var lot = new Sokoban.Lot(1, 1);
            Action action = () => lot.PutItemOn(player);
            action.Should().Throw<ArgumentException>().WithMessage($"The item {player} is not on the lot");
        }

        [Test]
        public static void LotPutItemOffItemOnLotTest()
        {
            Sokoban.Map.Load("testmap1.txt");
            var player = new Sokoban.Player(1, 1);
            var lot = new Sokoban.Lot(1, 1);
            Action action = () => lot.PutItemOff(player);
            action.Should().Throw<ArgumentException>().WithMessage($"The item {player} is still on the lot");
        }

        [Test]
        public static void LotPutItemOnTest()
        {
            Sokoban.Map.Load("testmap1.txt");
            var player = new Sokoban.Player(1, 1);
            var lot = new Sokoban.Lot(1, 1);
            lot.PutItemOn(player);
            lot.IsItemOn.Should().BeTrue();
        }

        [Test]
        public static void LotPutItemOffTest()
        {
            Sokoban.Map.Load("testmap1.txt");
            var player = new Sokoban.Player(1, 1);
            var lot = new Sokoban.Lot(1, 1);
            lot.PutItemOn(player);
            player.X = 2;
            lot.PutItemOff(player);
            lot.IsItemOn.Should().BeFalse();
        }

        [Test]
        public static void MovableItemMoveOnLotTest()
        {
            Sokoban.Map.Load("testmap1.txt");
            var player = new Sokoban.Player(1, 1);
            var lot = new Sokoban.Lot(1, 1);
            player.MoveOnLot(lot);
            player.OnLot.Should().BeSameAs(lot);
        }

        [Test]
        public static void MovableItemMoveOffLotTest()
        {
            Sokoban.Map.Load("testmap1.txt");
            var player = new Sokoban.Player(1, 1);
            var lot = new Sokoban.Lot(1, 1);
            player.MoveOnLot(lot);
            lot.X = 2;
            player.MoveOffLot();
            player.OnLot.Should().BeNull();
        }
    }
}
