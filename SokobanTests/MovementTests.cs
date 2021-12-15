using NUnit.Framework;
using FluentAssertions;
using System;

namespace SokobanTests
{
    [TestFixture]
    public static class MovementTests
    {
        [Test]
        public static void MovePlayerNullTest()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap11.txt");
            Sokoban.FunctionalItems.ClearFunctionalItems();
            Action action = () => Sokoban.Movement.MovePlayer(ConsoleKey.UpArrow);
            action.Should().Throw<NullReferenceException>();
        }

        [Test]
        public static void MovePlayerWrongKeyTest()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap11.txt");
            Sokoban.FunctionalItems.GetFunctionalItems();
            Action action = () => Sokoban.Movement.MovePlayer(ConsoleKey.Enter);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public static void MovePlayerIntoWallTest()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap11.txt");
            Sokoban.FunctionalItems.GetFunctionalItems();
            Sokoban.Movement.MovePlayer(ConsoleKey.UpArrow);
            Sokoban.FunctionalItems.Player.Y.Should().Be(1);
        }

        [Test]
        public static void MovePlayerToEmptyTest()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap11.txt");
            Sokoban.FunctionalItems.GetFunctionalItems();
            Sokoban.Movement.MovePlayer(ConsoleKey.LeftArrow);
            Sokoban.FunctionalItems.Player.X.Should().Be(1);
        }

        [Test]
        public static void MovePlayerIntoBoxThatCanBeMovedTest()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap11.txt");
            Sokoban.FunctionalItems.GetFunctionalItems();
            Sokoban.Movement.MovePlayer(ConsoleKey.LeftArrow);
            Sokoban.Movement.MovePlayer(ConsoleKey.DownArrow);
            Sokoban.Movement.MovePlayer(ConsoleKey.DownArrow);
            Sokoban.Movement.MovePlayer(ConsoleKey.DownArrow);
            Sokoban.Movement.MovePlayer(ConsoleKey.RightArrow);
            Sokoban.FunctionalItems.Player.X.Should().Be(2);
            Action action = () => Sokoban.FunctionalItems.GetBox(3, 4);
            action.Should().NotThrow();
        }
        [Test]
        public static void MovePlayerIntoBoxNextToWall()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap11.txt");
            Sokoban.FunctionalItems.GetFunctionalItems();
            Sokoban.Movement.MovePlayer(ConsoleKey.DownArrow);
            Sokoban.Movement.MovePlayer(ConsoleKey.DownArrow);
            Sokoban.Movement.MovePlayer(ConsoleKey.DownArrow);
            Sokoban.FunctionalItems.Player.Y.Should().Be(3);
            Action action = () => Sokoban.FunctionalItems.GetBox(2, 4);
            action.Should().NotThrow();
        }

        [Test]
        public static void MovePlayerIntoBoxNextToBoxTest()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap15.txt");
            Sokoban.FunctionalItems.GetFunctionalItems();
            Sokoban.Movement.MovePlayer(ConsoleKey.RightArrow);
            Sokoban.Movement.MovePlayer(ConsoleKey.DownArrow);
            Sokoban.Movement.MovePlayer(ConsoleKey.DownArrow);
            Sokoban.Movement.MovePlayer(ConsoleKey.RightArrow);
            Sokoban.Movement.MovePlayer(ConsoleKey.UpArrow);
            Sokoban.FunctionalItems.Player.Y.Should().Be(3);
            Action action = () => Sokoban.FunctionalItems.GetBox(3, 2);
            action.Should().NotThrow();
        }

        [Test]
        public static void MovePlayerOffLotTest()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap15.txt");
            Sokoban.FunctionalItems.GetFunctionalItems();
            var lot = Sokoban.FunctionalItems.Player.OnLot;
            Sokoban.Movement.MovePlayer(ConsoleKey.RightArrow);
            Sokoban.FunctionalItems.Player.X.Should().Be(2);
            Sokoban.FunctionalItems.Player.OnLot.Should().BeNull();
            lot.X.Should().Be(1);
            lot.IsItemOn.Should().BeFalse();
        }

        [Test]
        public static void MovePlayerOnLotTest()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap12.txt");
            Sokoban.FunctionalItems.GetFunctionalItems();
            Sokoban.Movement.MovePlayer(ConsoleKey.LeftArrow);
            Sokoban.Movement.MovePlayer(ConsoleKey.LeftArrow);
            Sokoban.Movement.MovePlayer(ConsoleKey.DownArrow);
            Sokoban.FunctionalItems.Player.Y.Should().Be(3);
            var lot = Sokoban.FunctionalItems.GetLot(1, 3);
            Sokoban.FunctionalItems.Player.OnLot.Should().BeSameAs(lot);
            lot.Y.Should().Be(3);
            lot.IsItemOn.Should().BeTrue();
        }

        [Test]
        public static void MoveBoxOnLotTest()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap1.txt");
            Sokoban.FunctionalItems.GetFunctionalItems();
            Sokoban.Movement.MovePlayer(ConsoleKey.RightArrow);
            Sokoban.Movement.MovePlayer(ConsoleKey.RightArrow);
            Sokoban.FunctionalItems.Player.X.Should().Be(3);
            Action action = () => Sokoban.FunctionalItems.GetBox(4, 1);
            action.Should().NotThrow();
            var box = Sokoban.FunctionalItems.GetBox(4, 1);
            var lot = Sokoban.FunctionalItems.GetLot(4, 1);
            box.OnLot.Should().BeSameAs(lot);
            lot.IsItemOn.Should().BeTrue();
        }

        [Test]
        public static void MoveBoxOffLotTest()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap16.txt");
            Sokoban.FunctionalItems.GetFunctionalItems();
            Sokoban.Movement.MovePlayer(ConsoleKey.RightArrow);
            Sokoban.FunctionalItems.Player.X.Should().Be(2);
            Action action = () => Sokoban.FunctionalItems.GetBox(3, 1);
            action.Should().NotThrow();
            var box = Sokoban.FunctionalItems.GetBox(3, 1);
            var lot = Sokoban.FunctionalItems.GetLot(2, 1);
            box.OnLot.Should().BeNull();
            lot.IsItemOn.Should().BeTrue();
            Sokoban.FunctionalItems.Player.OnLot.Should().BeSameAs(lot);
        }

        [Test]
        public static void MoveBoxFromLotToLotTest()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap15.txt");
            Sokoban.FunctionalItems.GetFunctionalItems();
            Sokoban.Movement.MovePlayer(ConsoleKey.RightArrow);
            Sokoban.Movement.MovePlayer(ConsoleKey.RightArrow);
            Sokoban.FunctionalItems.Player.X.Should().Be(3);
            Action action = () => Sokoban.FunctionalItems.GetBox(4, 1);
            action.Should().NotThrow();
            var box = Sokoban.FunctionalItems.GetBox(4, 1);
            var lot = Sokoban.FunctionalItems.GetLot(4, 1);
            box.OnLot.Should().BeSameAs(lot);
            lot.IsItemOn.Should().BeTrue();
            lot = Sokoban.FunctionalItems.GetLot(3, 1);
            Sokoban.FunctionalItems.Player.OnLot.Should().BeSameAs(lot);
            lot.IsItemOn.Should().BeTrue();
        }
    }
}
