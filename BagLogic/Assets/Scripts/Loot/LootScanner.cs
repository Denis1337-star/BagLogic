using UnityEngine;

public class LootScanner
{
    readonly Transform _origin;
    readonly float _radius;
    readonly LayerMask _lootMask;
    readonly Collider[] _hits;

    public LootScanner(Transform origin, float radius, LayerMask lootMask, int maxHits)
    {
        _origin = origin;
        _radius = radius;
        _lootMask = lootMask;
        _hits = new Collider[maxHits];
    }

    public int Collect(LootWorldItem[] results)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(_origin.position, _radius, _hits, _lootMask);
        int resultCount = 0;

        for (int i = 0; i < hitCount; i++)
        {
            LootWorldItem loot = _hits[i].GetComponentInParent<LootWorldItem>();
            if (loot == null || !loot.IsAvailable)
                continue;

            if (resultCount >= results.Length)
                break;

            results[resultCount] = loot;
            resultCount++;
        }

        return resultCount;
    }

    public LootWorldItem FindClosest(LootWorldItem[] buffer, PickupMode mode)
    {
        int count = Collect(buffer);
        LootWorldItem closest = null;
        float bestSqr = float.MaxValue;
        Vector3 origin = _origin.position;

        for (int i = 0; i < count; i++)
        {
            LootWorldItem loot = buffer[i];
            if (loot.Item.PickupMode != mode)
                continue;

            float sqr = (loot.transform.position - origin).sqrMagnitude;
            if (sqr >= bestSqr)
                continue;

            bestSqr = sqr;
            closest = loot;
        }

        return closest;
    }
}