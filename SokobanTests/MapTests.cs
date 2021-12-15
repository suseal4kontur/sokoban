using NUnit.Framework;
using FluentAssertions;
using System;

namespace SokobanTests
{
    [TestFixture]
    public static class MapTests
    {
        [Test]
        public static void PrintMapNotLoadedTest()
        {
            Sokoban.Map.Clear();
            Action action = () => Sokoban.Map.Print();
            action.Should().Throw<NullReferenceException>();
        }

        [Test]
        public static void GetItemMapNotLoadedTest()
        {
            Sokoban.Map.Clear();
            Action action = () => Sokoban.Map.ReadItem(0, 0);
            action.Should().Throw<NullReferenceException>();
        }

        [TestCase(1, 1, Sokoban.Map.ItemName.Player, "testmap1.txt")]
        [TestCase(0, 0, Sokoban.Map.ItemName.Wall, "testmap1.txt")]
        [TestCase(2, 1, Sokoban.Map.ItemName.Empty, "testmap1.txt")]
        [TestCase(1, 2, Sokoban.Map.ItemName.Wall, "testmap1.txt")]
        [TestCase(3, 1, Sokoban.Map.ItemName.Box, "testmap1.txt")]
        [TestCase(4, 1, Sokoban.Map.ItemName.Lot, "testmap1.txt")]
        [TestCase(5, 1, Sokoban.Map.ItemName.BoxOnLot, "testmap1.txt")]
        [TestCase(1, 1, Sokoban.Map.ItemName.PlayerOnLot, "testmap2.txt")]
        public static void LoadMapAndGetItemTest(int x, int y, Sokoban.Map.ItemName item, string fileName)
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load(fileName);
            Sokoban.Map.ReadItem(x, y).Should().Be(item);
        }

        [TestCase(-1, 0, "testmap1.txt")]
        [TestCase(7, 0, "testmap1.txt")]
        [TestCase(0, -2, "testmap1.txt")]
        [TestCase(0, 6, "testmap1.txt")]
        public static void GetItemWrongCoordsTest(int x, int y, string fileName)
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load(fileName);
            Action action = () => Sokoban.Map.ReadItem(x, y);
            action.Should().Throw<IndexOutOfRangeException>();
        }

        [TestCase(3, 1, Sokoban.Map.ItemName.Empty)]
        [TestCase(4, 1, Sokoban.Map.ItemName.BoxOnLot)]
        public static void SetItemTest(int x, int y, Sokoban.Map.ItemName item)
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap1.txt");
            Sokoban.Map.WriteItem(x, y, item);
            var readItem = Sokoban.Map.ReadItem(x, y);
            readItem.Should().Be(item);
        }

        [TestCase("testmap3.txt")]
        [TestCase("testmap4.txt")]
        public static void LoadMapEmptyTest(string fileName)
        {
            Sokoban.Map.Clear();
            Action action = () => Sokoban.Map.Load(fileName);
            action.Should().Throw<ArgumentException>()
                  .WithMessage($"Invalid map: rows must not be empty (Parameter '{fileName}')");
        }

        [TestCase("testmap5.txt")]
        [TestCase("testmap6.txt")]
        public static void LoadMapDifferentColumnCountsTest(string fileName)
        {
            Sokoban.Map.Clear();
            Action action = () => Sokoban.Map.Load(fileName);
            action.Should().Throw<ArgumentException>()
                  .WithMessage($"Invalid map: columns count must be consistent (Parameter '{fileName}')");
        }

        [TestCase("testmap7.txt")]
        [TestCase("testmap8.txt")]
        public static void LoadMapWithountNeededItemsTest(string fileName)
        {
            Sokoban.Map.Clear();
            Action action = () => Sokoban.Map.Load(fileName);
            action.Should().Throw<ArgumentException>()
                  .WithMessage($"Invalid map: must contain one player and equal amounts of boxes and lots (Parameter '{fileName}')");
        }

        [TestCase("testmap9.txt")]
        [TestCase("testmap10.txt")]
        public static void LoadMapPlayerNotEnclosedTest(string fileName)
        {
            Sokoban.Map.Clear();
            Action action = () => Sokoban.Map.Load(fileName);
            action.Should().Throw<ArgumentException>()
                  .WithMessage($"Invalid map: player must be enclosed by walls (Parameter '{fileName}')");
        }

        [TestCase("testmap11.txt")]
        [TestCase("testmap12.txt")]
        [TestCase("testmap13.txt")]
        public static void LoadMapCorrectMapTest(string fileName)
        {
            Sokoban.Map.Clear();
            Action action = () => Sokoban.Map.Load(fileName);
            action.Should().NotThrow();
        }

        [Test]
        public static void LoadMapPlayerSurrounded()
        {
            Sokoban.Map.Clear();
            Action action = () => Sokoban.Map.Load("testmap14.txt");
            action.Should().Throw<TimeoutException>()
                  .WithMessage($"Invalid map: the player is surrounded by 4 walls");
        }

        [Test]
        public static void UpdateMapTest()
        {
            Sokoban.Map.Clear();
            Sokoban.Map.Load("testmap11.txt");
            Sokoban.FunctionalItems.ClearFunctionalItems();
            Action action = () => Sokoban.Map.Update();
            action.Should().Throw<NullReferenceException>();
        }
    }
}
