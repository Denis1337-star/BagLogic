using UnityEngine;

public class PlayerLootPicker : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private PlayerInteractInput _interactInput;
    [SerializeField] private HudMessageHud _hudMessageHud;
    [SerializeField] private PickupPromptView _pickupPromptView;

    [Header("Scan")]
    [SerializeField] private LayerMask _lootMask;
    [SerializeField] private float _autoPickupRadius = 1.5f;
    [SerializeField] private float _manualPickupRadius = 2.5f;
    [SerializeField] private int _maxOverlapResults = 16;

    LootPickup _lootPickup;

    void Start()
    {
        LootScanner autoScanner = new LootScanner(transform, _autoPickupRadius, _lootMask, _maxOverlapResults);
        LootScanner manualScanner = new LootScanner(transform, _manualPickupRadius, _lootMask, _maxOverlapResults);
        _lootPickup = new LootPickup(_playerInventory.Inventory, autoScanner, manualScanner, _maxOverlapResults);
        _lootPickup.PickupFailed += OnPickupFailed;
    }

    void OnEnable()
    {
        _interactInput.InteractPressed += OnInteractPressed;
    }

    void OnDisable()
    {
        _interactInput.InteractPressed -= OnInteractPressed;
    }

    void OnDestroy()
    {
        _lootPickup.PickupFailed -= OnPickupFailed;
    }

    void Update()
    {
        _lootPickup.TickAutomaticPickups();

        LootWorldItem manualLootWorldItem = _lootPickup.GetClosestManualLoot();
        if (manualLootWorldItem != null)
            _pickupPromptView.ShowForItem(manualLootWorldItem.ItemConfig.DisplayName);
        else
            _pickupPromptView.Hide();
    }

    void OnInteractPressed()
    {
        _lootPickup.TryPickupClosestManual();
    }

    void OnPickupFailed(InventoryAddFailReason reason)
    {
        if (reason == InventoryAddFailReason.ExceedsWeight)
            _hudMessageHud.Message.Show("Не влезает по весу");
        else
            _hudMessageHud.Message.Show("Не удалось подобрать");
    }

}