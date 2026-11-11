using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private float _maxWeight = 75f;
    public Inventory Inventory { get; private set; }
    void Awake()
    {
        Inventory = new Inventory(_maxWeight);
    }
}