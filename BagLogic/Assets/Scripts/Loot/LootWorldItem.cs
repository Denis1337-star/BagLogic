using UnityEngine;

public class LootWorldItem : MonoBehaviour
{
    [SerializeField] private ItemDefinition _itemConfig;
    [SerializeField] private int _amount = 1;

    public ItemDefinition ItemConfig => _itemConfig;
    public int Amount => _amount;
    public bool IsAvailable
    {
        get
        {
            if (_itemConfig == null)
                return false;

            if (_amount <= 0)
                return false;

            if (!isActiveAndEnabled)
                return false;

            return true;
        }
    }

    public void Despawn()
    {
        _amount = 0;
        Destroy(gameObject);
    }
}