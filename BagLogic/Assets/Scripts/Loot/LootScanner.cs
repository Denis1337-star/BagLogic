using UnityEngine;

public class LootScanner
{
    readonly Transform _originTransform;
    readonly float _radius;
    readonly LayerMask _lootMask;
    readonly Collider[] _maxOverlapResults;

    public LootScanner(Transform originTransform, float radius, LayerMask lootMask, int maxOverlapResults)
    {
        _originTransform = originTransform;
        _radius = radius;
        _lootMask = lootMask;
        _maxOverlapResults = new Collider[maxOverlapResults];
    }

    public int Collect(LootWorldItem[] resultsLootWorldItem)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(_originTransform.position, _radius, _maxOverlapResults, _lootMask);
        int resultCount = 0;

        for (int i = 0; i < hitCount; i++)
        {
            LootWorldItem lootWorldItem = _maxOverlapResults[i].GetComponent<LootWorldItem>();
            if (lootWorldItem == null || !lootWorldItem.IsAvailable)
                continue;

            if (resultCount >= resultsLootWorldItem.Length)
                break;

            resultsLootWorldItem[resultCount] = lootWorldItem;
            resultCount++;
        }

        return resultCount;
    }

    public LootWorldItem FindClosest(LootWorldItem[] bufferLootWorldItem, PickupMode pickupMode)
    {
        int count = Collect(bufferLootWorldItem);
        LootWorldItem closestLootWorldItem = null;
        float best = float.MaxValue;
        Vector3 originTransform = _originTransform.position;

        for (int i = 0; i < count; i++)
        {
            LootWorldItem loot = bufferLootWorldItem[i];
            if (loot.ItemConfig.PickupMode != pickupMode)
                continue;

            float sqr = (loot.transform.position - originTransform).sqrMagnitude;
            if (sqr >= best)
                continue;

            best = sqr;
            closestLootWorldItem = loot;
        }

        return closestLootWorldItem;
    }
}