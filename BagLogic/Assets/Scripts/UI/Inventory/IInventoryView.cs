
public interface IInventoryView
{
    void SetVisible(bool visible);
    void SetWeight(float current, float max);
    void SetSlotCount(int count);
    void BindSlot(int index, ItemDefinition itemConfig, int amount, bool canDisassemble);
    void ClearSlot(int index);
}