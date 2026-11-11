using UnityEngine;

[CreateAssetMenu(menuName = "Game/Items/Item Definition", fileName = "Item_")]
public class ItemDefinition : ScriptableObject
{
    [SerializeField] private string _displayName;
    [SerializeField] private ItemCategory _itemCategory;
    [SerializeField] private Sprite _iconSprite;
    [SerializeField] private GameObject _worldPrefab;

    [Header("Inventory")]
    [SerializeField] private int _weight = 1;
    [SerializeField] private int _maxStack = 1;
    [SerializeField] private PickupMode _pickupMode = PickupMode.Automatic;

    [Header("Disassemble")]
    [SerializeField] private ItemAmount[] _disassembleResults;

    public string DisplayName => _displayName;
    public ItemCategory ItemCategory => _itemCategory;
    public Sprite IconSprite => _iconSprite;
    public GameObject WorldPrefab => _worldPrefab;
    public float Weight => _weight;
    public int MaxStack => _maxStack;
    public PickupMode PickupMode => _pickupMode;
    public ItemAmount[] DisassembleResults => _disassembleResults;
}