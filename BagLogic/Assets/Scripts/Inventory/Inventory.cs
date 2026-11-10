using System;
using System.Collections.Generic;

public class Inventory
{
    readonly List<InventorySlot> _slots = new List<InventorySlot>(32);
    readonly float _maxWeight;

    public Inventory(float maxWeight)
    {
        _maxWeight = maxWeight;
    }

    public float MaxWeight => _maxWeight;
    public float CurrentWeight { get; private set; }
    public float RemainingWeight => _maxWeight - CurrentWeight;
    public IReadOnlyList<InventorySlot> Slots => _slots;

    public event Action InventoryChanged;

    public InventoryAddResult TryAdd(ItemDefinition item, int amount)
    {
        if (item == null)
            return InventoryAddResult.Failed(InventoryAddFailReason.NullItem);

        if (amount <= 0)
            return InventoryAddResult.Failed(InventoryAddFailReason.InvalidAmount);

        float addWeight = item.Weight * amount;
        if (addWeight > RemainingWeight)
            return InventoryAddResult.Failed(InventoryAddFailReason.ExceedsWeight);

        int remainingAmount = amount;
        for (int i = 0; i < _slots.Count; i++)
        {
            InventorySlot slot = _slots[i];
            if (slot.Item != item || slot.IsFull)
                continue;

            int canPut = slot.FreeSpace;
            if (canPut > remainingAmount)
                canPut = remainingAmount;

            slot.AddCount(canPut);
            remainingAmount -= canPut;
            if (remainingAmount == 0)
                break;
        }

        while (remainingAmount > 0)
        {
            int stackCount = remainingAmount;
            if (stackCount > item.MaxStack)
                stackCount = item.MaxStack;

            _slots.Add(new InventorySlot(item, stackCount));
            remainingAmount -= stackCount;
        }

        CurrentWeight += addWeight;
        InventoryChanged?.Invoke();
        return InventoryAddResult.Succeeded(amount);
    }

    public bool TryRemove(ItemDefinition item, int amount)
    {
        if (item == null || amount < 0)
            return false;

        if (CountOf(item) < amount)
            return false;

        int remainingAmount = amount;
        for (int i = _slots.Count - 1; i >= 0 && remainingAmount > 0; i--)
        {
            InventorySlot slot = _slots[i];
            if (slot.Item != item)
                continue;

            int take = slot.Count;
            if (take > remainingAmount)
                take = remainingAmount;

            slot.RemoveCount(take);
            remainingAmount -= take;
            CurrentWeight -= item.Weight * take;

            if (slot.Count <= 0)
                _slots.RemoveAt(i);
        }

        InventoryChanged?.Invoke();
        return true;
    }

    public int CountOf(ItemDefinition item)
    {
        int total = 0;
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].Item == item)
                total += _slots[i].Count;
        }
        return total;
    }
}