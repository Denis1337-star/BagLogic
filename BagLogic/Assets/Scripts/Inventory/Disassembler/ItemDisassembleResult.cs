
public readonly struct ItemDisassembleResult
{
    public readonly bool Success;
    public readonly ItemDisassembleFailReason FailReason;

    public ItemDisassembleResult(bool success, ItemDisassembleFailReason failReason)
    {
        Success = success;
        FailReason = failReason;
    }

    public static ItemDisassembleResult Succeeded()
    {
        return new ItemDisassembleResult(true, ItemDisassembleFailReason.None);
    }

    public static ItemDisassembleResult Failed(ItemDisassembleFailReason reason)
    {
        return new ItemDisassembleResult(false, reason);
    }
}