namespace Sokoban
{
    public interface IItem
    {
        int X { get; set; }
        int Y { get; set; }

        Map.Item GetItem();
        char GetChar();
    }
}
