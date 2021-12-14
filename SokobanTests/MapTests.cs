using NUnit.Framework;
using FluentAssertions;
using System;

namespace SokobanTests
{
    [TestFixture]
    public class MapTests
    {
        [Test]
        public void PrintMapNotLoadedTest()
        {
            Sokoban.Map.ClearMap();
            Action action = () => Sokoban.Map.PrintMap();
            action.Should().Throw<NullReferenceException>();
        }

        [Test]
        public void GetItemMapNotLoadedTest()
        {
            Sokoban.Map.ClearMap();
            Action action = () => Sokoban.Map.GetItem(0, 0);
            action.Should().Throw<NullReferenceException>();
        }

        [TestCase(1, 1, Sokoban.Map.Item.Player, "testmap1.txt")]
        [TestCase(0, 0, Sokoban.Map.Item.Wall, "testmap1.txt")]
        [TestCase(2, 1, Sokoban.Map.Item.Empty, "testmap1.txt")]
        [TestCase(1, 2, Sokoban.Map.Item.Wall, "testmap1.txt")]
        [TestCase(3, 1, Sokoban.Map.Item.Box, "testmap1.txt")]
        [TestCase(4, 1, Sokoban.Map.Item.Lot, "testmap1.txt")]
        [TestCase(5, 1, Sokoban.Map.Item.BoxOnLot, "testmap1.txt")]
        [TestCase(1, 1, Sokoban.Map.Item.PlayerOnLot, "testmap2.txt")]
        public void LoadMapAndGetItemTest(int x, int y, Sokoban.Map.Item item, string fileName)
        {
            Sokoban.Map.ClearMap();
            Sokoban.Map.LoadMap(fileName);
            Sokoban.Map.GetItem(x, y).Should().Be(item);
        }

        [TestCase(-1, 0, "testmap1.txt")]
        [TestCase(7, 0, "testmap1.txt")]
        [TestCase(0, -2, "testmap1.txt")]
        [TestCase(0, 6, "testmap1.txt")]
        public void GetItemWrongCoordsTest(int x, int y, string fileName)
        {
            Sokoban.Map.ClearMap();
            Sokoban.Map.LoadMap(fileName);
            Action action = () => Sokoban.Map.GetItem(x, y);
            action.Should().Throw<IndexOutOfRangeException>();
        }

        [TestCase(3, 1, Sokoban.Map.Item.Empty)]
        [TestCase(4, 1, Sokoban.Map.Item.BoxOnLot)]
        public void SetItemTest(int x, int y, Sokoban.Map.Item item)
        {
            Sokoban.Map.ClearMap();
            Sokoban.Map.LoadMap("testmap1.txt");
            Sokoban.Map.SetItem(x, y, item);
            var readItem = Sokoban.Map.GetItem(x, y);
            readItem.Should().Be(item);
        }

        [TestCase("testmap3.txt")]
        [TestCase("testmap4.txt")]
        public void LoadMapEmptyTest(string fileName)
        {
            Sokoban.Map.ClearMap();
            Action action = () => Sokoban.Map.LoadMap(fileName);
            action.Should().Throw<ArgumentException>()
                  .WithMessage($"Invalid map: rows must not be empty (Parameter '{fileName}')");
        }

        [TestCase("testmap5.txt")]
        [TestCase("testmap6.txt")]
        public void LoadMapDifferentColumnCountsTest(string fileName)
        {
            Sokoban.Map.ClearMap();
            Action action = () => Sokoban.Map.LoadMap(fileName);
            action.Should().Throw<ArgumentException>()
                  .WithMessage($"Invalid map: columns count must be consistent (Parameter '{fileName}')");
        }

        [TestCase("testmap7.txt")]
        [TestCase("testmap8.txt")]
        public void LoadMapWithountNeededItemsTest(string fileName)
        {
            Sokoban.Map.ClearMap();
            Action action = () => Sokoban.Map.LoadMap(fileName);
            action.Should().Throw<ArgumentException>()
                  .WithMessage($"Invalid map: must contain one player and equal amounts of boxes and lots (Parameter '{fileName}')");
        }

        [TestCase("testmap9.txt")]
        [TestCase("testmap10.txt")]
        public void LoadMapPlayerNotEnclosedTest(string fileName)
        {
            Sokoban.Map.ClearMap();
            Action action = () => Sokoban.Map.LoadMap(fileName);
            action.Should().Throw<ArgumentException>()
                  .WithMessage($"Invalid map: player must be enclosed by walls (Parameter '{fileName}')");
        }

        [TestCase("testmap11.txt")]
        [TestCase("testmap12.txt")]
        [TestCase("testmap13.txt")]
        public void LoadMapCorrectMapTest(string fileName)
        {
            Sokoban.Map.ClearMap();
            Action action = () => Sokoban.Map.LoadMap(fileName);
            action.Should().NotThrow();
        }

        [Test]
        public void LoadMapPlayerSurrounded()
        {
            Sokoban.Map.ClearMap();
            Action action = () => Sokoban.Map.LoadMap("testmap14.txt");
            action.Should().Throw<TimeoutException>()
                  .WithMessage($"Invalid map: the player is surrounded by 4 walls");
        }
    }
}
