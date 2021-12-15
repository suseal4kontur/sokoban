using NUnit.Framework;
using FluentAssertions;
using System;

namespace SokobanTests
{
    [TestFixture]
    public static class FunctionalItemsTests
    {
        [Test]
        public static void ClearFunctionalItemsTest()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap1.txt");
            Sokoban.FunctionalItems.ClearFunctionalItems();
            Sokoban.FunctionalItems.Player.Should().BeNull();
            Sokoban.FunctionalItems.Boxes.Count.Should().Be(0);
            Sokoban.FunctionalItems.Lots.Count.Should().Be(0);
        }

        [Test]
        public static void GetFunctionalItemsTestOne()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap1.txt");
            Sokoban.FunctionalItems.ClearFunctionalItems();
            Sokoban.FunctionalItems.GetFunctionalItems();
            Sokoban.FunctionalItems.Player.X.Should().Be(1);
            Sokoban.FunctionalItems.Player.Y.Should().Be(1);
            Sokoban.FunctionalItems.Player.OnLot.Should().BeNull();
            Sokoban.FunctionalItems.Boxes[0].X.Should().Be(3);
            Sokoban.FunctionalItems.Boxes[0].Y.Should().Be(1);
            Sokoban.FunctionalItems.Boxes[0].OnLot.Should().BeNull();
            Sokoban.FunctionalItems.Boxes[1].X.Should().Be(5);
            Sokoban.FunctionalItems.Boxes[1].Y.Should().Be(1);
            Sokoban.FunctionalItems.Boxes[1].OnLot.Should().BeSameAs(Sokoban.FunctionalItems.Lots[1]);
            Sokoban.FunctionalItems.Lots[0].X.Should().Be(4);
            Sokoban.FunctionalItems.Lots[0].Y.Should().Be(1);
            Sokoban.FunctionalItems.Lots[0].IsItemOn.Should().BeFalse();
            Sokoban.FunctionalItems.Lots[1].X.Should().Be(5);
            Sokoban.FunctionalItems.Lots[1].Y.Should().Be(1);
            Sokoban.FunctionalItems.Lots[1].IsItemOn.Should().BeTrue();
        }

        [Test]
        public static void GetFunctionalItemsTestTwo()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap15.txt");
            Sokoban.FunctionalItems.ClearFunctionalItems();
            Sokoban.FunctionalItems.GetFunctionalItems();
            Sokoban.FunctionalItems.Player.X.Should().Be(1);
            Sokoban.FunctionalItems.Player.Y.Should().Be(1);
            Sokoban.FunctionalItems.Player.OnLot.Should().BeSameAs(Sokoban.FunctionalItems.Lots[0]);
            Sokoban.FunctionalItems.Lots[0].IsItemOn.Should().BeTrue();
        }

        [TestCase(1, 2, "testmap15.txt", 0)]
        [TestCase(3, 1, "testmap15.txt", 1)]
        [TestCase(3, 2, "testmap15.txt", 2)]
        public static void GetBoxCorrectCoordsTest(int x, int y, string fileName, int listIndex)
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load(fileName);
            Sokoban.FunctionalItems.GetFunctionalItems();
            var box = Sokoban.FunctionalItems.GetBox(x, y);
            box.Should().BeSameAs(Sokoban.FunctionalItems.Boxes[listIndex]);
        }

        [TestCase(1, 1, "testmap15.txt", 0)]
        [TestCase(3, 1, "testmap15.txt", 1)]
        [TestCase(4, 1, "testmap15.txt", 2)]
        public static void GetLotCorrectCoordsTest(int x, int y, string fileName, int listIndex)
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load(fileName);
            Sokoban.FunctionalItems.GetFunctionalItems();
            var lot = Sokoban.FunctionalItems.GetLot(x, y);
            lot.Should().BeSameAs(Sokoban.FunctionalItems.Lots[listIndex]);
        }

        [Test]
        public static void GetBoxIncorrectCoordsTest()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap15.txt");
            Sokoban.FunctionalItems.GetFunctionalItems();
            Action action = () => Sokoban.FunctionalItems.GetBox(2, 1);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public static void GetLotIncorrectCoordsTest()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap15.txt");
            Sokoban.FunctionalItems.GetFunctionalItems();
            Action action = () => Sokoban.FunctionalItems.GetLot(1, 2);
            action.Should().Throw<ArgumentException>();
        }
    }
}
