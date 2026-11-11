
public class InventorySlot
{
    public ItemDefinition ItemConfig { get; private set; }
    public int Count { get; private set; }

    public InventorySlot(ItemDefinition itemConfig, int count)
    {
        ItemConfig = itemConfig;
        Count = count;
    }

    public int RemainingStackSpace => ItemConfig.MaxStack - Count;
    public bool IsFull => Count >= ItemConfig.MaxStack;
    public float TotalWeight => ItemConfig.Weight * Count;

    public void AddCount(int amount)
    {
        Count += amount;
    }

    public void RemoveCount(int amount)
    {
        Count -= amount;
    }
}