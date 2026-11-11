
public class ItemDisassembler
{
    readonly Inventory _inventory;

    public ItemDisassembler(Inventory inventory)
    {
        _inventory = inventory;
    }

    public bool CanDisassemble(ItemDefinition item)
    {
        if (item == null)
            return false;

        ItemAmount[] results = item.DisassembleResults;
        if (results == null || results.Length == 0)
            return false;

        for (int i = 0; i < results.Length; i++)
        {
            if (results[i].ItemConfig == null || results[i].Amount <= 0)
                return false;
        }

        return true;
    }

    public ItemDisassembleResult TryDisassembleSlot(int slotIndex)
    {
        var slots = _inventory.InventorySlots;
        if (slotIndex < 0 || slotIndex >= slots.Count)
            return ItemDisassembleResult.Failed(ItemDisassembleFailReason.InvalidSlot);

        ItemDefinition junk = slots[slotIndex].ItemConfig;

        ItemAmount[] results = junk.DisassembleResults;
        if (results == null || results.Length == 0)
            return ItemDisassembleResult.Failed(ItemDisassembleFailReason.NoRecipe);

        float resultsWeight = 0f;
        for (int i = 0; i < results.Length; i++)
        {
            ItemAmount entry = results[i];
            if (entry.ItemConfig == null || entry.Amount <= 0)
                return ItemDisassembleResult.Failed(ItemDisassembleFailReason.NoRecipe);

            resultsWeight += entry.ItemConfig.Weight * entry.Amount;
        }

        float freeAfterRemove = _inventory.RemainingWeight + junk.Weight;
        if (resultsWeight > freeAfterRemove)
            return ItemDisassembleResult.Failed(ItemDisassembleFailReason.ExceedsWeight);

        if (!_inventory.TryRemove(junk, 1))
            return ItemDisassembleResult.Failed(ItemDisassembleFailReason.InvalidSlot);

        for (int i = 0; i < results.Length; i++)
        {
            InventoryAddResult add = _inventory.TryAdd(results[i].ItemConfig, results[i].Amount);
            if (!add.Success)
            {
                return ItemDisassembleResult.Failed(ItemDisassembleFailReason.ExceedsWeight);
            }
        }

        return ItemDisassembleResult.Succeeded();
    }
}