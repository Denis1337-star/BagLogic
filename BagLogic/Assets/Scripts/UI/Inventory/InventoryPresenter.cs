using UnityEngine;

public class InventoryPresenter
{
    readonly Inventory _inventory;
    readonly IInventoryView _inventoryView;
    readonly ItemDisassembler _itemDisassembler;
    readonly HudMessage _hudMessage;
    readonly int _visualCapacity;
    bool _isOpen;
    public bool IsOpen => _isOpen;

    public InventoryPresenter(Inventory inventory, IInventoryView inventoryView, int visualCapacity, HudMessage hudMessage)
    {
        _inventory = inventory;
        _inventoryView = inventoryView;
        _visualCapacity = visualCapacity;
        _hudMessage = hudMessage;
        _itemDisassembler = new ItemDisassembler(inventory);
        _inventory.InventoryChanged += Refresh;
        _inventoryView.SetVisible(false);
        Refresh();
    }
    public void OnSlotClicked(int index)
    {
        if (!_isOpen)
            return;

        ItemDisassembleResult result = _itemDisassembler.TryDisassembleSlot(index);
        if (result.Success)
            return;

        if (result.FailReason == ItemDisassembleFailReason.ExceedsWeight)
            _hudMessage.Show("Разборка: не влезает по весу");
    }

    public void Dispose()
    {
        _inventory.InventoryChanged -= Refresh;
    }

    public void Toggle()
    {
        SetOpen(!_isOpen);
    }

    public void SetOpen(bool open)
    {
        _isOpen = open;
        _inventoryView.SetVisible(_isOpen);
        if (_isOpen)
            Refresh();
    }

    void Refresh()
    {
        _inventoryView.SetWeight(_inventory.CurrentWeight, _inventory.MaxWeight);
        var inventorySlots = _inventory.InventorySlots;
        int count = inventorySlots.Count;

        if (count > _visualCapacity)
            count = _visualCapacity;

        for (int i = 0; i < count; i++)
        {
            ItemDefinition itemConfig = inventorySlots[i].ItemConfig;
            bool canDisassemble = _itemDisassembler.CanDisassemble(itemConfig);
            _inventoryView.BindSlot(i, itemConfig, inventorySlots[i].Count, canDisassemble);
        }

        for (int i = count; i < _visualCapacity; i++)
            _inventoryView.ClearSlot(i);
    }
}