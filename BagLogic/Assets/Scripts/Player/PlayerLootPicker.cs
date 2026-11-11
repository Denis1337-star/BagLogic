using UnityEngine;

public class PlayerLootPicker : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private PlayerInteractInput _interactInput;

    [Header("Scan")]
    [SerializeField] private LayerMask _lootMask;
    [SerializeField] private float _autoPickupRadius = 1.5f;
    [SerializeField] private float _manualPickupRadius = 2.5f;
    [SerializeField] private int _maxHits = 16;

    LootPickup _lootPickup;

    void Start()
    {
        LootScanner autoScanner = new LootScanner(transform, _autoPickupRadius, _lootMask, _maxHits);
        LootScanner manualScanner = new LootScanner(transform, _manualPickupRadius, _lootMask, _maxHits);
        _lootPickup = new LootPickup(_playerInventory.Inventory, autoScanner, manualScanner, _maxHits);
        _lootPickup.PickupFailed += OnPickupFailed;
        _lootPickup.PickupSucceeded += OnPickupSucceeded;
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
        if (_lootPickup == null)
            return;

        _lootPickup.PickupFailed -= OnPickupFailed;
        _lootPickup.PickupSucceeded -= OnPickupSucceeded;
    }

    void Update()
    {
        if (_lootPickup == null)
            return;

        _lootPickup.TickAutomatic();
    }

    void OnInteractPressed()
    {
        if (_lootPickup == null)
            return;

        _lootPickup.TryManualInteract();
    }

    void OnPickupFailed(InventoryAddFailReason reason)
    {
        if (reason == InventoryAddFailReason.ExceedsWeight)
            Debug.Log("Не влезает по весу");
        else
            Debug.Log($"Подбор не удался: {reason}");
    }

    void OnPickupSucceeded(ItemDefinition item, int amount)
    {
        Debug.Log($"Подобрано: {item.DisplayName} x{amount}");
    }
}