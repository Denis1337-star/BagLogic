using System;
using UnityEngine;

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

    public void TickAutomatic()
    {
        int count = _autoScanner.Collect(_autoBuffer);
        for (int i = 0; i < count; i++)
        {
            LootWorldItem loot = _autoBuffer[i];
            if (loot.Item.PickupMode != PickupMode.Automatic)
                continue;

            TryPickup(loot);
        }
    }

    public void TryManualInteract()
    {
        LootWorldItem loot = _manualScanner.FindClosest(_manualBuffer, PickupMode.Manual);
        if (loot == null)
            return;

        TryPickup(loot);
    }

    void TryPickup(LootWorldItem loot)
    {
        InventoryAddResult result = _inventory.TryAdd(loot.Item, loot.Amount);
        if (!result.Success)
        {
            PickupFailed?.Invoke(result.FailReason);
            return;
        }

        ItemDefinition item = loot.Item;
        int amount = loot.Amount;
        loot.Consume();
        PickupSucceeded?.Invoke(item, amount);
    }
}