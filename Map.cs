using System;
using System.IO;

namespace Sokoban
{
    static class Map
    {
        private static string[] MapData;
        private static int MapWidth;
        private static int MapHeight;

        public enum Item
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

        public static void LoadMap(string fileName)
        {
            MapData = GetMapData(fileName);
            MapHeight = MapData.Length;
            MapWidth = MapData[0].Length;
        }

        public static void PrintMap()
        {
            if (MapData == null)
                throw new NullReferenceException("A map must be loaded before accessing it");
            foreach (var mapRow in MapData)
                Console.WriteLine(mapRow);
        }

        public static Item GetItem(int x, int y) => GetItem(x, y, MapData);

        private static Item GetItem(int x, int y, string[] mapData)
        {
            if (mapData == null)
                throw new NullReferenceException("A map must be loaded before accessing it");
            return mapData[y][x] switch
            {
                'p' => Item.Player,
                '#' => Item.Wall,
                'o' => Item.Box,
                '+' => Item.Lot,
                'O' => Item.BoxOnLot,
                'P' => Item.PlayerOnLot,
                _ => Item.Empty,
            };
        }

        private static string[] GetMapData(string fileName)
        {
            var mapData = File.ReadAllText("maps/" + fileName).Split("\r\n");
            CheckMap(mapData, fileName);
            return mapData;
        }

        private static void CheckMap(string[] mapData, string fileName)
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
                throw new ArgumentException("Invalid map: a player must be enclosed by walls", fileName);
        }

        private static bool AreMapItemsCorrect(string[] mapData)
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
                    var item = GetItem(x, y, mapData);
                    if ((item == Item.Player || item == Item.PlayerOnLot) && playerFound)
                        return false;
                    switch (item)
                    {
                        case Item.Player:
                            playerFound = true;
                            break;
                        case Item.PlayerOnLot:
                            playerFound = true;
                            break;
                        case Item.Box:
                            boxCount++;
                            break;
                        case Item.Lot:
                            lotCount++;
                            break;
                    }
                }
            }

            return playerFound && boxCount == lotCount && boxCount != 0;
        }

        private static bool IsPlayerEnclosed(string[] mapData)
        {
            var mapHeight = mapData.Length;
            var mapWidth = mapData[0].Length;

            var playerCoords = FindPlayerCoords(mapData);
            if (playerCoords[0] == 0 || playerCoords[0] == mapWidth - 1
                || playerCoords[1] == 0 || playerCoords[1] == mapHeight - 1)
                    return false;

            Direction nextWallPosition;
            int nextX, nextY;
            for (var y = playerCoords[1]; y >= 0; y--)
            {
                if (GetItem(playerCoords[0], y, mapData) == Item.Wall)
                {
                    nextWallPosition = GetNextWallPosition(mapData, playerCoords[0], y + 1, Direction.North);
                    switch (nextWallPosition)
                    {
                        case Direction.North:
                            nextX = playerCoords[0] + 1;
                            nextY = y + 1;
                            break;
                        case Direction.East:
                            nextX = playerCoords[0];

                    }
                    return IsClosedLoopFound(mapData, playerCoords[0], y + 1,
                                             playerCoords[0] + 1, y + 1, Direction.North);
                }
            }

            return false;
        }

        private static int[] FindPlayerCoords(string[] mapData)
        {
            var mapHeight = mapData.Length;
            var mapWidth = mapData[0].Length;
            var playerCoords = new int[2];

            for (var x = 0; x < mapWidth; x++)
            {
                for (var y = 0; y < mapHeight; y++)
                {
                    var item = GetItem(x, y, mapData);
                    if (item == Item.Player || item == Item.PlayerOnLot)
                    {
                        playerCoords[0] = x;
                        playerCoords[1] = y;
                        return playerCoords;
                    }
                }
            }

            return null;
        }

        private static bool IsClosedLoopFound(string[] mapData, int originX, int originY, 
                                              int currentX, int currentY, Direction wallPosition)
        {

        }

        private static Direction GetNextWallPosition(string[] mapData, int currentX, int currentY, 
                                                      Direction wallPosition)
        {
            while (true)
            {
                switch (wallPosition)
                {
                    case Direction.North:
                        if (GetItem(currentX, currentY - 1) == Item.Wall)
                            return Direction.North;
                        wallPosition = Direction.East;
                        break;
                    case Direction.East:
                        if (GetItem(currentX + 1, currentY) == Item.Wall)
                            return Direction.East;
                        wallPosition = Direction.South;
                        break;
                    case Direction.South:
                        if (GetItem(currentX, currentY + 1) == Item.Wall)
                            return Direction.South;
                        wallPosition = Direction.West;
                        break;
                    case Direction.West:
                        if (GetItem(currentX - 1, currentY) == Item.Wall)
                            return Direction.West;
                        wallPosition = Direction.North;
                        break;
                }
            }
        }

        private static int[] GetNextCoords(int currentX, int currentY, Direction nextWallPosition)
        {
            var nextCoords = new int[2];
            switch (nextWallPosition)
            {
                case Direction.North:
                    nextCoords[0] = currentX + 1;
                    nextCoords[1] = currentY;
                    break;
                case Direction.East:
                    nextCoords[0] = currentX;
                    nextCoords[1] = currentY 

            }
        }
    }
}
