namespace Sokoban
{
    public interface IFunctionalItem
    {
        int X { get; set; }
        int Y { get; set; }

        Map.ItemName GetItemName();
    }
}
