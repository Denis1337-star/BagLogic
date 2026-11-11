using System;
using System.Collections.Generic;

public class Inventory
{
    readonly List<InventorySlot> _inventorySlots = new List<InventorySlot>(32);
    readonly float _maxWeight;

    public Inventory(float maxWeight)
    {
        _maxWeight = maxWeight;
    }

    public float MaxWeight => _maxWeight;
    public float CurrentWeight { get; private set; }
    public float RemainingWeight => _maxWeight - CurrentWeight;
    public IReadOnlyList<InventorySlot> InventorySlots => _inventorySlots;

    public event Action InventoryChanged;

    public InventoryAddResult TryAdd(ItemDefinition itemConfig, int amount)
    {
        if (itemConfig == null)
            return InventoryAddResult.Failed(InventoryAddFailReason.NullItem);

        if (amount <= 0)
            return InventoryAddResult.Failed(InventoryAddFailReason.InvalidAmount);

        float addWeight = itemConfig.Weight * amount;
        if (addWeight > RemainingWeight)
            return InventoryAddResult.Failed(InventoryAddFailReason.ExceedsWeight);

        int remainingAmount = amount;
        for (int i = 0; i < _inventorySlots.Count; i++)
        {
            InventorySlot inventorySlot = _inventorySlots[i];
            if (inventorySlot.ItemConfig != itemConfig || inventorySlot.IsFull)
                continue;

            int amountToPlace = inventorySlot.RemainingStackSpace;
            if (amountToPlace > remainingAmount)
                amountToPlace = remainingAmount;

            inventorySlot.AddCount(amountToPlace);
            remainingAmount -= amountToPlace;
            if (remainingAmount == 0)
                break;
        }

        while (remainingAmount > 0)
        {
            int stackCount = remainingAmount;
            if (stackCount > itemConfig.MaxStack)
                stackCount = itemConfig.MaxStack;

            _inventorySlots.Add(new InventorySlot(itemConfig, stackCount));
            remainingAmount -= stackCount;
        }

        CurrentWeight += addWeight;
        InventoryChanged?.Invoke();
        return InventoryAddResult.Succeeded(amount);
    }

    public bool TryRemove(ItemDefinition itemConfig, int amount)
    {
        if (itemConfig == null || amount < 0)
            return false;

        if (CountOf(itemConfig) < amount)
            return false;

        int remainingAmount = amount;
        for (int i = _inventorySlots.Count - 1; i >= 0 && remainingAmount > 0; i--)
        {
            InventorySlot slot = _inventorySlots[i];
            if (slot.ItemConfig != itemConfig)
                continue;

            int take = slot.Count;
            if (take > remainingAmount)
                take = remainingAmount;

            slot.RemoveCount(take);
            remainingAmount -= take;
            CurrentWeight -= itemConfig.Weight * take;

            if (slot.Count <= 0)
                _inventorySlots.RemoveAt(i);
        }

        InventoryChanged?.Invoke();
        return true;
    }

    public int CountOf(ItemDefinition itemConfig)
    {
        int total = 0;
        for (int i = 0; i < _inventorySlots.Count; i++)
        {
            if (_inventorySlots[i].ItemConfig == itemConfig)
                total += _inventorySlots[i].Count;
        }
        return total;
    }
}