using System;

public class LootPickup
{
    readonly Inventory _inventory;
    readonly LootScanner _autoScanner;
    readonly LootScanner _manualScanner;
    readonly LootWorldItem[] _autoBuffer;
    readonly LootWorldItem[] _manualBuffer;

    public event Action<InventoryAddFailReason> PickupFailed;
    public event Action<ItemDefinition, int> PickupSucceeded;

    public LootPickup(
        Inventory inventory,
        LootScanner autoScanner,
        LootScanner manualScanner,
        int bufferSize)
    {
        _inventory = inventory;
        _autoScanner = autoScanner;
        _manualScanner = manualScanner;
        _autoBuffer = new LootWorldItem[bufferSize];
        _manualBuffer = new LootWorldItem[bufferSize];
    }

    public void TickAutomaticPickups()
    {
        int count = _autoScanner.Collect(_autoBuffer);
        for (int i = 0; i < count; i++)
        {
            LootWorldItem lootWorldItem = _autoBuffer[i];
            if (lootWorldItem.ItemConfig.PickupMode != PickupMode.Automatic)
                continue;

            TryPickup(lootWorldItem);
        }
    }

    public void TryPickupClosestManual()
    {
        LootWorldItem lootWorldItem = _manualScanner.FindClosest(_manualBuffer, PickupMode.Manual);
        if (lootWorldItem == null)
            return;

        TryPickup(lootWorldItem);
    }

    void TryPickup(LootWorldItem lootWorldItem)
    {
        InventoryAddResult result = _inventory.TryAdd(lootWorldItem.ItemConfig, lootWorldItem.Amount);
        if (!result.Success)
        {
            PickupFailed?.Invoke(result.FailReason);
            return;
        }

        ItemDefinition itemConfig = lootWorldItem.ItemConfig;
        int amount = lootWorldItem.Amount;
        lootWorldItem.Despawn();
        PickupSucceeded?.Invoke(itemConfig, amount);
    }
    public LootWorldItem GetClosestManualLoot()
    {
        return _manualScanner.FindClosest(_manualBuffer, PickupMode.Manual);
    }
}