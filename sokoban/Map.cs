using System;
using System.IO;

namespace Sokoban
{
    public static class Map
    {
        private static char[][] Data;
        public static int Width { get; private set; }
        public static int Height { get; private set; }

        public enum ItemName
        {
            Empty,
            Wall,
            Player,
            Box,
            Lot,
            BoxOnLot,
            PlayerOnLot
        }

        public enum Direction
        {
            North,
            East,
            West,
            South
        }

        public static void Clear()
        {
            Data = null;
            Height = 0;
            Width = 0;
        }

        public static void Load(string fileName)
        {
            Data = GetMapData(fileName);
            Height = Data.Length;
            Width = Data[0].Length;
        }

        public static void Print() // maybe move to a different class
        {
            if (Data == null)
                throw new NullReferenceException("Map must be loaded before accessing it");
            foreach (var mapRow in Data)
                Console.WriteLine(mapRow);
        }

        public static ItemName ReadItem(int x, int y) => ReadItem(x, y, Data);

        public static void WriteItem(int x, int y, ItemName item)
        {
            CheckCoordsCorrectness(x, y, Data);
            Data[y][x] = ToChar(item);
        }

        private static char[][] GetMapData(string fileName)
        {
            var mapDataByRows = File.ReadAllText("maps/" + fileName).Split("\r\n");
            char[][] mapData = new char[mapDataByRows.Length][];

            for (var i = 0; i < mapDataByRows.Length; i++)
                mapData[i] = mapDataByRows[i].ToCharArray();

            CheckMap(mapData, fileName);
            return mapData;
        }

        private static void CheckCoordsCorrectness(int x, int y, char[][] mapData)
        {
            if (mapData == null)
                throw new NullReferenceException("Map must be loaded before accessing it");

            if (x < 0 || x > mapData[0].Length - 1 || y < 0 || y > mapData.Length - 1)
                throw new IndexOutOfRangeException();
        }

        private static ItemName ToItem(char character) => character switch
        {
            'p' => ItemName.Player,
            '#' => ItemName.Wall,
            'o' => ItemName.Box,
            '+' => ItemName.Lot,
            'O' => ItemName.BoxOnLot,
            'P' => ItemName.PlayerOnLot,
            ' ' => ItemName.Empty,
            _ => throw new ArgumentException($"The character {character} is unassigned"),
        };

        private static char ToChar(ItemName item) => item switch
        {
            ItemName.Player => 'p',
            ItemName.Wall => '#',
            ItemName.Box => 'o',
            ItemName.Lot => '+',
            ItemName.BoxOnLot => 'O',
            ItemName.PlayerOnLot => 'P',
            ItemName.Empty => ' ',
            _ => throw new ArgumentException($"The item {item} has no character assingned to it"),
        };

        private static ItemName ReadItem(int x, int y, char[][] mapData)
        {
            CheckCoordsCorrectness(x, y, mapData);
            return ToItem(mapData[y][x]);
        }

        private static void CheckMap(char[][] mapData, string fileName)
        {
            if (mapData == null || mapData[0].Length == 0)
                throw new ArgumentException("Invalid map: rows must not be empty", fileName);

            foreach (var mapRow in mapData)
            {
                if (mapRow.Length != mapData[0].Length)
                    throw new ArgumentException("Invalid map: columns count must be consistent", fileName);
            }

            if (!AreMapItemsCorrect(mapData))
                throw new ArgumentException("Invalid map: must contain one player and equal amounts of boxes and lots", fileName);
            if (!IsPlayerEnclosed(mapData))
                throw new ArgumentException("Invalid map: player must be enclosed by walls", fileName);
        }

        private static bool AreMapItemsCorrect(char[][] mapData)
        {
            var playerFound = false;
            var boxCount = 0;
            var lotCount = 0;
            var mapHeight = mapData.Length;
            var mapWidth = mapData[0].Length;

            for (var x = 0; x < mapWidth; x++)
            {
                for (var y = 0; y < mapHeight; y++)
                {
                    var item = ReadItem(x, y, mapData);
                    if ((item == ItemName.Player || item == ItemName.PlayerOnLot) && playerFound)
                        return false;
                    switch (item)
                    {
                        case ItemName.Player:
                            playerFound = true;
                            break;
                        case ItemName.PlayerOnLot:
                            playerFound = true;
                            lotCount++;
                            break;
                        case ItemName.Box:
                            boxCount++;
                            break;
                        case ItemName.Lot:
                            lotCount++;
                            break;
                    }
                }
            }

            return playerFound && boxCount == lotCount && boxCount != 0;
        }

