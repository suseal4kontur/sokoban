using System;
using System.IO;

namespace Sokoban
{
    static class Interface
    {
        public static void OpenMap(string fileName)
        {
            var mapData = File.ReadAllText("maps/" + fileName);
            Console.WriteLine(mapData);
        }
    }
}
