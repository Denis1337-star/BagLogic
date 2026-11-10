
public readonly struct InventoryAddResult
{
    public readonly bool Success;
    public readonly InventoryAddFailReason FailReason;
    public readonly int AddedAmount;

    public InventoryAddResult(bool success, InventoryAddFailReason failReason, int addedAmount)
    {
        Success = success;
        FailReason = failReason;
        AddedAmount = addedAmount;
    }

    public static InventoryAddResult Succeeded(int addedAmount)
    {
        return new InventoryAddResult(true, InventoryAddFailReason.None, addedAmount);
    }

    public static InventoryAddResult Failed(InventoryAddFailReason reason)
    {
        return new InventoryAddResult(false, reason, 0);
    }
}