        private static bool IsPlayerEnclosed(char[][] mapData)
        {
            var mapHeight = mapData.Length;
            var mapWidth = mapData[0].Length;

            var playerCoords = FindPlayerCoords(mapData);
            if (playerCoords[0] == 0 || playerCoords[0] == mapWidth - 1
                || playerCoords[1] == 0 || playerCoords[1] == mapHeight - 1)
                    return false;

            Direction nextExpectedWallPosition;
            int[] nextCoords;
            for (var y = playerCoords[1] - 1; y >= 0; y--)
            {
                if (ReadItem(playerCoords[0], y, mapData) == ItemName.Wall)
                {
                    nextExpectedWallPosition = GetNextExpectedWallPosition(mapData, playerCoords[0], y + 1, Direction.North);
                    nextCoords = GetNextCoords(playerCoords[0], y + 1, nextExpectedWallPosition);
                    return IsClosedLoopFound(mapData, mapWidth, mapHeight, playerCoords[0], y + 1,
                                             nextCoords[0], nextCoords[1], nextExpectedWallPosition);
                }
            }

            return false;
        }

        private static int[] FindPlayerCoords(char[][] mapData)
        {
            var mapHeight = mapData.Length;
            var mapWidth = mapData[0].Length;
            var playerCoords = new int[2];

            for (var x = 0; x < mapWidth; x++)
            {
                for (var y = 0; y < mapHeight; y++)
                {
                    var item = ReadItem(x, y, mapData);
                    if (item == ItemName.Player || item == ItemName.PlayerOnLot)
                    {
                        playerCoords[0] = x;
                        playerCoords[1] = y;
                        return playerCoords;
                    }
                }
            }

            throw new ArgumentException("Player is not found on the map");
        }

        private static Direction GetNextExpectedWallPosition(char[][] mapData, int currentX, int currentY,
                                                      Direction currentWallPosition)
        {
            var wallChecks = 0;
            while (true)
            {
                if (wallChecks > 3)
                    throw new TimeoutException("Invalid map: the player is surrounded by 4 walls");

                switch (currentWallPosition)
                {
                    case Direction.North:
                        if (ReadItem(currentX, currentY - 1, mapData) != ItemName.Wall)
                            return Direction.West;
                        currentWallPosition = Direction.East;
                        wallChecks++;
                        break;
                    case Direction.East:
                        if (ReadItem(currentX + 1, currentY, mapData) != ItemName.Wall)
                            return Direction.North;
                        currentWallPosition = Direction.South;
                        wallChecks++;
                        break;
                    case Direction.South:
                        if (ReadItem(currentX, currentY + 1, mapData) != ItemName.Wall)
                            return Direction.East;
                        currentWallPosition = Direction.West;
                        wallChecks++;
                        break;
                    case Direction.West:
                        if (ReadItem(currentX - 1, currentY, mapData) != ItemName.Wall)
                            return Direction.South;
                        currentWallPosition = Direction.North;
                        wallChecks++;
                        break;
                }
            }
        }

        private static bool IsClosedLoopFound(char[][] mapData, int mapWidth, int mapHeight,
                                              int originX, int originY, int currentX, int currentY,
                                              Direction currentWallPosition)
        {
            if (currentX == 0 || currentX == mapWidth - 1
                || currentY == 0 || currentY == mapHeight - 1)
                return false;

            if (currentX == originX && currentY == originY)
                return true;

            var nextExpectedWallPosition = GetNextExpectedWallPosition(mapData, currentX, currentY, currentWallPosition);
            var nextCoords = GetNextCoords(currentX, currentY, nextExpectedWallPosition);
            return IsClosedLoopFound(mapData, mapWidth, mapHeight, originX, originY,
                                     nextCoords[0], nextCoords[1], nextExpectedWallPosition);
        }

        private static int[] GetNextCoords(int currentX, int currentY, Direction nextExpectedWallPosition)
        {
            var nextCoords = new int[2];
            switch (nextExpectedWallPosition)
            {
                case Direction.North:
                    nextCoords[0] = currentX + 1;
                    nextCoords[1] = currentY;
                    break;
                case Direction.East:
                    nextCoords[0] = currentX;
                    nextCoords[1] = currentY + 1;
                    break;
                case Direction.South:
                    nextCoords[0] = currentX - 1;
                    nextCoords[1] = currentY;
                    break;
                case Direction.West:
                    nextCoords[0] = currentX;
                    nextCoords[1] = currentY - 1;
                    break;
            }
            return nextCoords;
        }
    }
}
