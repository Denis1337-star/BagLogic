using UnityEngine;

public class LootWorldItem : MonoBehaviour
{
    [SerializeField] private ItemDefinition _item;
    [SerializeField] private int _amount = 1;

    public ItemDefinition Item => _item;
    public int Amount => _amount;
    public bool IsAvailable
    {
        get
        {
            if (_item == null)
                return false;

            if (_amount <= 0)
                return false;

            if (!isActiveAndEnabled)
                return false;

            return true;
        }
    }

    public void Consume()
    {
        _amount = 0;
        Destroy(gameObject);
    }
}