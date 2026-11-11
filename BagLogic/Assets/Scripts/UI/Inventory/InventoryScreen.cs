using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryScreen : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private HudMessageHud _hudMessageHud;
    [SerializeField] private int _slotCapacity = 24;
    [SerializeField] private Key _toggleKey = Key.Tab;

    InventoryPresenter _inventoryPresenter;

    void Start()
    {
        _inventoryView.BuildSlots(_slotCapacity, OnSlotClicked);
        _inventoryPresenter = new InventoryPresenter(_playerInventory.Inventory,_inventoryView,
            _slotCapacity,_hudMessageHud.Message);
        SetGameplayInputEnabled(true);
    }

    void OnSlotClicked(int index)
    {
        _inventoryPresenter.OnSlotClicked(index);
    }

    void OnDestroy()
    {
        _inventoryPresenter?.Dispose();
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current[_toggleKey].wasPressedThisFrame)
        {
            _inventoryPresenter.Toggle();
            SetGameplayInputEnabled(!_inventoryPresenter.IsOpen);
        }
    }
    void SetGameplayInputEnabled(bool enabled)
    {
        if (_playerInput != null)
            _playerInput.enabled = enabled;

        Cursor.visible = !enabled;
        Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None;
    }
}