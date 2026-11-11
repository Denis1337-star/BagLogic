using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int _maxWeight = 75;

    public Inventory Inventory { get; private set; }

    void Awake()
    {
        Inventory = new Inventory(_maxWeight);
    }
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
            Debug.Log($"Weight {Inventory.CurrentWeight}/{Inventory.MaxWeight}, stacks {Inventory.Slots.Count}");
    }
}