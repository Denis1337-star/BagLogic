using System;
using TMPro;
using UnityEngine;

public class InventoryView : MonoBehaviour, IInventoryView
{
    [SerializeField] private GameObject _root;
    [SerializeField] private TMP_Text _weightText;
    [SerializeField] private Transform _slotsRoot;
    [SerializeField] private InventorySlotView _slotPrefab;

    InventorySlotView[] _invetrorySlots;

    public void BuildSlots(int capacity, Action<int> onSlotClicked)
    {
        _invetrorySlots = new InventorySlotView[capacity];
        for (int i = 0; i < capacity; i++)
        {
            InventorySlotView inventorySlotView = Instantiate(_slotPrefab, _slotsRoot);
            inventorySlotView.ShowEmpty();
            _invetrorySlots[i] = inventorySlotView;
            inventorySlotView.BindClickHandler(i, onSlotClicked);
        }
    }

    public void SetVisible(bool visible)
    {
        _root.SetActive(visible);
    }

    public void SetWeight(float current, float max)
    {
        _weightText.text = $"{Mathf.CeilToInt(current)}/{Mathf.CeilToInt(max)}";
    }

    public void SetSlotCount(int count)
    {
        //  сетка уже создана в BuildSlots
    }

    public void BindSlot(int index, ItemDefinition itemConfig, int amount, bool canDisassemble)
    {
        if (index < 0 || index >= _invetrorySlots.Length)
            return;

        _invetrorySlots[index].ShowItem(itemConfig, amount, canDisassemble);
    }

    public void ClearSlot(int index)
    {
        if (index < 0 || index >= _invetrorySlots.Length)
            return;
        _invetrorySlots[index].ShowEmpty();
    }
}