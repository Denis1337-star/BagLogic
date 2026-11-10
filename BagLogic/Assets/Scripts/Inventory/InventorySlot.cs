
public class InventorySlot
{
    public ItemDefinition Item { get; private set; }
    public int Count { get; private set; }

    public InventorySlot(ItemDefinition item, int count)
    {
        Item = item;
        Count = count;
    }

    public int FreeSpace => Item.MaxStack - Count;
    public bool IsFull => Count >= Item.MaxStack;
    public float TotalWeight => Item.Weight * Count;

    public void AddCount(int amount)
    {
        Count += amount;
    }

    public void RemoveCount(int amount)
    {
        Count -= amount;
    }
}